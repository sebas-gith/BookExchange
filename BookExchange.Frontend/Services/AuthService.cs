using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BookExchange.Application.DTOs.Auth;
using BookExchange.Application.DTOs.Students;

namespace BookExchange.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        }

        public async Task<StudentDto> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
                result.EnsureSuccessStatusCode();

                var student = await result.Content.ReadFromJsonAsync<StudentDto>();
                await _localStorage.SetItemAsync("currentUser", student);
                _authStateProvider.MarkUserAsAuthenticated(student); // Usa el nuevo método
                return student;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task Register(StudentRegisterDto registerDto)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
            Console.WriteLine("Vaina bacana");
            result.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("currentUser");
            _authStateProvider.MarkUserAsLoggedOut(); // Usa el nuevo método
        }
    }
}