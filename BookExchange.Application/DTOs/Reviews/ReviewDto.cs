using BookExchange.Application.DTOs.Students; // Para StudentDto
using BookExchange.Application.DTOs.ExchangeOffers; // Para ExchangeOfferDto

namespace BookExchange.Application.DTOs.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        public StudentDto Reviewer { get; set; } // DTO del estudiante que hizo la reseña
        public int ReviewedUserId { get; set; }
        public StudentDto ReviewedUser { get; set; } // DTO del estudiante reseñado
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int? ExchangeOfferId { get; set; }
        public ExchangeOfferDto ExchangeOffer { get; set; } // DTO de la oferta relacionada (opcional)
    }
}