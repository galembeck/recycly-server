using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class CollectionPointConfiguration : IEntityTypeConfiguration<CollectionPoint>
{
    public void Configure(EntityTypeBuilder<CollectionPoint> builder)
    {
        builder.Property(cp => cp.Name).HasMaxLength(150).IsRequired();
        builder.Property(cp => cp.Description).HasMaxLength(500);
        builder.Property(cp => cp.ZipCode).HasMaxLength(10).IsRequired();
        builder.Property(cp => cp.Address).HasMaxLength(250).IsRequired();
        builder.Property(cp => cp.Number).HasMaxLength(20).IsRequired();
        builder.Property(cp => cp.Complement).HasMaxLength(100);
        builder.Property(cp => cp.Neighborhood).HasMaxLength(150).IsRequired();
        builder.Property(cp => cp.City).HasMaxLength(150).IsRequired();
        builder.Property(cp => cp.State).HasMaxLength(2).IsRequired();
        builder.Property(cp => cp.Latitude).HasMaxLength(30);
        builder.Property(cp => cp.Longitude).HasMaxLength(30);
        builder.Property(cp => cp.Phone).HasMaxLength(20).IsRequired();

        builder.Ignore(cp => cp.IsOpen);

        builder.HasMany(cp => cp.Materials)
               .WithMany(m => m.CollectionPoints)
               .UsingEntity(j =>
               {
                   j.ToTable("TBCollectionPointMaterial");
                   j.Property<string>("CollectionPointsId").HasColumnName("CollectionPointId");
                   j.Property<string>("MaterialsId").HasColumnName("MaterialId");
               });

        builder.HasOne(cp => cp.Cooperative)
               .WithMany()
               .HasForeignKey(cp => cp.CooperativeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
