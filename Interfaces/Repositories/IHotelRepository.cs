using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<IEnumerable<Hotel>> SearchAsync(Guid? destinationId, string? keyword, int? minStars, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
    Task<Hotel?> GetWithDetailsAsync(Guid id);
}