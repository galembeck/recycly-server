using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface ICollectionPointRepository : IRepository<CollectionPoint>
{
    Task<CollectionPoint?> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default);
    Task<List<CollectionPoint>> GetByCooperativeIdAsync(string cooperativeId, CancellationToken cancellationToken = default);
    Task<List<CollectionPoint>> GetAllWithMaterialsAsync(CancellationToken cancellationToken = default);
    Task UpdateMaterialsAsync(string id, List<string> materialIds, CancellationToken cancellationToken = default);
}
