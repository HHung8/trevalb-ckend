using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Infrastructure.Data.DTOs.Attraction;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/attractions")]
public class AttractionsController : ControllerBase
{
    private readonly IAttractionService _attractionService;
    public AttractionsController(IAttractionService attractionService) => _attractionService = attractionService;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] AttractionQueryDto query)
    {
        var result = await _attractionService.SearchAsync(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _attractionService.GetByIdAsync(id);
        return Ok(ApiResponse<AttractionDto>.Ok(result));
    }

    [HttpGet("destinations/{destinationId:guid}")]
    public async Task<IActionResult> GetByDestination(Guid destinationId)
    {
        var result = await _attractionService.GetByDestinationAsync(destinationId);
        return Ok(ApiResponse<IEnumerable<AttractionSimpleDto>>.Ok(result));
    }
    
    [HttpGet("{id:guid}/schedules")]
    public async Task<IActionResult> GetSchedules(Guid id)
    {
        var schedules = await _attractionService.GetAttractionSchedulesAsync(id);
        return Ok(ApiResponse<IEnumerable<AttractionScheduleDto>>.Ok(schedules));
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttractionDto dto)
    {
        var result = await _attractionService.CreateAsync(dto);
        return Ok(ApiResponse<AttractionDto>.Ok(result, "Tạo địa điểm tham quan thành công."));
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAttractionDto dto)
    {
        var result = await _attractionService.UpdateAsync(id, dto);
        return Ok(ApiResponse<AttractionDto>.Ok(result, "Cập nhật địa điểm tham quan thành công."));
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _attractionService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "Xóa địa điểm tham quan thành công."));
    }
}