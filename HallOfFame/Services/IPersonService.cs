using HallOfFame.Models;

namespace HallOfFame.Services
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(long id);
        Task<Person> CreateAsync(Person person);
        Task<Person?> UpdateAsync(long id, Person person);
        Task<bool> DeleteAsync(long id);
    }
}