namespace API.Public.DTOs;

public class CreateCollectDTO
{
    public string CollectionPointId { get; set; } = string.Empty;
    public string MaterialId { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public DateTimeOffset CollectedAt { get; set; }
    public string? Notes { get; set; }
}
