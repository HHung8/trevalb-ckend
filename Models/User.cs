using Microsoft.AspNetCore.Identity;

namespace TrevalApp.Models;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Role { get; set; } = "user"; // user | admin | partner    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
 
    // Navigation   
    public ICollection<TourBooking> TourBookings { get; set; } = new List<TourBooking>();
    public ICollection<HotelBooking> HotelBookings { get; set; } = new List<HotelBooking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}