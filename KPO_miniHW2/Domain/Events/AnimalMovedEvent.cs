using ZooManagementSystem.Domain.Events;

public class AnimalMovedEvent : IDomainEvent
{
    public Guid AnimalId { get; }
    public Guid OldEnclosureId { get; }
    public Guid NewEnclosureId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public AnimalMovedEvent(Guid animalId, Guid oldEnclosureId, Guid newEnclosureId)
    {
        AnimalId = animalId;
        OldEnclosureId = oldEnclosureId;
        NewEnclosureId = newEnclosureId;
    }
}
