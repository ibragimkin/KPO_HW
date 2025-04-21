using ZooManagementSystem.Domain.Events;

namespace ZooManagementSystem.Infrastructure.Events.EventHandlers;

/// <summary>
/// Обработчик события перемещения животного.
/// </summary>
public class AnimalMovedHandler
{
    /// <summary>
    /// Обрабатывает событие AnimalMovedEvent (логирует).
    /// </summary>
    public Task HandleAsync(AnimalMovedEvent @event)
    {
        Console.WriteLine($"[EVENT] Животное {@event.AnimalId} перемещено из {@event.OldEnclosureId} в {@event.NewEnclosureId}");
        return Task.CompletedTask;
    }
}
