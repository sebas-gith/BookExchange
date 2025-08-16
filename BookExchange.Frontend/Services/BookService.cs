using BookExchange.Application.DTOs.Books; // Asegúrate de que esta ruta es correcta
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BookExchange.Frontend.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para publicar un nuevo libro
        public async Task CreateBookAsync(BookCreateDto bookCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/books", bookCreateDto);
            response.EnsureSuccessStatusCode();
        }

        // Método para obtener un solo libro por su ID
        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<BookDto>($"api/books/{id}");
        }

        // Método para obtener todos los libros
        public async Task<List<BookDto>> GetBooksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<BookDto>>("api/books");
        }

        public async Task UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/books/{bookUpdateDto.Id}", bookUpdateDto);
            response.EnsureSuccessStatusCode();
        }

        // Nuevo: Método para eliminar un libro
        public async Task DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/books/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
