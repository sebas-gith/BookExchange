using BookExchange.Application.DTOs.Reviews;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net; // Para HttpStatusCode
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // POST: api/Reviews
        [HttpPost]
        [ProducesResponseType(typeof(ReviewDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // El ReviewerId viene del DTO
                var review = await _reviewService.CreateReviewAsync(createDto);
                return CreatedAtAction(nameof(CreateReview), review); // O un endpoint de obtención de reviews
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // GET: api/Reviews/by-reviewed-user/{userId}
        [HttpGet("by-reviewed-user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByReviewedUser(int userId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByReviewedUserAsync(userId);
                return Ok(reviews);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Usuario con ID {userId} no encontrado para reseñas.");
            }
        }

        // GET: api/Reviews/by-reviewer/{reviewerId}
        [HttpGet("by-reviewer/{reviewerId}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsGivenByReviewer(int reviewerId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsGivenByReviewerAsync(reviewerId);
                return Ok(reviews);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Revisor con ID {reviewerId} no encontrado.");
            }
        }

        // GET: api/Reviews/average-rating/{userId}
        [HttpGet("average-rating/{userId}")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<double>> GetAverageRatingForUser(int userId)
        {
            try
            {
                var averageRating = await _reviewService.GetAverageRatingForUserAsync(userId);
                return Ok(averageRating);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Usuario con ID {userId} no encontrado.");
            }
        }


        // PUT: api/Reviews/5
        // Si se permite la actualización de reseñas (y cómo se autoriza)
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la reseña en el cuerpo.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Si el servicio de actualización de reseñas necesita el reviewerId para verificar,
                // deberías modificar IReviewService.UpdateReviewAsync(ReviewUpdateDto updateDto, int reviewerId)
                // y pasarlo aquí (desde un header o query param si no hay JWT).
                await _reviewService.UpdateReviewAsync(updateDto);
                return Ok(new { message = "Reseña actualizada exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                // Si el servicio de eliminación de reseñas necesita el reviewerId para verificar,
                // deberías modificar IReviewService.DeleteReviewAsync(int reviewId, int reviewerId)
                // y pasarlo aquí.
                await _reviewService.DeleteReviewAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}