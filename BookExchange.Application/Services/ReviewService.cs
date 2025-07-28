using AutoMapper;
using BookExchange.Application.DTOs.Reviews;
using BookExchange.Application.Exceptions;
using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IStudentRepository _studentRepository; // Para validar reviewer y reviewedUser
        private readonly IExchangeOfferRepository _offerRepository; // Para validar la oferta (si aplica)
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IStudentRepository studentRepository, IExchangeOfferRepository offerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _studentRepository = studentRepository;
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> CreateReviewAsync(ReviewCreateDto createDto)
        {
            // Validar que el reviewer existe
            var reviewer = await _studentRepository.GetByIdAsync(createDto.ReviewerId);
            if (reviewer == null) throw new Exceptions.ApplicationException($"El revisor (StudentId) con ID {createDto.ReviewerId} no existe.");

            // Validar que el usuario a reseñar existe
            var reviewedUser = await _studentRepository.GetByIdAsync(createDto.ReviewedUserId);
            if (reviewedUser == null) throw new Exceptions.ApplicationException($"El usuario a reseñar (ReviewedUserId) con ID {createDto.ReviewedUserId} no existe.");

            // Evitar que un usuario se reseñe a sí mismo
            if (createDto.ReviewerId == createDto.ReviewedUserId)
            {
                throw new ValidationException("No puedes reseñarte a ti mismo.");
            }

            // Validar si la oferta existe si se proporciona
            if (createDto.ExchangeOfferId.HasValue)
            {
                var offerExists = await _offerRepository.GetByIdAsync(createDto.ExchangeOfferId.Value);
                if (offerExists == null)
                {
                    throw new ValidationException($"La oferta de intercambio con ID {createDto.ExchangeOfferId.Value} no existe.");
                }
                // Opcional: Lógica para asegurar que la reseña es de una transacción real (ej. offer.Status == Sold/Exchanged)
            }

            // Opcional: Prevenir múltiples reviews del mismo reviewer al mismo reviewedUser para la misma oferta
            var existingReview = await _reviewRepository.FindAsync(r =>
                r.ReviewerId == createDto.ReviewerId &&
                r.ReviewedUserId == createDto.ReviewedUserId &&
                r.ExchangeOfferId == createDto.ExchangeOfferId);

            if (existingReview.Any())
            {
                throw new Exceptions.ApplicationException("Ya existe una reseña para esta transacción por este usuario.");
            }


            var review = _mapper.Map<Review>(createDto);
            review.ReviewerId = createDto.ReviewerId;
            review.ReviewDate = DateTime.UtcNow;

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByReviewedUserAsync(int reviewedUserId)
        {
            var reviews = await _reviewRepository.GetReviewsByReviewedUserAsync(reviewedUserId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsGivenByReviewerAsync(int reviewerId)
        {
            var reviews = await _reviewRepository.GetReviewsGivenByReviewerAsync(reviewerId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<double> GetAverageRatingForUserAsync(int userId)
        {
            // Verificar si el usuario existe antes de calcular el promedio
            var userExists = await _studentRepository.GetByIdAsync(userId);
            if (userExists == null)
            {
                throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
            }

            return await _reviewRepository.GetAverageRatingForUserAsync(userId);
        }

        public async Task UpdateReviewAsync(ReviewUpdateDto updateDto)
        {
            var reviewToUpdate = await _reviewRepository.GetByIdAsync(updateDto.Id);
            if (reviewToUpdate == null)
            {
                throw new KeyNotFoundException($"Reseña con ID {updateDto.Id} no encontrada.");
            }
            // Aquí podrías añadir lógica de autorización (solo el reviewer puede actualizar su reseña)

            _mapper.Map(updateDto, reviewToUpdate);
            _reviewRepository.Update(reviewToUpdate);
            await _reviewRepository.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var reviewToDelete = await _reviewRepository.GetByIdAsync(reviewId);
            if (reviewToDelete == null)
            {
                throw new KeyNotFoundException($"Reseña con ID {reviewId} no encontrada.");
            }
            // Aquí podrías añadir lógica de autorización

            _reviewRepository.Remove(reviewToDelete);
            await _reviewRepository.SaveChangesAsync();
        }
    }
}