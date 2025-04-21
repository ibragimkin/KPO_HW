namespace ZooManagementSystem.Domain.Events;

/// <summary>
/// Маркерный интерфейс для всех доменных событий.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
