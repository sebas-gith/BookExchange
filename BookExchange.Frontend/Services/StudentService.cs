using BookExchange.Application.DTOs.Students;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BookExchange.Frontend.Services
{
    public class StudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StudentDto> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<StudentDto>($"api/students/{id}");
        }
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<StudentDto>>("api/students");
        }
    }
}
