using Domain.Data.Entities;
using Domain.Data.Models.Auth;

namespace Domain.Services.ResponsibleAuth;

public interface IResponsibleAuthService
{
    Task<Tokens> RegisterAsync(string name, string email, string cpf, string password, DateOnly birthDate, List<string> phones, CancellationToken cancellationToken = default);
    Task<Tokens> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Tokens> RefreshAsync(string refreshTokenId, CancellationToken cancellationToken = default);
    Task<Tokens> RevokeAsync(string accessTokenId, string refreshTokenId, User actor, CancellationToken cancellationToken = default);
    Task SendPasswordRecoveryAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> VerifyPasswordRecoveryTokenAsync(string email, string token, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
}
