using BookExchange.Application.DTOs.Students; // Para StudentDto
using BookExchange.Application.DTOs.ExchangeOffers; // Para ExchangeOfferDto

namespace BookExchange.Application.DTOs.Messages
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public StudentDto Sender { get; set; } // DTO del remitente
        public int ReceiverId { get; set; }
        public StudentDto Receiver { get; set; } // DTO del receptor
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public int? ExchangeOfferId { get; set; }
        public ExchangeOfferDto ExchangeOffer { get; set; } // DTO de la oferta relacionada (opcional)
    }
}