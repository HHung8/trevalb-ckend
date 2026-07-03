using TrevalApp.DTOs.Common;

namespace TrevalApp.Interfaces.Services;

public interface IWishlistService
{
    Task<IEnumerable<WishlistItemDto>> GetByUserAsync(Guid userId);
    Task AddAsync(Guid userId, string itemType, Guid itemId);
    Task RemoveAsync(Guid userId, string itemType, Guid itemId);
    Task<bool> CheckAsync(Guid userId, string itemType, Guid itemId);
}