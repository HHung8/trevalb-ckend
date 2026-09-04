using TrevalApp.DTOs.Hotel;

namespace TrevalApp.Interfaces.Services;

public interface IRoomImageService
{
    Task<RoomImageDto> CreateRoomImageAsync(Guid roomId, CreateRoomImageDto dto);
    Task<List<RoomImageDto>> GetRoomImagesAsync(Guid roomId);
    Task DeleteRoomImageAsync(Guid imageId);
    Task<RoomImageDto> UpdateRoomImageOrderAsync(Guid imageId, int displayOrder);
}