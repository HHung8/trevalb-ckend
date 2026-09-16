using TravelApp.Infrastructure.Data.DTOs.Attraction;
using TrevalApp.DTOs.Common;

namespace TrevalApp.Interfaces.Services;

public interface IAttractionService
{
    Task<PagedResultDto<AttractionDto>> SearchAsync(AttractionQueryDto query);
    Task<AttractionDto> GetByIdAsync(Guid id);
    Task<IEnumerable<AttractionSimpleDto>> GetByDestinationAsync(Guid destinationId);
    Task<AttractionDto> CreateAsync(CreateAttractionDto dto);
    Task<AttractionDto> UpdateAsync(Guid id, UpdateAttractionDto dto);
    // IAttractionService
    Task<IEnumerable<AttractionScheduleDto>> GetAttractionSchedulesAsync(Guid attractionId);
    Task<AttractionScheduleDto> CreateAttractionScheduleAsync(CreateAttractionScheduleDto dto);
    Task DeleteAttractionScheduleAsync(Guid id);
    Task DeleteAsync(Guid id);
}