using TravelApp.Infrastructure.Data.DTOs.Wishlish;
using TrevalApp.DTOs.Common;

namespace TrevalApp.Interfaces.Services;

public interface IWishlistService
{
    Task<ToggleWishlistResultDto> ToggleAsync(Guid userId, ToggleWishlistDto dto);
    Task<bool> IsWishlistedAsync(Guid userId, string itemType, Guid itemId);
    Task<PagedResultDto<WishlistTourDto>> GetToursAsync(Guid userId, WishlistQueryDto query);
    Task<PagedResultDto<WishlistHotelDto>> GetHotelsAsync(Guid userId, WishlistQueryDto query);
    Task RemoveAsync(Guid id, Guid userId);
}