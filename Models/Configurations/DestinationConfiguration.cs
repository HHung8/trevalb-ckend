using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TrevalApp.Models.Configurations;

public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> b)
    {
        b.ToTable("destinations");
        b.HasKey(x => x.Id);
 
        b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        b.Property(x => x.Country).IsRequired().HasMaxLength(100);
        b.Property(x => x.City).IsRequired().HasMaxLength(100);
        b.Property(x => x.Description).HasMaxLength(2000);
        b.Property(x => x.ThumbnailUrl).HasMaxLength(500);
        b.Property(x => x.Climate).HasMaxLength(200);
        b.Property(x => x.BestTimeToVisit).HasMaxLength(200);
 
        b.HasIndex(x => x.Country);
        b.HasIndex(x => x.IsFeatured);
        b.HasIndex(x => new { x.Country, x.City });
 
        b.HasMany(x => x.Tours).WithOne(x => x.Destination)
            .HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Hotels).WithOne(x => x.Destination)
            .HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Attractions).WithOne(x => x.Destination)
            .HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Images).WithOne(x => x.Destination)
            .HasForeignKey(x => x.DestinationId).OnDelete(DeleteBehavior.Cascade);
    }
}