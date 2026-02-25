using PeopleManager.Models;

namespace PeopleManager.Services;

public class PersonRepository
{
    private readonly Dictionary<int, Person> _people = new();
    private int _nextId = 1;

    public IEnumerable<Person> GetAll() => _people.Values.ToList();

    public Person? GetById(int id) => _people.TryGetValue(id, out var person) ? person : null;

    public Person Add(Person person)
    {
        person.Id = _nextId++;
        _people[person.Id] = person;
        return person;
    }

    public bool Update(Person person)
    {
        if (!_people.ContainsKey(person.Id))
            return false;

        _people[person.Id] = person;
        return true;
    }

    public bool Delete(int id)
    {
        return _people.Remove(id);
    }

    public bool Exists(int id) => _people.ContainsKey(id);
}
