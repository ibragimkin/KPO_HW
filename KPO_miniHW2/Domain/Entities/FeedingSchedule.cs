using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Domain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid AnimalId { get; private set; }
    public FeedingTime FeedingTime { get; private set; }
    public string FoodType { get; private set; }
    public bool IsCompleted { get; private set; }

    public FeedingSchedule(Guid animalId, FeedingTime time, string foodType)
    {
        AnimalId = animalId;
        FeedingTime = time;
        FoodType = foodType;
        IsCompleted = false;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
    }

    public void Reschedule(FeedingTime newTime)
    {
        FeedingTime = newTime;
        IsCompleted = false;
    }
}
