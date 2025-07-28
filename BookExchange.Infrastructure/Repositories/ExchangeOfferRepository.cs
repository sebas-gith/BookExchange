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

        public async Task<IEnumerable<ExchangeOffer>> GetOffersFilteredAsync(decimal? minPrice, decimal? maxPrice, int? subjectId, string condition)
        {
            IQueryable<ExchangeOffer> query = _dbSet.Include(eo => eo.Book); // Siempre cargar el libro

            if (minPrice.HasValue)
            {
                query = query.Where(eo => eo.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(eo => eo.Price <= maxPrice.Value);
            }
            if (subjectId.HasValue)
            {
                query = query.Where(eo => eo.Book.SubjectId == subjectId.Value);
            }
            if (!string.IsNullOrEmpty(condition))
            {
                // Convierte la cadena a enum para comparar
                if (Enum.TryParse(condition, true, out BookCondition bookCondition))
                {
                    query = query.Where(eo => eo.Book.Condition == bookCondition);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<ExchangeOffer> GetOfferWithBookAndSellerDetailsAsync(int offerId)
        {
            return await _dbSet
                .Include(eo => eo.Book)
                    .ThenInclude(b => b.Subject) // Cargar la materia del libro
                .Include(eo => eo.Seller)       // Cargar el estudiante vendedor
                .FirstOrDefaultAsync(eo => eo.Id == offerId);
        }
    }
}
