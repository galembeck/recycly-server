using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository.Responsible;

public interface IResponsibleAccessTokenRepository : IRepository<ResponsibleAccessToken>
{
    Task<ResponsibleAccessToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
}
