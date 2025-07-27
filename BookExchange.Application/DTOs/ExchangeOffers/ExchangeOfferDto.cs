namespace BookExchange.Application.DTOs.ExchangeOffers
{
    public class ExchangeOfferDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public string BookTitle { get; set; }           // Información útil desde Book
        public string SellerFullName { get; set; }      // Información útil desde Student

        public string OfferType { get; set; }           // Sale, Exchange, Both
        public decimal Price { get; set; }
        public string DesiredBooksForExchange { get; set; }
        public string Location { get; set; }

        public DateTime PostedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string OfferStatus { get; set; }         // Active, Sold, etc.
    }
}
