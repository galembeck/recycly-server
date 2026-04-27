namespace API.Public.DTOs;

public class CreateSaleDTO
{
    public string BuyerName { get; set; } = string.Empty;
    public List<string> MaterialIds { get; set; } = [];
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset SoldAt { get; set; }
    public string? Notes { get; set; }
}
