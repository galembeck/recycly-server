using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Services.Geocoding;

namespace Domain.Services;

public class CollectionPointService(
    ICollectionPointRepository repository,
    IGeocodingService geocodingService) : ICollectionPointService(repository)
{
    private readonly IGeocodingService _geocodingService = geocodingService;

    public override async Task<CollectionPoint> CreateWithGeocodingAsync(
        CollectionPoint entity,
        List<string> materialIds,
        string actorId,
        CancellationToken cancellationToken = default)
    {
        var coordinates = await ResolveCoordinatesAsync(entity, cancellationToken);

        if (coordinates.HasValue)
        {
            entity.Latitude = coordinates.Value.Latitude;
            entity.Longitude = coordinates.Value.Longitude;
        }

        var saved = await _Repository.InsertAsync(entity, actorId);

        if (materialIds.Count > 0)
            await _Repository.UpdateMaterialsAsync(saved.Id, materialIds, cancellationToken);

        return await _Repository.GetWithMaterialsAsync(saved.Id, cancellationToken) ?? saved;
    }

    public override async Task<CollectionPoint> GetWithMaterialsAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _Repository.GetWithMaterialsAsync(id, cancellationToken);
        if (entity is null)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_FOUND);

        return entity;
    }

    public override async Task<List<CollectionPoint>> GetByCooperativeAsync(string cooperativeId, CancellationToken cancellationToken = default)
    {
        return await _Repository.GetByCooperativeIdAsync(cooperativeId, cancellationToken);
    }

    public override async Task<List<CollectionPoint>> GetAllPublicAsync(CancellationToken cancellationToken = default)
    {
        return await _Repository.GetAllWithMaterialsAsync(cancellationToken);
    }

    public override async Task<CollectionPoint> UpdateCollectionPointAsync(string id, CollectionPoint entity, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetWithMaterialsAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_FOUND);

        existing.Name = entity.Name;
        existing.Description = entity.Description;
        existing.ZipCode = entity.ZipCode;
        existing.Address = entity.Address;
        existing.Number = entity.Number;
        existing.Complement = entity.Complement;
        existing.Neighborhood = entity.Neighborhood;
        existing.City = entity.City;
        existing.State = entity.State;
        existing.OpeningTime = entity.OpeningTime;
        existing.ClosingTime = entity.ClosingTime;
        existing.Phone = entity.Phone;

        var coordinates = await ResolveCoordinatesAsync(existing, cancellationToken);
        if (coordinates.HasValue)
        {
            existing.Latitude = coordinates.Value.Latitude;
            existing.Longitude = coordinates.Value.Longitude;
        }

        return await _Repository.UpdateAsync(existing, actorId);
    }

    public override async Task UpdateMaterialsAsync(string id, List<string> materialIds, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_FOUND);

        await _Repository.UpdateMaterialsAsync(id, materialIds, cancellationToken);
    }

    public override async Task DeleteCollectionPointAsync(string id, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.COLLECTION_POINT_NOT_FOUND);

        await _Repository.DeleteAsync(existing, actorId);
    }

    private async Task<(string Latitude, string Longitude)?> ResolveCoordinatesAsync(CollectionPoint entity, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(entity.ZipCode))
        {
            var byZip = await _geocodingService.GetCoordinatesAsync($"{entity.ZipCode}, Brasil", cancellationToken);
            if (byZip.HasValue)
                return byZip;
        }

        var fullAddress = $"{entity.Address}, {entity.Number}, {entity.City}, {entity.State}, Brasil";
        return await _geocodingService.GetCoordinatesAsync(fullAddress, cancellationToken);
    }
}
