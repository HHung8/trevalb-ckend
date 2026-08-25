using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/hotels/{hotelId:guid}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService) =>  _roomService = roomService;

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateRoom(Guid hotelId, [FromBody] CreateRoomDto dto)
    {
        var result = await _roomService.CreateRoomAsync(hotelId, dto);
        return Ok(ApiResponse<RoomDto>.Ok(result, "Create Room Successful"));
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateRoom(Guid hotelId, Guid id, [FromBody] UpdateRoomDto dto)
    {
        var result = await _roomService.UpdateRoomAsync(id, dto);
        return Ok(ApiResponse<RoomDto>.Ok(result, "Cập nhật phòng thành công."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteRoom(Guid hotelId, Guid id)
    {
        await _roomService.DeleteRoomAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa phòng thành công."));
    }
    
}