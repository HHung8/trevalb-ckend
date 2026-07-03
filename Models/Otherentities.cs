namespace TrevalApp.Models;

public class Payment : BaseEntity
{
    public Guid UserId { get; set; }
    public string BookingType { get; set; } = string.Empty; // tour | hotel
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty; // vnpay | momo | stripe | cash
    public string Status { get; set; } = "pending"; // pending | success | failed | refunded
    public string? TransactionRef { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime? PaidAt { get; set; }
 
    // Navigation
    public User User { get; set; } = null!;
}
 
public class Review : BaseEntity
{
    public Guid UserId { get; set; }
    public string TargetType { get; set; } = string.Empty; // tour | hotel | attraction
    public Guid TargetId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
    public bool IsVerified { get; set; } = false;
 
    // Navigation
    public User User { get; set; } = null!;
    public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();
}
 
public class Post : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? DestinationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Tags { get; set; } // JSON array
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    public string Status { get; set; } = "published"; // draft | published | archived
 
    // Navigation
    public User User { get; set; } = null!;
    public Destination? Destination { get; set; }
    public ICollection<PostMedia> Media { get; set; } = new List<PostMedia>();
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
}
 
public class PostMedia : BaseEntity
{
    public Guid PostId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = "image"; // image | video
    public int DisplayOrder { get; set; } = 0;
 
    // Navigation
    public Post Post { get; set; } = null!;
}
 
public class PostComment : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public string Content { get; set; } = string.Empty;
 
    // Navigation
    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}
 
public class PostLike : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
 
    // Navigation
    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}
 
public class Attraction : BaseEntity
{
    public Guid DestinationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // museum | park | beach | temple | etc
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? OpeningHours { get; set; }
    public decimal? EntryFee { get; set; }
    public string? Website { get; set; }
 
    // Navigation
    public Destination Destination { get; set; } = null!;
}
 
public class Wishlist : BaseEntity
{
    public Guid UserId { get; set; }
    public string ItemType { get; set; } = string.Empty; // tour | hotel | attraction | destination
    public Guid ItemId { get; set; }
 
    // Navigation
    public User User { get; set; } = null!;
}
 
public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty; // booking_confirmed | payment_success | review_reply
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public string? ActionUrl { get; set; }
    public string? Metadata { get; set; } // JSON
 
    // Navigation
    public User User { get; set; } = null!;
}
 
// Image entities
public class DestinationImage : BaseEntity
{
    public Guid DestinationId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public Destination Destination { get; set; } = null!;
}
 
public class TourImage : BaseEntity
{
    public Guid TourId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public Tour Tour { get; set; } = null!;
}
 
public class HotelImage : BaseEntity
{
    public Guid HotelId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public Hotel Hotel { get; set; } = null!;
}
 
public class RoomImage : BaseEntity
{
    public Guid RoomId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public Room Room { get; set; } = null!;
}
 
public class ReviewImage : BaseEntity
{
    public Guid ReviewId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public Review Review { get; set; } = null!;
}
 