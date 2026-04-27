using Domain.Data.Entities;
using Domain.Repository;
using Domain.SearchParameters._Base;
using Domain.Services._Base;

namespace Domain.Services;

public abstract class ICollectionPointService : IService<CollectionPoint, ICollectionPointRepository, BaseSearchParameter>
{
    public ICollectionPointService(ICollectionPointRepository repository) : base(repository) { }

    public abstract Task<CollectionPoint> CreateWithGeocodingAsync(CollectionPoint entity, List<string> materialIds, string actorId, CancellationToken cancellationToken = default);
    public abstract Task<CollectionPoint> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default);
    public abstract Task<List<CollectionPoint>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default);
    public abstract Task<List<CollectionPoint>> GetAllPublicAsync(CancellationToken cancellationToken = default);
    public abstract Task<CollectionPoint> UpdateCollectionPointAsync(string id, CollectionPoint entity, string actorId, CancellationToken cancellationToken = default);
    public abstract Task UpdateMaterialsAsync(string id, List<string> materialIds, string actorId, CancellationToken cancellationToken = default);
    public abstract Task DeleteCollectionPointAsync(string id, string actorId, CancellationToken cancellationToken = default);
}
