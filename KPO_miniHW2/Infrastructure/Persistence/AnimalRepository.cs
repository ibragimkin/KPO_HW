using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;

namespace ZooManagementSystem.Infrastructure.Persistence;

/// <summary>
/// In-memory реализация репозитория животных.
/// </summary>
public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly Dictionary<Guid, Animal> _animals = new();

    public Task AddAsync(Animal animal)
    {
        _animals[animal.Id] = animal;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _animals.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Animal>> GetAllAsync()
    {
        return Task.FromResult(_animals.Values.AsEnumerable());
    }

    public Task<Animal?> GetByIdAsync(Guid id)
    {
        _animals.TryGetValue(id, out var animal);
        return Task.FromResult(animal);
    }

    public Task UpdateAsync(Animal animal)
    {
        if (_animals.ContainsKey(animal.Id))
        {
            _animals[animal.Id] = animal;
        }
        return Task.CompletedTask;
    }
}
