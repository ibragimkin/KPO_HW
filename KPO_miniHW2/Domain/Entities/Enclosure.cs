using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Domain.Entities;

public class Enclosure
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Type { get; private set; }
    public int Size { get; private set; }
    public int MaxCapacity { get; private set; }

    private readonly List<Guid> _animalIds = new();
    public IReadOnlyCollection<Guid> AnimalIds => _animalIds.AsReadOnly();

    public Enclosure(string type, int size, int maxCapacity)
    {
        Type = type;
        Size = size;
        MaxCapacity = maxCapacity;
    }

    public bool CanAddAnimal() => _animalIds.Count < MaxCapacity;

    public void AddAnimal(Guid animalId)
    {
        if (!CanAddAnimal())
            throw new InvalidOperationException("Вольер переполнен.");
        _animalIds.Add(animalId);
    }

    public void RemoveAnimal(Guid animalId)
    {
        _animalIds.Remove(animalId);
    }

    public void Clean()
    {
        // Логика уборки вольера
    }
}
