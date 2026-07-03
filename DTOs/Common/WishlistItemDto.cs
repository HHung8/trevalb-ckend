namespace TrevalApp.DTOs.Common;

public record WishlistItemDto(
    Guid Id, 
    string ItemType, 
    Guid ItemId, 
    DateTime CreatedAt);