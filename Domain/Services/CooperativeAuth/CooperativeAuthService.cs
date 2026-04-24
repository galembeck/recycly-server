using Domain.Constants;
using Domain.Data.Entities;
using Domain.Data.Models.Auth;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository.Cooperative;
using Domain.Utils;
using Hangfire;

namespace Domain.Services.CooperativeAuth;

public class CooperativeAuthService : ICooperativeAuthService
{
    private readonly ICooperativeRepository _cooperativeRepository;
    private readonly ICooperativeAccessTokenRepository _accessTokenRepository;
    private readonly ICooperativeRefreshTokenRepository _refreshTokenRepository;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public CooperativeAuthService(
        ICooperativeRepository cooperativeRepository,
        ICooperativeAccessTokenRepository accessTokenRepository,
        ICooperativeRefreshTokenRepository refreshTokenRepository,
        IBackgroundJobClient backgroundJobClient)
    {
        _cooperativeRepository = cooperativeRepository;
        _accessTokenRepository = accessTokenRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<Tokens> RegisterAsync(string name, string email, string cpf, string password, DateOnly birthDate, List<string> phones, CancellationToken cancellationToken = default)
    {
        var existingByEmail = await _cooperativeRepository.GetByEmailAsync(email, cancellationToken);
        if (existingByEmail is not null)
            throw new BusinessException(BusinessErrorMessage.COOPERATIVE_WITH_REPEAT_REGISTRATION_EMAIL);

        var existingByCpf = await _cooperativeRepository.GetByCpfAsync(cpf, cancellationToken);
        if (existingByCpf is not null)
            throw new BusinessException(BusinessErrorMessage.COOPERATIVE_WITH_REPEAT_REGISTRATION_CPF);

        var cooperative = new Cooperative
        {
            Name = name,
            Email = email,
            Cpf = cpf,
            Password = StringUtil.SHA512(password),
            BirthDate = birthDate,
            Phones = phones,
        };

        var saved = await _cooperativeRepository.InsertAsync(cooperative);

        return await GenerateTokensAsync(saved.Id);
    }

    public async Task<Tokens> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var cooperative = await _cooperativeRepository.GetByEmailAsync(email, cancellationToken);

        if (cooperative is null)
            throw new BusinessException(BusinessErrorMessage.COOPERATIVE_NOT_FOUND);

        if (cooperative.Password != StringUtil.SHA512(password))
            throw new BusinessException(BusinessErrorMessage.COOPERATIVE_NOT_FOUND_OR_INVALID_PASSWORD);

        var tokens = await GenerateTokensAsync(cooperative.Id);

        await _cooperativeRepository.UpdatePartialAsync(
            new Cooperative { Id = cooperative.Id },
            c => c.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RefreshAsync(string refreshTokenId, CancellationToken cancellationToken = default)
    {
        var refresh = await _refreshTokenRepository.GetAsync(refreshTokenId, cancellationToken);
        if (refresh is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var cooperative = await _cooperativeRepository.GetAsync(refresh.CooperativeId, cancellationToken);
        if (cooperative is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        if (refresh.ExpiresAt < DateTimeOffset.UtcNow)
            throw new AuthenticationException(AuthenticationErrorMessage.TOKEN_EXPIRED);

        var tokens = await GenerateTokensAsync(refresh.CooperativeId);

        await _cooperativeRepository.UpdatePartialAsync(
            new Cooperative { Id = cooperative.Id },
            c => c.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RevokeAsync(string accessTokenId, string refreshTokenId, Cooperative actor, CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessTokenRepository.GetByTokenAsync(accessTokenId, cancellationToken);
        var refreshToken = await _refreshTokenRepository.GetAsync(refreshTokenId, cancellationToken);

        if (accessToken is null)
            throw new AuthenticationException(AuthenticationErrorMessage.ACCESSTOKEN_NOT_FOUND);

        _ = await _accessTokenRepository.UpdatePartialAsync(
            new CooperativeAccessToken { Id = accessToken.Id },
            at =>
            {
                at.UpdatedBy = actor.Id;
                at.DeletedAt = DateTimeOffset.UtcNow;
                at.ExpiresAt = DateTimeOffset.MinValue;
            },
            actor.Id);

        if (refreshToken is null)
            throw new AuthenticationException(AuthenticationErrorMessage.REFRESHTOKEN_NOT_FOUND);

        refreshToken.UpdatedBy = actor.Id;
        refreshToken.DeletedAt = DateTimeOffset.UtcNow;
        refreshToken.ExpiresAt = DateTimeOffset.MinValue;

        refreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken, actor.Id);

        return new Tokens
        {
            AccessToken = accessToken.Id,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = refreshToken.Id,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };
    }

    public async Task SendPasswordRecoveryAsync(string email, CancellationToken cancellationToken = default)
    {
        var cooperative = await _cooperativeRepository.GetByEmailAsync(email, cancellationToken);

        if (cooperative is null)
            return;

        var token     = StringUtil.GenerateRandom(Constant.Settings.AuthSettings.RecoveryPasswordLength).ToUpper();
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(Constant.Settings.AuthSettings.RecoveryPasswordExpiration);

        await _cooperativeRepository.UpdatePartialAsync(
            new Cooperative { Id = cooperative.Id },
            c =>
            {
                c.PasswordChangeToken          = token;
                c.PasswordChangeTokenExpiresAt = expiresAt;
            });

        _backgroundJobClient.Enqueue<IEmailService>(s =>
            s.SendPasswordRecoveryEmailAsync(cooperative.Name, cooperative.Email, token, expiresAt.UtcDateTime));
    }

    public async Task<bool> VerifyPasswordRecoveryTokenAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var cooperative = await _cooperativeRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);
        return cooperative is not null;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var cooperative = await _cooperativeRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);

        if (cooperative is null)
            throw new BusinessException(BusinessErrorMessage.INVALID_DOCUMENT_OR_RECOVERY_PASSWORD_TOKEN);

        await _cooperativeRepository.UpdatePartialAsync(
            new Cooperative { Id = cooperative.Id },
            c =>
            {
                c.Password                     = StringUtil.SHA512(newPassword);
                c.PasswordChangeToken          = null;
                c.PasswordChangeTokenExpiresAt = null;
            });
    }

    #region .: PRIVATE METHODS :.

    private async Task<Tokens> GenerateTokensAsync(string cooperativeId)
    {
        var accessToken = await _accessTokenRepository.InsertAsync(new CooperativeAccessToken
        {
            CooperativeId = cooperativeId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.AccessTokenExpiration),
        });

        var refreshToken = await _refreshTokenRepository.InsertAsync(new CooperativeRefreshToken
        {
            CooperativeId = cooperativeId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.RefreshTokenExpiration),
        });

        return new Tokens
        {
            AccessToken = accessToken.Id,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = refreshToken.Id,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt,
        };
    }

    #endregion .: PRIVATE METHODS :.
}
