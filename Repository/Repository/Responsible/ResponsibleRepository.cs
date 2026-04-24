using Domain.Exceptions;
using Domain.Repository.Responsible;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Responsible;

public class ResponsibleRepository : BaseRepository<Domain.Data.Entities.Responsible>, IResponsibleRepository
{
    public ResponsibleRepository(AppDbContext context) : base(context, context.Responsibles) { }

    public async Task<Domain.Data.Entities.Responsible> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Email == email && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e) { throw new PersistenceException(e); }
    }

    public async Task<Domain.Data.Entities.Responsible> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Cpf == cpf && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e) { throw new PersistenceException(e); }
    }

    public async Task<Domain.Data.Entities.Responsible> GetByEmailAndTokenAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Email == email
                    && x.PasswordChangeToken == token
                    && x.PasswordChangeTokenExpiresAt > DateTimeOffset.UtcNow
                    && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e) { throw new PersistenceException(e); }
    }
}
