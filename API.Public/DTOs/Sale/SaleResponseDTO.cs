using API.Public.DTOs._Base;
using Domain.Data.Entities;

namespace API.Public.DTOs;

public class SaleResponseDTO : PublicBaseDTO<Sale>
{
    public string BuyerName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset SoldAt { get; set; }
    public string? Notes { get; set; }
    public string CooperativeId { get; set; } = string.Empty;
    public List<MaterialDTO> Materials { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }

    public SaleResponseDTO() { }

    public SaleResponseDTO(Sale o) : base(o)
    {
        if (o == null) return;

        BuyerName = o.BuyerName;
        WeightKg = o.WeightKg;
        Price = o.Price;
        SoldAt = o.SoldAt;
        Notes = o.Notes;
        CooperativeId = o.CooperativeId;
        Materials = o.Materials?.Select(m => new MaterialDTO(m)).ToList() ?? [];
        CreatedAt = o.CreatedAt;
    }

    public static SaleResponseDTO? ModelToDTO(Sale o) =>
        o == null ? null : new SaleResponseDTO(o);

    public static List<SaleResponseDTO> ModelToDTO(IEnumerable<Sale> list) =>
        list.Select(s => new SaleResponseDTO(s)).ToList();
}
