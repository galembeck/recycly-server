using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository.Cooperative;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Cooperative;

public class CooperativeRefreshTokenRepository : BaseRepository<CooperativeRefreshToken>, ICooperativeRefreshTokenRepository
{
    public CooperativeRefreshTokenRepository(AppDbContext context) : base(context, context.CooperativeRefreshTokens) { }

    public async Task<CooperativeRefreshToken> GetByCooperativeIdAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.CooperativeId == cooperativeId && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new PersistenceException(ex);
        }
    }
}
