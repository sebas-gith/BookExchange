using BookExchange.Domain.Entities; // Necesario para Subject
using System.Threading.Tasks;

namespace BookExchange.Domain.Interfaces
{
    public interface ISubjectRepository : IBaseRepository<Subject>
    {
        // Métodos específicos para Subject
        Task<Subject> GetSubjectByNameAsync(string name);
    }
}
