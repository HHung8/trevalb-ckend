using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IDestinationRepository : IRepository<Destination>
{
    Task<IEnumerable<Destination>> SearchAsync(string? keyword, string? country, int page, int pageSize);
    Task<Destination?> GetByIdAsync(int id);
}