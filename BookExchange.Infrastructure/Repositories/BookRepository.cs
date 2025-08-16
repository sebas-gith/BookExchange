using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookExchange.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(BookExchangeContext context) : base(context)
        {
        }

        
        public async Task<IEnumerable<Book>> GetBooksBySubjectAsync(int subjectId)
        {
            return await _dbSet.Where(b => b.SubjectId == subjectId).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByOwnerAsync(int ownerId)
        {
            return await _dbSet.Where(b => b.OwnerId == ownerId).ToListAsync();
        }

        public async Task<Book> GetBookWithDetailsAsync(int bookId)
        {
            return await _dbSet
                .Include(b => b.Subject) // Cargar la materia
                .Include(b => b.Owner)   // Cargar el propietario (estudiante)
                .FirstOrDefaultAsync(b => b.Id == bookId);
        }
    }
}
