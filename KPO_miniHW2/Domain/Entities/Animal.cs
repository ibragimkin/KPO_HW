using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Domain.Entities;

public class Animal
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Species { get; private set; }
    public string Name { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Gender { get; private set; }
    public string FavoriteFood { get; private set; }
    public AnimalStatus Status { get; private set; }

    public Guid? EnclosureId { get; private set; }

    public Animal(string species, string name, DateOnly birthDate, string gender, string favoriteFood)
    {
        Species = species;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
        FavoriteFood = favoriteFood;
        Status = AnimalStatus.Healthy;
    }

    public void Feed() // Представим, что голодный лев = больной лев)
    {
        Status = AnimalStatus.Healthy;
    }

    public void Heal()
    {
        Status = AnimalStatus.Healthy;
    }

    public void MoveToEnclosure(Guid newEnclosureId)
    {
        EnclosureId = newEnclosureId;
    }

    public void MarkAsSick()
    {
        Status = AnimalStatus.Sick;
    }
}
