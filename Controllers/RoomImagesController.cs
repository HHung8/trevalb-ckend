using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/rooms/{roomId}/images")]
public class RoomImagesController : ControllerBase
{
    private readonly IRoomImageService _roomImageService;
    public RoomImagesController(IRoomImageService roomImageService)
    {
        _roomImageService = roomImageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetImages(Guid roomId)
    {
        var images = await _roomImageService.GetRoomImagesAsync(roomId);
        return Ok(new { success = true, message=(string?)null, data = images, errors = (object?)null });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateImage(Guid roomId, [FromBody] CreateRoomImageDto dto)
    {
        var image = await _roomImageService.CreateRoomImageAsync(roomId, dto);
        return Ok(new { success = true, message = "Thêm ảnh  thành công" ,data = image, errors = (object?)null });
    }

    [HttpDelete("{imageId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _roomImageService.DeleteRoomImageAsync(imageId);
        return Ok(new {success = true, message = "Xoá ảnh phòng thành công", data = (object?)null, errors = (object?)null });
    }

    [HttpPut("{imageId:guid}/order")]
    [Authorize]
    public async Task<IActionResult> UpdateOrder(Guid roomId, Guid imageId, [FromBody] UpdateRoomImageOrderDto dto)
    {
        var images = await _roomImageService.UpdateRoomImageOrderAsync(imageId, dto.DisplayOrder);
        return Ok(new {success = true, message = "Cập nhật thứ tự thành công.", data = images, errors = (object?)null });
    }
}