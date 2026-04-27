using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository;

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
    public MaterialRepository(AppDbContext context) : base(context, context.Materials) { }

    public async Task<List<Material>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.DeletedAt == null)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
