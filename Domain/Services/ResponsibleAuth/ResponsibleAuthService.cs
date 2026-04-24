using Domain.Constants;
using Domain.Data.Entities;
using Domain.Data.Models.Auth;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository.Responsible;
using Domain.Utils;
using Hangfire;

namespace Domain.Services.ResponsibleAuth;

public class ResponsibleAuthService : IResponsibleAuthService
{
    private readonly IResponsibleRepository _responsibleRepository;
    private readonly IResponsibleAccessTokenRepository _accessTokenRepository;
    private readonly IResponsibleRefreshTokenRepository _refreshTokenRepository;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ResponsibleAuthService(
        IResponsibleRepository responsibleRepository,
        IResponsibleAccessTokenRepository accessTokenRepository,
        IResponsibleRefreshTokenRepository refreshTokenRepository,
        IBackgroundJobClient backgroundJobClient)
    {
        _responsibleRepository = responsibleRepository;
        _accessTokenRepository = accessTokenRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<Tokens> RegisterAsync(string name, string email, string cpf, string password, DateOnly birthDate, List<string> phones, CancellationToken cancellationToken = default)
    {
        if (await _responsibleRepository.GetByEmailAsync(email, cancellationToken) is not null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_WITH_REPEAT_REGISTRATION_EMAIL);

        if (await _responsibleRepository.GetByCpfAsync(cpf, cancellationToken) is not null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_WITH_REPEAT_REGISTRATION_CPF);

        var responsible = new Responsible
        {
            Name = name,
            Email = email,
            Cpf = cpf,
            Password = StringUtil.SHA512(password),
            BirthDate = birthDate,
            Phones = phones,
        };

        var saved = await _responsibleRepository.InsertAsync(responsible);

        return await GenerateTokensAsync(saved.Id);
    }

    public async Task<Tokens> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var responsible = await _responsibleRepository.GetByEmailAsync(email, cancellationToken);

        if (responsible is null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_NOT_FOUND);

        if (responsible.Password != StringUtil.SHA512(password))
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_NOT_FOUND_OR_INVALID_PASSWORD);

        var tokens = await GenerateTokensAsync(responsible.Id);

        await _responsibleRepository.UpdatePartialAsync(
            new Responsible { Id = responsible.Id },
            r => r.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RefreshAsync(string refreshTokenId, CancellationToken cancellationToken = default)
    {
        var refresh = await _refreshTokenRepository.GetAsync(refreshTokenId, cancellationToken);
        if (refresh is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var responsible = await _responsibleRepository.GetAsync(refresh.ResponsibleId, cancellationToken);
        if (responsible is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        if (refresh.ExpiresAt < DateTimeOffset.UtcNow)
            throw new AuthenticationException(AuthenticationErrorMessage.TOKEN_EXPIRED);

        var tokens = await GenerateTokensAsync(refresh.ResponsibleId);

        await _responsibleRepository.UpdatePartialAsync(
            new Responsible { Id = responsible.Id },
            r => r.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RevokeAsync(string accessTokenId, string refreshTokenId, Responsible actor, CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessTokenRepository.GetByTokenAsync(accessTokenId, cancellationToken);
        if (accessToken is null)
            throw new AuthenticationException(AuthenticationErrorMessage.ACCESSTOKEN_NOT_FOUND);

        _ = await _accessTokenRepository.UpdatePartialAsync(
            new ResponsibleAccessToken { Id = accessToken.Id },
            at =>
            {
                at.UpdatedBy = actor.Id;
                at.DeletedAt = DateTimeOffset.UtcNow;
                at.ExpiresAt = DateTimeOffset.MinValue;
            },
            actor.Id);

        var refreshToken = await _refreshTokenRepository.GetAsync(refreshTokenId, cancellationToken);
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
        var responsible = await _responsibleRepository.GetByEmailAsync(email, cancellationToken);

        if (responsible is null)
            return;

        var token     = StringUtil.GenerateRandom(Constant.Settings.AuthSettings.RecoveryPasswordLength).ToUpper();
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(Constant.Settings.AuthSettings.RecoveryPasswordExpiration);

        await _responsibleRepository.UpdatePartialAsync(
            new Responsible { Id = responsible.Id },
            r =>
            {
                r.PasswordChangeToken          = token;
                r.PasswordChangeTokenExpiresAt = expiresAt;
            });

        _backgroundJobClient.Enqueue<IEmailService>(s =>
            s.SendPasswordRecoveryEmailAsync(responsible.Name, responsible.Email, token, expiresAt.UtcDateTime));
    }

    public async Task<bool> VerifyPasswordRecoveryTokenAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var responsible = await _responsibleRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);
        return responsible is not null;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var responsible = await _responsibleRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);

        if (responsible is null)
            throw new BusinessException(BusinessErrorMessage.INVALID_DOCUMENT_OR_RECOVERY_PASSWORD_TOKEN);

        await _responsibleRepository.UpdatePartialAsync(
            new Responsible { Id = responsible.Id },
            r =>
            {
                r.Password                     = StringUtil.SHA512(newPassword);
                r.PasswordChangeToken          = null;
                r.PasswordChangeTokenExpiresAt = null;
            });
    }

    #region .: PRIVATE METHODS :.

    private async Task<Tokens> GenerateTokensAsync(string responsibleId)
    {
        var accessToken = await _accessTokenRepository.InsertAsync(new ResponsibleAccessToken
        {
            ResponsibleId = responsibleId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.AccessTokenExpiration),
        });

        var refreshToken = await _refreshTokenRepository.InsertAsync(new ResponsibleRefreshToken
        {
            ResponsibleId = responsibleId,
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
