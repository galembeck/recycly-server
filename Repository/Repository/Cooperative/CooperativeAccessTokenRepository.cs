using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository.Cooperative;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Cooperative;

public class CooperativeAccessTokenRepository : BaseRepository<CooperativeAccessToken>, ICooperativeAccessTokenRepository
{
    public CooperativeAccessTokenRepository(AppDbContext context) : base(context, context.CooperativeAccessTokens) { }

    public async Task<CooperativeAccessToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Id == token && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new PersistenceException(ex);
        }
    }
}
