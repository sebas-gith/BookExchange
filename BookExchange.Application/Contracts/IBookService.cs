using BookExchange.Application.DTOs.Books;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Services
{
    public interface IBookService
    {
        Task<BookDto> CreateBookAsync(BookCreateDto createDto, int ownerId); // ownerId viene del usuario autenticado
        Task<BookDto> GetBookByIdAsync(int bookId);
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task UpdateBookAsync(BookUpdateDto updateDto); // Necesitarías un BookUpdateDto similar a StudentUpdateDto
        Task DeleteBookAsync(int bookId);
        Task<IEnumerable<BookDto>> GetBooksBySubjectAsync(int subjectId);
        Task<IEnumerable<BookDto>> GetBooksByOwnerAsync(int ownerId);
        Task<BookDto> GetBookWithDetailsAsync(int bookId);
    }
}