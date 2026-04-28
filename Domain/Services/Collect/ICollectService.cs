using Domain.Data.Entities;
using Domain.Repository;
using Domain.SearchParameters._Base;
using Domain.Services._Base;

namespace Domain.Services;

public abstract class ICollectService : IService<Collect, ICollectRepository, BaseSearchParameter>
{
    public ICollectService(ICollectRepository repository) : base(repository) { }

    public abstract Task<Collect?> GetCollectByIdAsync(string id, CancellationToken cancellationToken = default);
    public abstract Task<Collect> CreateCollectAsync(Collect collect, string actorId, CancellationToken cancellationToken = default);
    public abstract Task<List<Collect>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    public abstract Task<List<Collect>> GetByCollectionPointAsync(string collectionPointId, CancellationToken cancellationToken = default);
    public abstract Task<List<Collect>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default);
    public abstract Task DeleteCollectAsync(string id, string userId, CancellationToken cancellationToken = default);
}
