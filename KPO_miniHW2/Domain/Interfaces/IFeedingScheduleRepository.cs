using ZooManagementSystem.Domain.Entities;

namespace ZooManagementSystem.Domain.Interfaces;

/// <summary>
/// Репозиторий для управления расписанием кормлений.
/// </summary>
public interface IFeedingScheduleRepository
{
    /// <summary>
    /// Получает вольер по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор вольера.</param>
    /// <returns>Вольер.</returns>
    Task<FeedingSchedule?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает все расписания кормлений.
    /// </summary>
    Task<IEnumerable<FeedingSchedule>> GetAllAsync();

    Task AddAsync(FeedingSchedule schedule);

    /// <summary>
    /// Удаляет запись кормления.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Обновляет запись кормления.
    /// </summary>
    Task UpdateAsync(FeedingSchedule schedule);

    /// <summary>
    /// Получает расписание для конкретного животного.
    /// </summary>
    Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId);
}
