using BookExchange.Domain.Core;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BookExchange.Domain.Entities;

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