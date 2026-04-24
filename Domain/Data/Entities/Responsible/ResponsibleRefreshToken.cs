using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBResponsibleRefreshToken")]
public class ResponsibleRefreshToken : BaseEntity, IBaseEntity<ResponsibleRefreshToken>
{
    public string ResponsibleId { get; set; }
    public Responsible Responsible { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }



    #region .: HELPER METHODS :.

    public ResponsibleRefreshToken WithoutRelations(ResponsibleRefreshToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new ResponsibleRefreshToken
        {
            ResponsibleId = entity.ResponsibleId,
            ExpiresAt = entity.ExpiresAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: HELPER METHODS :.
}
