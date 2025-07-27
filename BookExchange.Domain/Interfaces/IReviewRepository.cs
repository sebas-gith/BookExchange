using BookExchange.Domain.Entities; // Necesario para Review
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Domain.Interfaces
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        // Métodos específicos para Review
        Task<IEnumerable<Review>> GetReviewsByReviewedUserAsync(int reviewedUserId);
        Task<IEnumerable<Review>> GetReviewsGivenByReviewerAsync(int reviewerId);
        Task<double> GetAverageRatingForUserAsync(int userId);
    }
}
