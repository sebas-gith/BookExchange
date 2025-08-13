using System.Net.Http.Json; 
using BookExchange.Application.DTOs.Books;

namespace BookExchange.Frontend.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookDto[]> GetBooksAsync()
        {
            return await _httpClient.GetFromJsonAsync<BookDto[]>("books");
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BookDto>($"books/{id}");
        }

        public async Task CreateBookAsync(BookCreateDto newBook)
        {
            await _httpClient.PostAsJsonAsync("books", newBook);
        }

    }
}
