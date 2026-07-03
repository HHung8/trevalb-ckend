using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrevalApp.DTOs.APIRESPONSE;
using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Destination;
using TrevalApp.Interfaces.Services;

namespace TravelApp.Infrastructure.Data.Controllers;

[ApiController]
[Route("api/destinations")]
public class DestinationsController : ControllerBase
{
    private readonly IDestinationService _destinationService;
    public DestinationsController(IDestinationService destinationService) => _destinationService = destinationService;
    
    // Lấy danh sách destinations
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DestinationQueryDto query)
    {
        var result = await _destinationService.GetAllAsync(query);
        return Ok(ApiResponse<PagedResultDto<DestinationDto>>.Ok(result));
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 6)
    {
        var result = await _destinationService.GetFeaturedAsync(count);
        return Ok(ApiResponse<IEnumerable<DestinationDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _destinationService.GetByIdAsync(id);
        return Ok(ApiResponse<DestinationDetailDto>.Ok(result));
    }
    
    // create destination only admin
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDestinationDto dto)
    {
        var result = await _destinationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<DestinationDto>.Ok(result, "Crated Destination Success"));
    }
    
    // update destination only admin
    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDestinationDto dto)
    {
        var result = await _destinationService.UpdateAsync(id, dto);
        return Ok(ApiResponse<DestinationDto>.Ok(result, "Updated Destination Success"));
    }
    
    // delete destination only admin
    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _destinationService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new {}, "Deleted Destination Success"));
    }
}