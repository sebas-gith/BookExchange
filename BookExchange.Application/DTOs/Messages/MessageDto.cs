namespace BookExchange.Application.DTOs.Messages
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public string SenderName { get; set; }

        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }

        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }

        public int? ExchangeOfferId { get; set; }
        public string OfferTitle { get; set; } // Opcional: título del libro de la oferta
    }
}
