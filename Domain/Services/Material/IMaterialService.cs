using Domain.Data.Entities;
using Domain.Repository;
using Domain.SearchParameters._Base;
using Domain.Services._Base;

namespace Domain.Services;

public abstract class IMaterialService : IService<Material, IMaterialRepository, BaseSearchParameter>
{
    public IMaterialService(IMaterialRepository repository) : base(repository) { }

    public abstract Task<List<Material>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    public abstract Task<Material> CreateMaterialAsync(Material material, string actorId, CancellationToken cancellationToken = default);
    public abstract Task<Material> UpdateMaterialAsync(string id, Material material, string actorId, CancellationToken cancellationToken = default);
    public abstract Task DeleteMaterialAsync(string id, string actorId, CancellationToken cancellationToken = default);
}
