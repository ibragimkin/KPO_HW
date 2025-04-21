using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Application.DTOs;

namespace ZooManagementSystem.Application.Services;

/// <summary>
/// Сервис для сбора статистики по зоопарку.
/// </summary>
public class ZooStatisticsService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public ZooStatisticsService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
    {
        _animalRepository = animalRepo;
        _enclosureRepository = enclosureRepo;
    }

    /// <summary>
    /// Получает общее количество животных.
    /// </summary>
    public async Task<int> GetTotalAnimalsAsync()
    {
        var allAnimals = await _animalRepository.GetAllAsync();
        return allAnimals.Count();
    }

    /// <summary>
    /// Получает количество свободных вольеров.
    /// </summary>
    public async Task<int> GetFreeEnclosuresAsync()
    {
        var enclosures = await _enclosureRepository.GetAllAsync();
        return enclosures.Count(e => e.AnimalIds.Count < e.MaxCapacity);
    }

    /// <summary>
    /// Получает всю статистику.
    /// </summary>
    public async Task<ZooStatisticsDto> GetStatisticsAsync()
    {
        return new ZooStatisticsDto
        {
            TotalAnimals = await GetTotalAnimalsAsync(),
            FreeEnclosures = await GetFreeEnclosuresAsync()
        };
    }
}
