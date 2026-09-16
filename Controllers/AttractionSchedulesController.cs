using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.DTOs.Attraction;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/attraction-schedules")]
public class AttractionSchedulesController : ControllerBase
{
    private readonly IAttractionService _attractionService;
    public AttractionSchedulesController(IAttractionService attractionService) => _attractionService = attractionService;

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateAttractionScheduleDto dto)
    {
        var result = await _attractionService.CreateAttractionScheduleAsync(dto);
        return Ok(ApiResponse<AttractionScheduleDto>.Ok(result, "Tạo lịch thăm quan thành công"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _attractionService.DeleteAttractionScheduleAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa lịch tham quan thành công."));
    }
    
}