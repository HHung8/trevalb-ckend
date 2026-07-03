using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrevalApp.Models;
namespace TravelApp.Infrastructure.Data.Configurations;

public class TourBookingConfiguration : IEntityTypeConfiguration<TourBooking>
{
    public void Configure(EntityTypeBuilder<TourBooking> b)
    {
        b.ToTable("tour_bookings");
        b.HasKey(x => x.Id);

        b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        b.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");
        b.Property(x => x.BookingCode).IsRequired().HasMaxLength(20);
        b.Property(x => x.SpecialRequest).HasMaxLength(1000);

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.TourId);
        b.HasIndex(x => x.BookingCode).IsUnique();
        b.HasIndex(x => x.Status);

        b.HasOne(x => x.User).WithMany(x => x.TourBookings)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class HotelBookingConfiguration : IEntityTypeConfiguration<HotelBooking>
{
    public void Configure(EntityTypeBuilder<HotelBooking> b)
    {
        b.ToTable("hotel_bookings");
        b.HasKey(x => x.Id);

        b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        b.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");
        b.Property(x => x.BookingCode).IsRequired().HasMaxLength(20);
        b.Property(x => x.SpecialRequest).HasMaxLength(1000);

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.RoomId);
        b.HasIndex(x => x.BookingCode).IsUnique();
        b.HasIndex(x => x.Status);
        b.HasIndex(x => new { x.RoomId, x.CheckIn, x.CheckOut });

        b.HasOne(x => x.User).WithMany(x => x.HotelBookings)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("payments");
        b.HasKey(x => x.Id);

        b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        b.Property(x => x.BookingType).IsRequired().HasMaxLength(20);
        b.Property(x => x.Method).IsRequired().HasMaxLength(30);
        b.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("pending");
        b.Property(x => x.TransactionRef).HasMaxLength(200);
        b.Property(x => x.GatewayResponse).HasColumnType("jsonb");

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.BookingId);
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.TransactionRef);

        b.HasOne(x => x.User).WithMany(x => x.Payments)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.ToTable("reviews");
        b.HasKey(x => x.Id);

        b.Property(x => x.TargetType).IsRequired().HasMaxLength(20);
        b.Property(x => x.Comment).HasMaxLength(2000);
        b.Property(x => x.Rating).IsRequired();

        b.HasCheckConstraint("CK_reviews_rating", "\"Rating\" BETWEEN 1 AND 5");
        b.HasIndex(x => new { x.TargetType, x.TargetId });
        b.HasIndex(x => x.UserId);

        b.HasOne(x => x.User).WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Images).WithOne(x => x.Review)
            .HasForeignKey(x => x.ReviewId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> b)
    {
        b.ToTable("posts");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title).IsRequired().HasMaxLength(300);
        b.Property(x => x.Content).IsRequired();
        b.Property(x => x.ThumbnailUrl).HasMaxLength(500);
        b.Property(x => x.Tags).HasColumnType("jsonb");
        b.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("published");

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.DestinationId);
        b.HasIndex(x => x.IsPublished);
        b.HasIndex(x => x.CreatedAt);

        b.HasOne(x => x.User).WithMany(x => x.Posts)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Media).WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Comments).WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(x => x.Likes).WithOne(x => x.Post)
            .HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> b)
    {
        b.ToTable("post_likes");
        b.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();
    }
}

public class AttractionConfiguration : IEntityTypeConfiguration<Attraction>
{
    public void Configure(EntityTypeBuilder<Attraction> b)
    {
        b.ToTable("attractions");
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        b.Property(x => x.Category).IsRequired().HasMaxLength(50);
        b.Property(x => x.Description).HasMaxLength(2000);
        b.Property(x => x.ThumbnailUrl).HasMaxLength(500);
        b.Property(x => x.OpeningHours).HasMaxLength(200);
        b.Property(x => x.EntryFee).HasColumnType("decimal(18,2)");
        b.Property(x => x.Website).HasMaxLength(300);

        b.HasIndex(x => x.DestinationId);
        b.HasIndex(x => x.Category);
    }
}

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> b)
    {
        b.ToTable("wishlists");
        b.HasKey(x => x.Id);
        b.Property(x => x.ItemType).IsRequired().HasMaxLength(20);
        b.HasIndex(x => new { x.UserId, x.ItemType, x.ItemId }).IsUnique();

        b.HasOne(x => x.User).WithMany(x => x.Wishlists)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> b)
    {
        b.ToTable("notifications");
        b.HasKey(x => x.Id);

        b.Property(x => x.Type).IsRequired().HasMaxLength(50);
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.Message).IsRequired().HasMaxLength(1000);
        b.Property(x => x.ActionUrl).HasMaxLength(500);
        b.Property(x => x.Metadata).HasColumnType("jsonb");

        b.HasIndex(x => x.UserId);
        b.HasIndex(x => x.IsRead);
        b.HasIndex(x => x.CreatedAt);

        b.HasOne(x => x.User).WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}