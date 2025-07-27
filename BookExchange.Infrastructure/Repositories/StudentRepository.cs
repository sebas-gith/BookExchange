using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookExchange.Infrastructure.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(BookExchangeContext context) : base(context)
        {
        }

        // Implementación de métodos específicos para Student
        public async Task<Student> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithBooksAsync()
        {
            // Ejemplo de carga con relaciones (Eager Loading)
            return await _dbSet.Include(s => s.BooksPublished).ToListAsync();
        }
    }
}