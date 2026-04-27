using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository;

public class SaleRepository : BaseRepository<Sale>, ISaleRepository
{
    public SaleRepository(AppDbContext context) : base(context, context.Sales) { }

    public async Task<List<Sale>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(s => s.Materials)
                .Where(s => s.CooperativeId == cooperativeId && s.DeletedAt == null)
                .OrderByDescending(s => s.SoldAt)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<Sale?> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Include(s => s.Materials)
                .Where(s => s.Id == id && s.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task UpdateMaterialsAsync(string saleId, List<string> materialIds, CancellationToken cancellationToken = default)
    {
        try
        {
            var sale = await _entity
                .Include(s => s.Materials)
                .Where(s => s.Id == saleId && s.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new PersistenceException(new Exception($"Sale {saleId} not found"));

            var materials = await _context.Set<Material>()
                .Where(m => materialIds.Contains(m.Id))
                .ToListAsync(cancellationToken);

            sale.Materials?.Clear();
            sale.Materials ??= [];
            sale.Materials.AddRange(materials);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
