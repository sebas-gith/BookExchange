using BookExchange.Domain.Entities;

namespace BookExchange.Domain.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> GetByEmailAsync(string email);
        Task<IEnumerable<Student>> GetStudentsWithBooksAsync(); // Carga de relaciones
    }
}
