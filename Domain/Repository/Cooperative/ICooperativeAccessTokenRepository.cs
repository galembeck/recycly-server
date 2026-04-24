using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository.Cooperative;

public interface ICooperativeAccessTokenRepository : IRepository<CooperativeAccessToken>
{
    Task<CooperativeAccessToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
}
