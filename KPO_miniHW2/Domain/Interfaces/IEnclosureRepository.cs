using ZooManagementSystem.Domain.Entities;

namespace ZooManagementSystem.Domain.Interfaces;

/// <summary>
/// Репозиторий для управления вольерами.
/// </summary>
public interface IEnclosureRepository
{
    /// <summary>
    /// Получает вольер по идентификатору.
    /// </summary>
    Task<Enclosure?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает все вольеры.
    /// </summary>
    Task<IEnumerable<Enclosure>> GetAllAsync();

    /// <summary>
    /// Добавляет новый вольер.
    /// </summary>
    Task AddAsync(Enclosure enclosure);

    /// <summary>
    /// Удаляет вольер.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Обновляет вольер.
    /// </summary>
    Task UpdateAsync(Enclosure enclosure);
}
