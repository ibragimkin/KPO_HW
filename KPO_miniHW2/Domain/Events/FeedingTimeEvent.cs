using ZooManagementSystem.Domain.Events;

public class FeedingTimeEvent : IDomainEvent
{
    public Guid FeedingScheduleId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public FeedingTimeEvent(Guid feedingScheduleId)
    {
        FeedingScheduleId = feedingScheduleId;
    }
}
