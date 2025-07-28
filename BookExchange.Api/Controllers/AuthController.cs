using BookExchange.Application.Contracts;
using BookExchange.Application;
using BookExchange.Application.DTOs.Auth;
using BookExchange.Application.DTOs.Students;
using BookExchange.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStudentService _studentService; // Para poder usar el RegisterStudentAsync

        public AuthController(IAuthService authService, IStudentService studentService)
        {
            _authService = authService;
            _studentService = studentService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] StudentRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var student = await _studentService.RegisterStudentAsync(registerDto); // Usa StudentService para el registro
                return CreatedAtAction(nameof(Register), student);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var studentDto = await _authService.LoginAsync(loginDto);
                return Ok(studentDto); // Devuelve la información del estudiante
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}