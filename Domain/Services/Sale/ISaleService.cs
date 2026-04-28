using Domain.Data.Entities;
using Domain.Repository;
using Domain.SearchParameters._Base;
using Domain.Services._Base;

namespace Domain.Services;

public abstract class ISaleService : IService<Sale, ISaleRepository, BaseSearchParameter>
{
    public ISaleService(ISaleRepository repository) : base(repository) { }

    public abstract Task<Sale?> GetSaleByIdAsync(string id, CancellationToken cancellationToken = default);
    public abstract Task<List<Sale>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default);
    public abstract Task<Sale> CreateSaleAsync(Sale sale, List<string> materialIds, string actorId, CancellationToken cancellationToken = default);
    public abstract Task DeleteSaleAsync(string id, string actorId, CancellationToken cancellationToken = default);
}
