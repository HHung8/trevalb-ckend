using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Tour;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/tours")]
public class ToursController : ControllerBase
{
    private readonly ITourService _tourService;
    public ToursController(ITourService tourService) => _tourService = tourService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TourQueryDto query)
    {
        var result = await _tourService.GetAllAsync(query);
        return Ok(ApiResponse<PagedResultDto<TourDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _tourService.GetByIdAsync(id);
        return Ok(ApiResponse<TourDetailDto>.Ok(result));
    }
    
    [HttpGet("by-destination/{destinationId:guid}")]
    public async Task<IActionResult> GetByDestination(Guid destinationId)
    {
        var result = await _tourService.GetByDestinationAsync(destinationId);
        return Ok(ApiResponse<IEnumerable<TourDto>>.Ok(result));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTourDto dto)
    {
        var result = await _tourService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id}, ApiResponse<TourDto>.Ok(result, "Created tour successfully"));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTourDto dto)
    {
       var result = await _tourService.UpdateAsync(id, dto);
       return Ok(ApiResponse<TourDto>.Ok(result, "Update tour successfully"));
    }

    [Authorize(Roles = "admin, partner")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tourService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new {}, "Deleted tour successfully"));
    }
    
}













