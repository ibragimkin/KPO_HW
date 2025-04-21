using ZooManagementSystem.Domain.Entities;

namespace ZooManagementSystem.Domain.Interfaces;

/// <summary>
/// Репозиторий для управления сущностями животных.
/// </summary>
public interface IAnimalRepository
{
    /// <summary>
    /// Получает животное по его идентификатору.
    /// </summary>
    Task<Animal?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех животных.
    /// </summary>
    Task<IEnumerable<Animal>> GetAllAsync();

    /// <summary>
    /// Добавляет новое животное.
    /// </summary>
    Task AddAsync(Animal animal);

    /// <summary>
    /// Удаляет животное по идентификатору.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Обновляет информацию о животном.
    /// </summary>
    Task UpdateAsync(Animal animal);
}
