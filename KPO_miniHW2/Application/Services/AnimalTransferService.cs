using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Domain.Events;

namespace ZooManagementSystem.Application.Services;

/// <summary>
/// Сервис для перемещения животных между вольерами.
/// </summary>
public class AnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public AnimalTransferService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
    {
        _animalRepository = animalRepo;
        _enclosureRepository = enclosureRepo;
    }

    /// <summary>
    /// Перемещает животное в другой вольер.
    /// </summary>
    public async Task<AnimalMovedEvent> TransferAnimalAsync(Guid animalId, Guid targetEnclosureId)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId)
            ?? throw new InvalidOperationException("Животное не найдено");

        var currentEnclosureId = animal.EnclosureId;

        if (currentEnclosureId == targetEnclosureId)
            throw new InvalidOperationException("Животное уже находится в этом вольере");

        var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId)
            ?? throw new InvalidOperationException("Целевой вольер не найден");

        if (!targetEnclosure.CanAddAnimal())
            throw new InvalidOperationException("Вольер переполнен");

        if (currentEnclosureId is Guid sourceId)
        {
            var oldEnclosure = await _enclosureRepository.GetByIdAsync(sourceId);
            oldEnclosure?.RemoveAnimal(animalId);
            await _enclosureRepository.UpdateAsync(oldEnclosure!);
        }

        targetEnclosure.AddAnimal(animalId);
        await _enclosureRepository.UpdateAsync(targetEnclosure);

        animal.MoveToEnclosure(targetEnclosureId);
        await _animalRepository.UpdateAsync(animal);

        return new AnimalMovedEvent(animalId, currentEnclosureId ?? Guid.Empty, targetEnclosureId);
    }
}
