using BookExchange.Domain.Entities; // Necesario para Book

namespace BookExchange.Domain.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        // Métodos específicos para Book
        Task<IEnumerable<Book>> GetBooksBySubjectAsync(int subjectId);
        Task<IEnumerable<Book>> GetBooksByOwnerAsync(int ownerId);
        Task<Book> GetBookWithDetailsAsync(int bookId); // Para cargar Subject, Owner, etc.
    }
}