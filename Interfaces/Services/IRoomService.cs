using TrevalApp.DTOs.Hotel;
using CreateRoomDto = TrevalApp.DTOs.Hotel.CreateRoomDto;
namespace TrevalApp.Interfaces.Services;

public interface IRoomService
{
    Task<RoomDto> CreateRoomAsync(Guid hotelId, CreateRoomDto dto);
    Task<RoomDto> UpdateRoomAsync(Guid id, UpdateRoomDto dto);
    Task DeleteRoomAsync(Guid id);
}