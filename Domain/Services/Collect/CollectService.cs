using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;

namespace Domain.Services;

public class CollectService(
    ICollectRepository repository,
    ICollectionPointRepository collectionPointRepository,
    IMaterialRepository materialRepository) : ICollectService(repository)
{
    private readonly ICollectionPointRepository _collectionPointRepository = collectionPointRepository;
    private readonly IMaterialRepository _materialRepository = materialRepository;

    public override async Task<Collect?> GetCollectByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByIdWithDetailsAsync(id, cancellationToken);
    }

    public override async Task<Collect> CreateCollectAsync(Collect collect, string actorId, CancellationToken cancellationToken = default)
    {
        var point = await _collectionPointRepository.GetWithMaterialsAsync(collect.CollectionPointId, cancellationToken);
        if (point is null)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_FOUND);

        var materialAccepted = point.Materials?.Any(m => m.Id == collect.MaterialId) ?? false;
        if (!materialAccepted)
            throw new BusinessException(BusinessErrorMessage.MATERIAL_NOT_ACCEPTED_AT_POINT);

        return await _Repository.InsertAsync(collect, actorId);
    }

    public override async Task<List<Collect>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByUserAsync(userId, cancellationToken);
    }

    public override async Task<List<Collect>> GetByCollectionPointAsync(string collectionPointId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByCollectionPointAsync(collectionPointId, cancellationToken);
    }

    public override async Task<List<Collect>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByCooperativeAsync(cooperativeId, cancellationToken);
    }

    public override async Task DeleteCollectAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.COLLECT_NOT_FOUND);

        if (existing.UserId != userId)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_BELONG_THIS_USER);

        await _Repository.DeleteAsync(existing, userId);
    }
}
