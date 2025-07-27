namespace BookExchange.Application.DTOs.ExchangeOffers
{
    public class ExchangeOfferCreateDto
    {
        public int BookId { get; set; }
        public OfferType Type { get; set; } // Enum del dominio (puedes usar int o string si prefieres)
        public decimal Price { get; set; }
        public string DesiredBooksForExchange { get; set; }
        public string Location { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public enum OfferType
    {
        Sale,
        Exchange,
        Both
    }
}
