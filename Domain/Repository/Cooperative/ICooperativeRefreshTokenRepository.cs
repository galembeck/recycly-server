using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository.Cooperative;

public interface ICooperativeRefreshTokenRepository : IRepository<CooperativeRefreshToken>
{
    Task<CooperativeRefreshToken> GetByCooperativeIdAsync(string cooperativeId, CancellationToken cancellationToken = default);
}
