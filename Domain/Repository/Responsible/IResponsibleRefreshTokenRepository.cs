using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository.Responsible;

public interface IResponsibleRefreshTokenRepository : IRepository<ResponsibleRefreshToken>
{
    Task<ResponsibleRefreshToken> GetByResponsibleIdAsync(string responsibleId, CancellationToken cancellationToken = default);
}
