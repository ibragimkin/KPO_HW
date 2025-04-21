using ZooManagementSystem.Domain.Events;

namespace ZooManagementSystem.Infrastructure.Events;

/// <summary>
/// Публикует события и вызывает зарегистрированные обработчики.
/// </summary>
public class EventPublisher
{
    private readonly Dictionary<Type, List<Func<IDomainEvent, Task>>> _handlers = new();

    /// <summary>
    /// Регистрирует обработчик для конкретного типа события.
    /// </summary>
    public void Register<TEvent>(Func<TEvent, Task> handler) where TEvent : IDomainEvent
    {
        var type = typeof(TEvent);

        if (!_handlers.ContainsKey(type))
            _handlers[type] = new List<Func<IDomainEvent, Task>>();

        _handlers[type].Add(async (e) => await handler((TEvent)e));
    }

    /// <summary>
    /// Публикует событие и вызывает всех подписчиков.
    /// </summary>
    public async Task PublishAsync(IDomainEvent domainEvent)
    {
        var type = domainEvent.GetType();

        if (_handlers.TryGetValue(type, out var handlers))
        {
            foreach (var handler in handlers)
                await handler(domainEvent);
        }
    }
}
