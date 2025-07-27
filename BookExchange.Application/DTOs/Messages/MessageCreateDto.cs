namespace BookExchange.Application.DTOs.Messages
{
    public class MessageCreateDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public string Content { get; set; }

        public int? ExchangeOfferId { get; set; } // Si aplica
    }
}
