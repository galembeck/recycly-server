using Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class CollectConfiguration : IEntityTypeConfiguration<Collect>
{
    public void Configure(EntityTypeBuilder<Collect> builder)
    {
        builder.Property(c => c.WeightKg).HasPrecision(10, 3);
        builder.Property(c => c.Notes).HasMaxLength(500);

        builder.HasOne(c => c.CollectionPoint)
               .WithMany()
               .HasForeignKey(c => c.CollectionPointId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Material)
               .WithMany()
               .HasForeignKey(c => c.MaterialId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
