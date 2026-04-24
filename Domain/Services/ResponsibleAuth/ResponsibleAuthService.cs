using Domain.Constants;
using Domain.Data.Entities;
using Domain.Data.Models.Auth;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Repository.User;
using Domain.Utils;
using Hangfire;

namespace Domain.Services.ResponsibleAuth;

public class ResponsibleAuthService : IResponsibleAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ResponsibleAuthService(
        IUserRepository userRepository,
        IAccessTokenRepository accessTokenRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IBackgroundJobClient backgroundJobClient)
    {
        _userRepository = userRepository;
        _accessTokenRepository = accessTokenRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task<Tokens> RegisterAsync(string name, string email, string cpf, string password, DateOnly birthDate, List<string> phones, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.GetByEmailAsync(email, cancellationToken) is not null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_WITH_REPEAT_REGISTRATION_EMAIL);

        if (await _userRepository.GetByDocumentAsync(cpf, cancellationToken) is not null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_WITH_REPEAT_REGISTRATION_CPF);

        var user = new User
        {
            Name = name,
            Email = email,
            Document = cpf,
            Password = StringUtil.SHA512(password),
            BirthDate = birthDate,
            Phones = phones,
            Cellphone = phones.FirstOrDefault() ?? string.Empty,
        };

        var saved = await _userRepository.InsertAsync(user);

        return await GenerateTokensAsync(saved.Id);
    }

    public async Task<Tokens> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_NOT_FOUND);

        if (user.Password != StringUtil.SHA512(password))
            throw new BusinessException(BusinessErrorMessage.RESPONSIBLE_NOT_FOUND_OR_INVALID_PASSWORD);

        var tokens = await GenerateTokensAsync(user.Id);

        await _userRepository.UpdatePartialAsync(
            new User { Id = user.Id },
            u => u.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RefreshAsync(string refreshTokenId, CancellationToken cancellationToken = default)
    {
        var refresh = await _refreshTokenRepository.GetAsync(refreshTokenId, cancellationToken);
        if (refresh is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        var user = await _userRepository.GetAsync(refresh.UserId, cancellationToken);
        if (user is null)
            throw new AuthenticationException(AuthenticationErrorMessage.UNAUTHORIZED);

        if (refresh.ExpiresAt < DateTimeOffset.UtcNow)
            throw new AuthenticationException(AuthenticationErrorMessage.TOKEN_EXPIRED);

        var tokens = await GenerateTokensAsync(refresh.UserId);

        await _userRepository.UpdatePartialAsync(
            new User { Id = user.Id },
            u => u.LastAccessAt = DateTimeOffset.UtcNow);

        return tokens;
    }

    public async Task<Tokens> RevokeAsync(string accessTokenId, string refreshTokenId, User actor, CancellationToken cancellationToken = default)
    {
        var accessToken = await _accessTokenRepository.GetByToken(accessTokenId, cancellationToken);
        if (accessToken is null)
            throw new AuthenticationException(AuthenticationErrorMessage.ACCESSTOKEN_NOT_FOUND);

        _ = await _accessTokenRepository.UpdatePartialAsync(
            new AccessToken { Id = accessToken.Id },
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
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            return;

        var token     = StringUtil.GenerateRandom(Constant.Settings.AuthSettings.RecoveryPasswordLength).ToUpper();
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(Constant.Settings.AuthSettings.RecoveryPasswordExpiration);

        await _userRepository.UpdatePartialAsync(
            new User { Id = user.Id },
            u =>
            {
                u.PasswordChangeToken          = token;
                u.PasswordChangeTokenExpiresAt = expiresAt;
            });

        _backgroundJobClient.Enqueue<IEmailService>(s =>
            s.SendPasswordRecoveryEmailAsync(user.Name, user.Email, token, expiresAt.UtcDateTime));
    }

    public async Task<bool> VerifyPasswordRecoveryTokenAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);
        return user is not null;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAndTokenAsync(email, token, cancellationToken);

        if (user is null)
            throw new BusinessException(BusinessErrorMessage.INVALID_DOCUMENT_OR_RECOVERY_PASSWORD_TOKEN);

        await _userRepository.UpdatePartialAsync(
            new User { Id = user.Id },
            u =>
            {
                u.Password                     = StringUtil.SHA512(newPassword);
                u.PasswordChangeToken          = null;
                u.PasswordChangeTokenExpiresAt = null;
            });
    }

    #region .: PRIVATE METHODS :.

    private async Task<Tokens> GenerateTokensAsync(string userId)
    {
        var accessToken = await _accessTokenRepository.InsertAsync(new AccessToken
        {
            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(Constant.Settings.AuthSettings.AccessTokenExpiration),
        });

        var refreshToken = await _refreshTokenRepository.InsertAsync(new RefreshToken
        {
            UserId = userId,
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
