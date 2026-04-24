using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository.Cooperative;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Cooperative;

public class CooperativeRepository : BaseRepository<Domain.Data.Entities.Cooperative>, ICooperativeRepository
{
    public CooperativeRepository(AppDbContext context) : base(context, context.Cooperatives) { }

    public async Task<Domain.Data.Entities.Cooperative> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Email == email && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<Domain.Data.Entities.Cooperative> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Cpf == cpf && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }

    public async Task<Domain.Data.Entities.Cooperative> GetByEmailAndTokenAsync(string email, string token, CancellationToken cancellationToken = default)
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
        catch (Exception e)
        {
            throw new PersistenceException(e);
        }
    }
}
