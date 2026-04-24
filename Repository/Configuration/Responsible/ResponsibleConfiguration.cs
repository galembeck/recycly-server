using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Repository.Configuration.Responsible;

public class ResponsibleConfiguration : IEntityTypeConfiguration<Domain.Data.Entities.Responsible>
{
    public void Configure(EntityTypeBuilder<Domain.Data.Entities.Responsible> builder)
    {
        builder.ToTable("TBResponsible");

        builder.HasIndex(r => r.Email).IsUnique();
        builder.HasIndex(r => r.Cpf).IsUnique();

        var listStringConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
        );

        var listStringComparer = new ValueComparer<List<string>>(
            (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
            c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c == null ? new List<string>() : c.ToList()
        );

        builder.Property(r => r.Phones)
            .HasConversion(listStringConverter)
            .Metadata.SetValueComparer(listStringComparer);
    }
}
