using ZooManagementSystem.Domain.Events;

namespace ZooManagementSystem.Infrastructure.Events.EventHandlers;

/// <summary>
/// Обработчик события наступления времени кормления.
/// </summary>
public class FeedingTimeHandler
{
    /// <summary>
    /// Обрабатывает FeedingTimeEvent (логирует).
    /// </summary>
    public Task HandleAsync(FeedingTimeEvent @event)
    {
        Console.WriteLine($"[EVENT] Кормление выполнено по расписанию: {@event.FeedingScheduleId}");
        return Task.CompletedTask;
    }
}
