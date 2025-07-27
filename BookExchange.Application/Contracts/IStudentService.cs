using BookExchange.Application.DTOs.Students;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookExchange.Application.Services
{
    public interface IStudentService
    {
        Task<StudentDto> RegisterStudentAsync(StudentRegisterDto registerDto);
        Task<StudentDto> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task UpdateStudentAsync(StudentUpdateDto updateDto);
        Task DeleteStudentAsync(int studentId);
        Task<StudentDto> GetStudentByEmailAsync(string email);     }
}