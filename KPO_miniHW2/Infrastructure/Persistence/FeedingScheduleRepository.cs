using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;

namespace ZooManagementSystem.Infrastructure.Persistence;

/// <summary>
/// In-memory реализация репозитория расписания кормлений.
/// </summary>
public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly Dictionary<Guid, FeedingSchedule> _schedules = new();

    public Task AddAsync(FeedingSchedule schedule)
    {
        _schedules[schedule.Id] = schedule;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _schedules.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
    {
        return Task.FromResult(_schedules.Values.AsEnumerable());
    }

    public Task<FeedingSchedule?> GetByIdAsync(Guid id)
    {
        _schedules.TryGetValue(id, out var schedule);
        return Task.FromResult(schedule);
    }

    public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId)
    {
        var result = _schedules.Values.Where(s => s.AnimalId == animalId);
        return Task.FromResult(result);
    }

    public Task UpdateAsync(FeedingSchedule schedule)
    {
        if (_schedules.ContainsKey(schedule.Id))
        {
            _schedules[schedule.Id] = schedule;
        }
        return Task.CompletedTask;
    }
}
