using Domain.Enumerators;

namespace API.Public.DTOs;

public class UpdateProductDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProductCategory? Category { get; set; }
    public decimal? Price { get; set; }
    public List<string>? Ingredients { get; set; }
    public int? StockAmount { get; set; }
    public float? Weight { get; set; }
    public float? Height { get; set; }
    public float? Width { get; set; }
    public float? Length { get; set; }
}
