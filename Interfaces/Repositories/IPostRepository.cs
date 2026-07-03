using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<IEnumerable<Post>> GetFeedAsync (int page, int pageSize);
    Task<IEnumerable<Post>> GetByUserAsync(Guid userId, int page, int pageSize);
    Task<Post?> GetWithDetailsAsync(Guid id);
}

