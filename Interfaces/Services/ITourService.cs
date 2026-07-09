using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Tour;

namespace TrevalApp.Interfaces.Services;

public interface ITourService
{
    Task<PagedResultDto<TourDto>> GetAllAsync(TourQueryDto query);
    Task<TourDetailDto> GetByIdAsync(Guid id);
    Task<IEnumerable<TourDto>> GetByDestinationAsync(Guid destinationId);
    Task<TourDto> CreateAsync(CreateTourDto dto);
    Task<TourDto> UpdateAsync(Guid id, UpdateTourDto dto);
    Task DeleteAsync(Guid id);
} 