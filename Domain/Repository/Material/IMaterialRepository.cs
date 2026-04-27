using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository;

public interface IMaterialRepository : IRepository<Material>
{
    Task<List<Material>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
