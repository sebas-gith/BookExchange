using BookExchange.Domain.Core;

namespace BookExchange.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int SenderId { get; set; }
        public Student Sender { get; set; } // Propiedad de navegación al remitente

        public int ReceiverId { get; set; }
        public Student Receiver { get; set; } // Propiedad de navegación al receptor

        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }

        // Opcional: Relacionar el mensaje con una oferta específica
        public int? ExchangeOfferId { get; set; } // Nullable si un mensaje no siempre se relaciona con una oferta
        public ExchangeOffer ExchangeOffer { get; set; }
    }
}
