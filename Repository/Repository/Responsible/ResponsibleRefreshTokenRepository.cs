using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository.Responsible;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Responsible;

public class ResponsibleRefreshTokenRepository : BaseRepository<ResponsibleRefreshToken>, IResponsibleRefreshTokenRepository
{
    public ResponsibleRefreshTokenRepository(AppDbContext context) : base(context, context.ResponsibleRefreshTokens) { }

    public async Task<ResponsibleRefreshToken> GetByResponsibleIdAsync(string responsibleId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.ResponsibleId == responsibleId && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex) { throw new PersistenceException(ex); }
    }
}
