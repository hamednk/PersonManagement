using Core.Models;

namespace Core.Services;

public interface IPersonRepository
{
    Task<Person> CreateAsync(Person person);
    Task<Person> GetByIdAsync(string id);
    Task<IEnumerable<Person>> GetAllAsync(int page, int pageSize);
    Task<Person> UpdateAsync(Person person);
    Task<bool> DeleteAsync(string id);
}