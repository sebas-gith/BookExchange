using AutoMapper;
using BookExchange.Application.DTOs.Auth;
using BookExchange.Application.DTOs.Students;
using BookExchange.Application.Exceptions;
using BookExchange.Domain.Core;
using BookExchange.Domain.Interfaces;
// using Microsoft.Extensions.Configuration; // Ya no necesario si no se usa JWT
// using System.IdentityModel.Tokens.Jwt; // Ya no necesario
// using System.Security.Claims; // Ya no necesario para generar token
// using System.Text; // Ya no necesario
using System.Threading.Tasks;
using BCrypt.Net;
using BookExchange.Domain.Entities;

namespace BookExchange.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public AuthService(IStudentRepository studentRepository, IMapper mapper /*, IConfiguration configuration*/)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<StudentDto> RegisterAsync(StudentRegisterDto registerDto)
        {
            var existingStudent = await _studentRepository.GetByEmailAsync(registerDto.Email);
            if (existingStudent != null)
            {
                throw new Exceptions.ApplicationException("El correo electrónico ya está registrado.");
            }

            var student = _mapper.Map<Student>(registerDto);
            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            student.RegistrationDate = DateTime.UtcNow;

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> LoginAsync(LoginDto loginDto) // Devuelve StudentDto directamente
        {
            var student = await _studentRepository.GetByEmailAsync(loginDto.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, student.PasswordHash))
            {
                throw new Exceptions.ApplicationException("Credenciales inválidas.");
            }

            return _mapper.Map<StudentDto>(student);
        }
    }
}