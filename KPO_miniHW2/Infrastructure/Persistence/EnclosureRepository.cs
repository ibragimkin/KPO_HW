using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;

namespace ZooManagementSystem.Infrastructure.Persistence;

/// <summary>
/// In-memory реализация репозитория вольеров.
/// </summary>
public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly Dictionary<Guid, Enclosure> _enclosures = new();

    public Task AddAsync(Enclosure enclosure)
    {
        _enclosures[enclosure.Id] = enclosure;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _enclosures.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Enclosure>> GetAllAsync()
    {
        return Task.FromResult(_enclosures.Values.AsEnumerable());
    }

    public Task<Enclosure?> GetByIdAsync(Guid id)
    {
        _enclosures.TryGetValue(id, out var enclosure);
        return Task.FromResult(enclosure);
    }

    public Task UpdateAsync(Enclosure enclosure)
    {
        if (_enclosures.ContainsKey(enclosure.Id))
        {
            _enclosures[enclosure.Id] = enclosure;
        }
        return Task.CompletedTask;
    }
}
