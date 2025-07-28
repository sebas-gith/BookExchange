using BookExchange.Application.DTOs.Reviews; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Contracts
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(ReviewCreateDto createDto);
        Task<ReviewDto> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsByReviewedUserAsync(int reviewedUserId);
        Task<IEnumerable<ReviewDto>> GetReviewsGivenByReviewerAsync(int reviewerId);
        Task<double> GetAverageRatingForUserAsync(int userId);
        Task UpdateReviewAsync(ReviewUpdateDto updateDto); 
        Task DeleteReviewAsync(int reviewId); 
    }
}