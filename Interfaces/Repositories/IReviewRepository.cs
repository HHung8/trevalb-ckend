using TrevalApp.Models;

namespace TrevalApp.Interfaces.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByTargetAsync(string targetType, Guid targetId, int page, int pageSize);
    Task<double> GetAverageRatingAsync(string targetType, Guid targetId);
}