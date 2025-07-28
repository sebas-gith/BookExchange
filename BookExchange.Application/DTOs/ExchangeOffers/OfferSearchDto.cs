using BookExchange.Domain.Entities; // Para BookCondition

namespace BookExchange.Application.DTOs.ExchangeOffers
{
    public class OfferSearchDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? SubjectId { get; set; }
        public BookCondition? Condition { get; set; } // Filtro por condición del libro
        public OfferType? Type { get; set; } // Filtro por tipo de oferta
        public OfferStatus? Status { get; set; } // Filtro por estado de la oferta
        public string Keywords { get; set; } // Para buscar en título, autor, descripción
    }
}