using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

using BookExchange.Application.DTOs.Students;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var student = await _localStorage.GetItemAsync<StudentDto>("currentUser");
            if (student == null)
            {
                return _anonymous;
            }

            // Crea los claims del usuario
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, student.Email),
                new Claim(ClaimTypes.GivenName, student.FirstName),
                new Claim(ClaimTypes.Surname, student.LastName)
            };

            var identity = new ClaimsIdentity(claims, "Authentication");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return _anonymous;
        }
    }

    public void MarkUserAsAuthenticated(StudentDto student)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, student.Email),
            new Claim(ClaimTypes.GivenName, student.FirstName),
            new Claim(ClaimTypes.Surname, student.LastName)
        };

        var identity = new ClaimsIdentity(claims, "Authentication");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }
}