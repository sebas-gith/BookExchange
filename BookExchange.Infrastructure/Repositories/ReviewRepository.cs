using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookExchange.Infrastructure.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(BookExchangeContext context) : base(context)
        {
        }

        // Implementación de métodos específicos para Review
        public async Task<IEnumerable<Review>> GetReviewsByReviewedUserAsync(int reviewedUserId)
        {
            return await _dbSet.Where(r => r.ReviewedUserId == reviewedUserId).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsGivenByReviewerAsync(int reviewerId)
        {
            return await _dbSet.Where(r => r.ReviewerId == reviewerId).ToListAsync();
        }

        public async Task<double> GetAverageRatingForUserAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.ReviewedUserId == userId)
                .Select(r => (double)r.Rating)
                .AverageAsync();
        }
    }
}
