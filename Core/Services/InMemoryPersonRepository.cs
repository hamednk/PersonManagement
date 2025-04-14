using Core.Models;

namespace Core.Services;

public class InMemoryPersonRepository : IPersonRepository
{
    private readonly Dictionary<string, Person> _persons = [];

    public Task<Person> CreateAsync(Person person)
    {
        person.Id = Guid.NewGuid().ToString();
        _persons[person.Id] = person;
        return Task.FromResult(person);
    }

    public Task<Person> GetByIdAsync(string id)
    {
        _persons.TryGetValue(id, out var person);
        return Task.FromResult(person);
    }

    public Task<IEnumerable<Person>> GetAllAsync(int page, int pageSize)
    {
        var result = _persons.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        return Task.FromResult(result);
    }

    public Task<Person> UpdateAsync(Person person)
    {
        if (!_persons.ContainsKey(person.Id))
        {
            return Task.FromResult<Person>(null);
        }

        _persons[person.Id] = person;
        return Task.FromResult(person);
    }

    public Task<bool> DeleteAsync(string id)
    {
        return Task.FromResult(_persons.Remove(id));
    }
}