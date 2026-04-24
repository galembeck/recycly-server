using Domain.Data.Entities;
using Domain.Exceptions;
using Domain.Repository.Responsible;
using Microsoft.EntityFrameworkCore;
using Repository.Repository._Base;

namespace Repository.Repository.Responsible;

public class ResponsibleAccessTokenRepository : BaseRepository<ResponsibleAccessToken>, IResponsibleAccessTokenRepository
{
    public ResponsibleAccessTokenRepository(AppDbContext context) : base(context, context.ResponsibleAccessTokens) { }

    public async Task<ResponsibleAccessToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _entity
                .Where(x => x.Id == token && x.DeletedAt == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex) { throw new PersistenceException(ex); }
    }
}
