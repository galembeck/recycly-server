using Domain.Data.Entities;
using Domain.Repository._Base;

namespace Domain.Repository.Cooperative;

public interface ICooperativeRepository : IRepository<Domain.Data.Entities.Cooperative>
{
    Task<Domain.Data.Entities.Cooperative> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Domain.Data.Entities.Cooperative> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<Domain.Data.Entities.Cooperative> GetByEmailAndTokenAsync(string email, string token, CancellationToken cancellationToken = default);
}
