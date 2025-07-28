using AutoMapper;
using BookExchange.Application.DTOs.Students;
using BookExchange.Domain.Core;
using BookExchange.Domain.Interfaces; // Aquí se referencian las interfaces de repositorio
using System.Threading.Tasks;
using System.Collections.Generic;
using BCrypt.Net; // Para hashear contraseñas 
using BookExchange.Domain.Entities;
using BookExchange.Application.Contracts;


namespace BookExchange.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<StudentDto> RegisterStudentAsync(StudentRegisterDto registerDto)
        {
            // Verificar si el email ya existe
            var existingStudent = await _studentRepository.GetByEmailAsync(registerDto.Email);
            if (existingStudent != null)
            {
                // Podrías lanzar una excepción personalizada aquí
                throw new ApplicationException("El correo electrónico ya está registrado.");
            }

            var student = _mapper.Map<Student>(registerDto);
            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password); // Hashear la contraseña
            student.RegistrationDate = DateTime.UtcNow; // Asegurar fecha de registro

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync(); // Guardar cambios

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task UpdateStudentAsync(StudentUpdateDto updateDto)
        {
            var studentToUpdate = await _studentRepository.GetByIdAsync(updateDto.Id);
            if (studentToUpdate == null)
            {
                throw new KeyNotFoundException($"Estudiante con ID {updateDto.Id} no encontrado.");
            }

            // Mapear solo las propiedades que se pueden actualizar
            _mapper.Map(updateDto, studentToUpdate);

            _studentRepository.Update(studentToUpdate);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            var studentToDelete = await _studentRepository.GetByIdAsync(studentId);
            if (studentToDelete == null)
            {
                throw new KeyNotFoundException($"Estudiante con ID {studentId} no encontrado.");
            }

            _studentRepository.Remove(studentToDelete);
            await _studentRepository.SaveChangesAsync();
        }

        public async Task<StudentDto> GetStudentByEmailAsync(string email)
        {
            var student = await _studentRepository.GetByEmailAsync(email);
            return _mapper.Map<StudentDto>(student);
        }
    }
}