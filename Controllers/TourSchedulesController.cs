using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Tour;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/tours/{tourId:guid}/schedules")]
public class TourSchedulesController : ControllerBase
{
    private readonly ITourScheduleService _scheduleService;
    public TourSchedulesController(ITourScheduleService scheduleService) => _scheduleService = scheduleService;

    [HttpGet]
    public async Task<IActionResult> GetByTour(Guid tourId)
    {
        var result = await _scheduleService.GetByTourAsync(tourId);
        return Ok(ApiResponse<IEnumerable<TourScheduleDto>>.Ok(result));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpPost]
    public async Task<IActionResult> Create(Guid tourId, [FromBody] CreateTourScheduleDto dto)
    {
        var result = await _scheduleService.CreateAsync(tourId, dto);
        return Ok(ApiResponse<TourScheduleDto>.Ok(result, "Create Calander successfully"));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid tourId, Guid id)
    {
        await _scheduleService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new{}, "Delete successfully"));
    }
}