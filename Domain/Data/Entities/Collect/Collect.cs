using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBCollect")]
public class Collect : BaseEntity, IBaseEntity<Collect>
{
    public string CollectionPointId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string MaterialId { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public DateTimeOffset CollectedAt { get; set; }
    public string? Notes { get; set; }

    public CollectionPoint? CollectionPoint { get; set; }
    public User? User { get; set; }
    public Material? Material { get; set; }

    #region .: METHODS :.

    public Collect WithoutRelations(Collect entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new Collect()
        {
            CollectionPointId = entity.CollectionPointId,
            UserId = entity.UserId,
            MaterialId = entity.MaterialId,
            WeightKg = entity.WeightKg,
            CollectedAt = entity.CollectedAt,
            Notes = entity.Notes,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
