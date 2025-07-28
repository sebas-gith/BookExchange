using BookExchange.Domain.Entities; // Necesario para Subject

namespace BookExchange.Domain.Interfaces
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {
        // Métodos específicos para Subject
        Task<Subject> GetSubjectByNameAsync(string name);
    }
}
