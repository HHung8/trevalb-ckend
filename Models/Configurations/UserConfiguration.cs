using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrevalApp.Models;
namespace TravelApp.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.Property(x => x.FullName).IsRequired().HasMaxLength(200);
        b.Property(x => x.AvatarUrl).HasMaxLength(500);
        b.Property(x => x.Bio).HasMaxLength(500);
        b.Property(x => x.Address).HasMaxLength(300);
        b.Property(x => x.Role).HasMaxLength(20).HasDefaultValue("user");
        b.Property(x => x.RefreshToken).HasMaxLength(500);
    
        b.HasIndex(x => x.Role);
        b.HasIndex(x => x.IsActive);
    }
}