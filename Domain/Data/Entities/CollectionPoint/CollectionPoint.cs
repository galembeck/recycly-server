using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBCollectionPoint")]
public class CollectionPoint : BaseEntity, IBaseEntity<CollectionPoint>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string CooperativeId { get; set; } = string.Empty;

    public User? Cooperative { get; set; }
    public List<Material>? Materials { get; set; }

    [NotMapped]
    public bool IsOpen
    {
        get
        {
            var now = TimeOnly.FromDateTime(DateTime.Now);
            return now >= OpeningTime && now <= ClosingTime;
        }
    }

    #region .: METHODS :.

    public CollectionPoint WithoutRelations(CollectionPoint entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new CollectionPoint()
        {
            Name = entity.Name,
            Description = entity.Description,
            ZipCode = entity.ZipCode,
            Address = entity.Address,
            Number = entity.Number,
            Complement = entity.Complement,
            Neighborhood = entity.Neighborhood,
            City = entity.City,
            State = entity.State,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            OpeningTime = entity.OpeningTime,
            ClosingTime = entity.ClosingTime,
            Phone = entity.Phone,
            CooperativeId = entity.CooperativeId,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}
