using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBSale")]
public class Sale : BaseEntity, IBaseEntity<Sale>
{
    public string BuyerName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset SoldAt { get; set; }
    public string? Notes { get; set; }
    public string CooperativeId { get; set; } = string.Empty;

    public User? Cooperative { get; set; }
    public List<Material>? Materials { get; set; }

    #region .: METHODS :.

    public Sale WithoutRelations(Sale entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new Sale
        {
            BuyerName = entity.BuyerName,
            WeightKg = entity.WeightKg,
            Price = entity.Price,
            SoldAt = entity.SoldAt,
            Notes = entity.Notes,
            CooperativeId = entity.CooperativeId,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
