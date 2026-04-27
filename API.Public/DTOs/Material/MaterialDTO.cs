using API.Public.DTOs._Base;
using Domain.Data.Entities;

namespace API.Public.DTOs;

public class MaterialDTO : PublicBaseDTO<Material>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public MaterialDTO() { }

    public MaterialDTO(Material o) : base(o)
    {
        if (o == null) return;

        Name = o.Name;
        Description = o.Description;
        Color = o.Color;
    }

    public static MaterialDTO? ModelToDTO(Material o) => o == null ? null : new MaterialDTO(o);

    public static List<MaterialDTO> ModelToDTO(IEnumerable<Material> list) =>
        list.Select(m => new MaterialDTO(m)).ToList();

    public static Material? DTOToModel(MaterialDTO o)
    {
        if (o == null) return null;

        var model = new Material
        {
            Name = o.Name,
            Description = o.Description,
            Color = o.Color,
        };

        return o.InitializeInstance(model);
    }
}
