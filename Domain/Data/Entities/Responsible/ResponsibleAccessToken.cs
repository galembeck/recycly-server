using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBResponsibleAccessToken")]
public class ResponsibleAccessToken : BaseEntity, IBaseEntity<ResponsibleAccessToken>
{
    public string ResponsibleId { get; set; }
    public Responsible Responsible { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }



    #region .: HELPER METHODS :.

    public ResponsibleAccessToken WithoutRelations(ResponsibleAccessToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new ResponsibleAccessToken
        {
            ResponsibleId = entity.ResponsibleId,
            ExpiresAt = entity.ExpiresAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: HELPER METHODS :.
}
