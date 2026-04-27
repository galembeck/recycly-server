using API.Public.DTOs._Base;
using Domain.Data.Entities;

namespace API.Public.DTOs;

public class CollectResponseDTO : PublicBaseDTO<Collect>
{
    public string CollectionPointId { get; set; } = string.Empty;
    public string? CollectionPointName { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string MaterialId { get; set; } = string.Empty;
    public MaterialDTO? Material { get; set; }
    public decimal WeightKg { get; set; }
    public DateTimeOffset CollectedAt { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public CollectResponseDTO() { }

    public CollectResponseDTO(Collect o) : base(o)
    {
        if (o == null) return;

        CollectionPointId = o.CollectionPointId;
        CollectionPointName = o.CollectionPoint?.Name;
        UserId = o.UserId;
        MaterialId = o.MaterialId;
        Material = o.Material != null ? new MaterialDTO(o.Material) : null;
        WeightKg = o.WeightKg;
        CollectedAt = o.CollectedAt;
        Notes = o.Notes;
        CreatedAt = o.CreatedAt;
    }

    public static CollectResponseDTO? ModelToDTO(Collect o) => o == null ? null : new CollectResponseDTO(o);

    public static List<CollectResponseDTO> ModelToDTO(IEnumerable<Collect> list) =>
        list.Select(c => new CollectResponseDTO(c)).ToList();
}
