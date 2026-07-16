using TrevalApp.DTOs.Tour;
using TrevalApp.Models;

namespace TrevalApp.Interfaces.Services;

public interface ITourScheduleService
{
    Task<IEnumerable<TourScheduleDto>> GetByTourAsync(Guid tourId);
    Task<TourScheduleDto> CreateAsync(Guid tourId, CreateTourScheduleDto dto);
    Task DeleteAsync(Guid id);
}