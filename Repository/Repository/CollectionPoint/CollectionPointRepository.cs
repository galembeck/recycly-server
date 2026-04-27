using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository;

public class CollectionPointRepository : BaseRepository<CollectionPoint>, ICollectionPointRepository
{
    public CollectionPointRepository(AppDbContext context) : base(context, context.CollectionPoints) { }

    public async Task<CollectionPoint?> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(cp => cp.Materials)
                .Where(cp => cp.Id == id && cp.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<List<CollectionPoint>> GetByCooperativeIdAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(cp => cp.Materials)
                .Where(cp => cp.CooperativeId == cooperativeId && cp.DeletedAt == null)
                .OrderByDescending(cp => cp.CreatedAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<List<CollectionPoint>> GetAllWithMaterialsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(cp => cp.Materials)
                .Where(cp => cp.DeletedAt == null)
                .OrderBy(cp => cp.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task UpdateMaterialsAsync(string id, List<string> materialIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var cp = await _context.CollectionPoints
                .Include(x => x.Materials)
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null, cancellationToken);

            if (cp is null)
                return;

            cp.Materials!.Clear();
            await _context.SaveChangesAsync(cancellationToken);

            var newMaterials = await _context.Materials
                .Where(m => materialIds.Contains(m.Id) && m.DeletedAt == null)
                .ToListAsync(cancellationToken);

            cp.Materials!.AddRange(newMaterials);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
