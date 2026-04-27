using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface ICollectRepository : IRepository<Collect>
{
    Task<List<Collect>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<Collect>> GetByCollectionPointAsync(string collectionPointId, CancellationToken cancellationToken = default);
    Task<List<Collect>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default);
}
