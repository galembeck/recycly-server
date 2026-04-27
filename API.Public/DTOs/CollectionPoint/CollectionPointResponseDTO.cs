using API.Public.DTOs._Base;
using Domain.Data.Entities;

namespace API.Public.DTOs;

public class CollectionPointResponseDTO : PublicBaseDTO<CollectionPoint>
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
    public string OpeningTime { get; set; } = string.Empty;
    public string ClosingTime { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsOpen { get; set; }
    public string CooperativeId { get; set; } = string.Empty;
    public List<MaterialDTO> Materials { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }

    public CollectionPointResponseDTO() { }

    public CollectionPointResponseDTO(CollectionPoint o) : base(o)
    {
        if (o == null) return;

        Name = o.Name;
        Description = o.Description;
        ZipCode = o.ZipCode;
        Address = o.Address;
        Number = o.Number;
        Complement = o.Complement;
        Neighborhood = o.Neighborhood;
        City = o.City;
        State = o.State;
        Latitude = o.Latitude;
        Longitude = o.Longitude;
        OpeningTime = o.OpeningTime.ToString("HH:mm");
        ClosingTime = o.ClosingTime.ToString("HH:mm");
        Phone = o.Phone;
        IsOpen = o.IsOpen;
        CooperativeId = o.CooperativeId;
        Materials = o.Materials?.Select(m => new MaterialDTO(m)).ToList() ?? [];
        CreatedAt = o.CreatedAt;
    }

    public static CollectionPointResponseDTO? ModelToDTO(CollectionPoint o) =>
        o == null ? null : new CollectionPointResponseDTO(o);

    public static List<CollectionPointResponseDTO> ModelToDTO(IEnumerable<CollectionPoint> list) =>
        list.Select(cp => new CollectionPointResponseDTO(cp)).ToList();
}
