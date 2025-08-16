using BookExchange.Application.DTOs.Auth;
using BookExchange.Application.DTOs.Students;
using System.Threading.Tasks;

namespace BookExchange.Application
{
    public interface IAuthService
    {
        Task<StudentDto> RegisterAsync(StudentRegisterDto registerDto);
        Task<StudentDto> LoginAsync(LoginDto loginDto); 
    }
}
