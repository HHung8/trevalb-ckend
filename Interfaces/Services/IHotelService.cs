using TrevalApp.DTOs.Common;
using TrevalApp.DTOs.Hotel;

namespace TrevalApp.Interfaces.Services;

public interface IHotelService
{
    Task<PagedResultDto<HotelDto>> GetAllAsync(HotelQueryDto query);
    Task<HotelDetailDto> GetByIdAsync(Guid id);
    Task<HotelDto> CreateAsync(CreateHotelDto dto);
    Task<HotelDto> UpdateAsync(Guid id, UpdateHotelDto dto);
    Task DeleteAsync(Guid id);
}