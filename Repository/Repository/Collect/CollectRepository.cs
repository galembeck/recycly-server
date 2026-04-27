using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository;

public class CollectRepository : BaseRepository<Collect>, ICollectRepository
{
    public CollectRepository(AppDbContext context) : base(context, context.Collects) { }

    public async Task<List<Collect>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(c => c.CollectionPoint)
                .Include(c => c.Material)
                .Where(c => c.UserId == userId && c.DeletedAt == null)
                .OrderByDescending(c => c.CollectedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<List<Collect>> GetByCollectionPointAsync(string collectionPointId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(c => c.User)
                .Include(c => c.Material)
                .Where(c => c.CollectionPointId == collectionPointId && c.DeletedAt == null)
                .OrderByDescending(c => c.CollectedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<List<Collect>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(c => c.CollectionPoint)
                .Include(c => c.Material)
                .Where(c => c.CollectionPoint!.CooperativeId == cooperativeId && c.DeletedAt == null)
                .OrderByDescending(c => c.CollectedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
