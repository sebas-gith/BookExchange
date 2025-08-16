using BookExchange.Domain.Entities; // Necesario para ExchangeOffer

namespace BookExchange.Domain.Interfaces
{
    public interface IExchangeOfferRepository : IBaseRepository<ExchangeOffer>
    {
        // Métodos específicos para ExchangeOffer
        Task<IEnumerable<ExchangeOffer>> GetActiveOffersForBookAsync(int bookId);
        Task<IEnumerable<ExchangeOffer>> GetOffersBySellerIdAsync(int sellerId);
        Task<IEnumerable<ExchangeOffer>> GetOffersFilteredAsync(
            decimal? minPrice,
            decimal? maxPrice,
            int? subjectId,
            BookCondition? condition,
            OfferType? type,
            OfferStatus? status,
            string keywords);

        Task<ExchangeOffer> GetOfferWithBookAndSellerDetailsAsync(int offerId);
        Task<IEnumerable<ExchangeOffer>> GetAllOffersWithDetailsAsync();
    }
}