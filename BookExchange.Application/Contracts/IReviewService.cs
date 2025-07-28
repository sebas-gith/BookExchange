using BookExchange.Application.DTOs.Reviews; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(ReviewCreateDto createDto, int reviewerId);
        Task<ReviewDto> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsByReviewedUserAsync(int reviewedUserId);
        Task<IEnumerable<ReviewDto>> GetReviewsGivenByReviewerAsync(int reviewerId);
        Task<double> GetAverageRatingForUserAsync(int userId);
        Task UpdateReviewAsync(ReviewUpdateDto updateDto); 
        Task DeleteReviewAsync(int reviewId); 
    }
}