using Domain.Repository._Base;

namespace Domain.Repository.Responsible;

public interface IResponsibleRepository : IRepository<Data.Entities.Responsible>
{
    Task<Data.Entities.Responsible> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Data.Entities.Responsible> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<Data.Entities.Responsible> GetByEmailAndTokenAsync(string email, string token, CancellationToken cancellationToken = default);
}
