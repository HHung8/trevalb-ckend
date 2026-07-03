using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Destination;

namespace TrevalApp.Interfaces.Services;

public interface IDestinationService
{
    Task<PagedResultDto<DestinationDto>> GetAllAsync(DestinationQueryDto query);
    Task<DestinationDetailDto> GetByIdAsync(Guid id);
    Task<IEnumerable<DestinationDto>> GetFeaturedAsync(int count = 6);
    Task<DestinationDto> CreateAsync(CreateDestinationDto dto);
    Task<DestinationDto> UpdateAsync(Guid id, UpdateDestinationDto dto);
    Task DeleteAsync(Guid id);
}