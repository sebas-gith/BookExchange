using BookExchange.Domain.Entities; // Para OfferType, OfferStatus
using BookExchange.Application.DTOs.Books; // Para BookDto
using BookExchange.Application.DTOs.Students; // Para StudentDto

namespace BookExchange.Application.DTOs.ExchangeOffers
{
    public class ExchangeOfferDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookDto Book { get; set; } // DTO del libro asociado
        public int SellerId { get; set; }
        public StudentDto Seller { get; set; } // DTO del estudiante vendedor
        public OfferType Type { get; set; }
        public decimal Price { get; set; }
        public string DesiredBooksForExchange { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public OfferStatus Status { get; set; }
    }
}