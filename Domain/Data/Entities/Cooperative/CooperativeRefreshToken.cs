using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBCooperativeRefreshToken")]
public class CooperativeRefreshToken : BaseEntity, IBaseEntity<CooperativeRefreshToken>
{
    public string CooperativeId { get; set; }
    public Cooperative Cooperative { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }



    #region .: HELPER METHODS :.

    public CooperativeRefreshToken WithoutRelations(CooperativeRefreshToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new CooperativeRefreshToken
        {
            CooperativeId = entity.CooperativeId,
            ExpiresAt = entity.ExpiresAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: HELPER METHODS :.
}
