using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBMaterial")]
public class Material : BaseEntity, IBaseEntity<Material>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public List<CollectionPoint>? CollectionPoints { get; set; }
    public List<Sale>? Sales { get; set; }

    #region .: METHODS :.

    public Material WithoutRelations(Material entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new Material()
        {
            Name = entity.Name,
            Description = entity.Description,
            Color = entity.Color,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
