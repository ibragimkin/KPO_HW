using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Domain.Events;
using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Application.Services;

/// <summary>
/// Сервис для управления процессом кормления.
/// </summary>
public class FeedingOrganizationService
{
    private readonly IFeedingScheduleRepository _feedingRepository;
    private readonly IAnimalRepository _animalRepository;

    public FeedingOrganizationService(IFeedingScheduleRepository feedingRepo, IAnimalRepository animalRepo)
    {
        _feedingRepository = feedingRepo;
        _animalRepository = animalRepo;
    }

    /// <summary>
    /// Добавляет запись в расписание кормлений.
    /// </summary>
    public async Task AddFeedingAsync(Guid animalId, FeedingTime time, string foodType)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId)
            ?? throw new InvalidOperationException("Животное не найдено");

        var schedule = new FeedingSchedule(animalId, time, foodType);
        await _feedingRepository.AddAsync(schedule);
    }

    /// <summary>
    /// Выполняет кормление животного и помечает как выполненное.
    /// </summary>
    public async Task<FeedingTimeEvent> ExecuteFeedingAsync(Guid feedingScheduleId)
    {
        var schedule = await _feedingRepository.GetByIdAsync(feedingScheduleId)
            ?? throw new InvalidOperationException("Запись расписания не найдена");

        var animal = await _animalRepository.GetByIdAsync(schedule.AnimalId)
            ?? throw new InvalidOperationException("Животное не найдено");

        animal.Feed();
        await _animalRepository.UpdateAsync(animal);

        schedule.MarkCompleted();
        await _feedingRepository.UpdateAsync(schedule);

        return new FeedingTimeEvent(feedingScheduleId);
    }
}
