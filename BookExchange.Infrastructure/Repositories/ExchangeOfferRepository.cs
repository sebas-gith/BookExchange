using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookExchange.Infrastructure.Repositories
{
    public class ExchangeOfferRepository : BaseRepository<ExchangeOffer>, IExchangeOfferRepository
    {
        public ExchangeOfferRepository(BookExchangeContext context) : base(context)
        {
        }

        // Implementación de métodos específicos para ExchangeOffer
        public async Task<IEnumerable<ExchangeOffer>> GetActiveOffersForBookAsync(int bookId)
        {
            return await _dbSet.Where(eo => eo.BookId == bookId && eo.Status == OfferStatus.Active).ToListAsync();
        }

        public async Task<IEnumerable<ExchangeOffer>> GetOffersBySellerIdAsync(int sellerId)
        {
            return await _dbSet.Where(eo => eo.SellerId == sellerId).ToListAsync();
        }

        public async Task<IEnumerable<ExchangeOffer>> GetOffersFilteredAsync(
            decimal? minPrice,
            decimal? maxPrice,
            int? subjectId,
            BookCondition? condition,
            OfferType? type,
            OfferStatus? status,
            string keywords)
        {
            var query = _context.ExchangeOffers
                                .Include(o => o.Book)
                                .ThenInclude(b => b.Subject)
                                .Include(o => o.Seller)
                                .AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(o => o.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(o => o.Price <= maxPrice.Value);

            if (subjectId.HasValue)
                query = query.Where(o => o.Book.SubjectId == subjectId.Value);

            if (condition.HasValue)
                query = query.Where(o => o.Book.Condition == condition.Value);

            if (type.HasValue)
                query = query.Where(o => o.Type == type.Value);

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (!string.IsNullOrEmpty(keywords))
            {
                var searchKeywords = keywords.ToLower();
                query = query.Where(o =>
                    o.Book.Title.ToLower().Contains(searchKeywords) ||
                    o.Book.Author.ToLower().Contains(searchKeywords));
            }

            return await query.ToListAsync();
        }

        public async Task<ExchangeOffer> GetOfferWithBookAndSellerDetailsAsync(int offerId)
        {
            return await _context.ExchangeOffers
                                 .Include(o => o.Book)
                                 .ThenInclude(b => b.Subject)
                                 .Include(o => o.Seller)
                                 .FirstOrDefaultAsync(o => o.Id == offerId);
        }
        public async Task<IEnumerable<ExchangeOffer>> GetAllOffersWithDetailsAsync()
        {
            return await _context.ExchangeOffers
                                 // Incluye el libro asociado a la oferta
                                 .Include(o => o.Book)
                                 // Incluye la materia del libro
                                 .ThenInclude(b => b.Subject)
                                 // Incluye el vendedor de la oferta
                                 .Include(o => o.Seller)
                                 .ToListAsync();
        }
    }
}
