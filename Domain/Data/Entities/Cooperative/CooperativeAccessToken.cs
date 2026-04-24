using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBCooperativeAccessToken")]
public class CooperativeAccessToken : BaseEntity, IBaseEntity<CooperativeAccessToken>
{
    public string CooperativeId { get; set; }
    public Cooperative Cooperative { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }



    #region .: HELPER METHODS :.

    public CooperativeAccessToken WithoutRelations(CooperativeAccessToken entity)
    {
        if (entity == null)
            return null;

        var newEntity = new CooperativeAccessToken
        {
            CooperativeId = entity.CooperativeId,
            ExpiresAt = entity.ExpiresAt,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: HELPER METHODS :.
}
