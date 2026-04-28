using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;

namespace Domain.Services;

public class SaleService(ISaleRepository repository) : ISaleService(repository)
{
    public override async Task<Sale?> GetSaleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetWithMaterialsAsync(id, cancellationToken);
    }

    public override async Task<List<Sale>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByCooperativeAsync(cooperativeId, cancellationToken);
    }

    public override async Task<Sale> CreateSaleAsync(Sale sale, List<string> materialIds, string actorId, CancellationToken cancellationToken = default)
    {
        var saved = await _Repository.InsertAsync(sale, actorId);

        if (materialIds.Count > 0)
            await _Repository.UpdateMaterialsAsync(saved.Id, materialIds, cancellationToken);

        return await _Repository.GetWithMaterialsAsync(saved.Id, cancellationToken) ?? saved;
    }

    public override async Task DeleteSaleAsync(string id, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.SALE_NOT_FOUND);

        await _Repository.DeleteAsync(existing, actorId);
    }
}
