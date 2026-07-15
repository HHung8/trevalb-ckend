using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Hotel;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/hotels")]
public class Hotelscontroller : ControllerBase
{
    private readonly IHotelService _hotelService;
    public Hotelscontroller(IHotelService hotelService) => _hotelService = hotelService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] HotelQueryDto query)
    {
        var result = await _hotelService.GetAllAsync(query);
        return Ok(ApiResponse<PagedResultDto<HotelDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _hotelService.GetByIdAsync(id);
        return Ok(ApiResponse<HotelDetailDto>.Ok(result));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHotelDto dto)
    {
        var result = await _hotelService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<HotelDto>.Ok(result, "Tạo khách sạn thành công."));
    }
    
    [Authorize(Roles = "admin, partner")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotelDto dto)
    {
        var result = await _hotelService.UpdateAsync(id, dto);
        return Ok(ApiResponse<HotelDto>.Ok(result, "Cập nhật khách sạn thành công."));
    }
    
    [Authorize(Roles = "admin,partner")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _hotelService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa khách sạn thành công."));
    }
}