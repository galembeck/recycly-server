using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface ISaleRepository : IRepository<Sale>
{
    Task<List<Sale>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default);
    Task<Sale?> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default);
    Task UpdateMaterialsAsync(string saleId, List<string> materialIds, CancellationToken cancellationToken = default);
}
