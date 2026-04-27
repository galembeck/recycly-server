using Domain.Data.Entities;
using Domain.Enumerators;
using Domain.Exceptions;
using Domain.Repository;

namespace Domain.Services;

public class MaterialService(IMaterialRepository repository) : IMaterialService(repository)
{
    public override async Task<List<Material>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _Repository.GetAllActiveAsync(cancellationToken);
    }

    public override async Task<Material> CreateMaterialAsync(Material material, string actorId, CancellationToken cancellationToken = default)
    {
        return await _Repository.InsertAsync(material, actorId);
    }

    public override async Task<Material> UpdateMaterialAsync(string id, Material material, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.MATERIAL_NOT_FOUND);

        existing.Name = material.Name;
        existing.Description = material.Description;
        existing.Color = material.Color;

        return await _Repository.UpdateAsync(existing, actorId);
    }

    public override async Task DeleteMaterialAsync(string id, string actorId, CancellationToken cancellationToken = default)
    {
        var existing = await _Repository.GetAsync(id, cancellationToken);
        if (existing is null)
            throw new BusinessException(BusinessErrorMessage.MATERIAL_NOT_FOUND);

        await _Repository.DeleteAsync(existing, actorId);
    }
}
