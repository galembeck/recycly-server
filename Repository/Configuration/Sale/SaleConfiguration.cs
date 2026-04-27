using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.Property(s => s.BuyerName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Notes).HasMaxLength(500);
        builder.Property(s => s.WeightKg).HasPrecision(10, 3);
        builder.Property(s => s.Price).HasPrecision(10, 2);

        builder.HasMany(s => s.Materials)
               .WithMany(m => m.Sales)
               .UsingEntity(j =>
               {
                   j.ToTable("TBSaleMaterial");
                   j.Property<string>("SalesId").HasColumnName("SaleId");
                   j.Property<string>("MaterialsId").HasColumnName("MaterialId");
               });

        builder.HasOne(s => s.Cooperative)
               .WithMany()
               .HasForeignKey(s => s.CooperativeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
