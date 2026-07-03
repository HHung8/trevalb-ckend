using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface ITourRepository : IRepository<Tour>
{
    Task<IEnumerable<Tour>> SearchAsync(Guid? destinationId, string? keyword, decimal? minPrice, decimal? maxPrice,
        string? difficulty, int page, int pageSize);
    Task<Tour?> GetWithDetailsAsync(Guid id);
    Task<IEnumerable<Tour>> GetByDestinationAsync(Guid destinationId);
}