# ZooManagementSystem


## Эндпоинты

| Use Case                                      | HTTP Endpoint                          | Реализация в коде                                |
|----------------------------------------------|----------------------------------------|--------------------------------------------------|
| Добавить животное                             | `POST /api/animal`                     | `AnimalController.cs`, `InMemoryAnimalRepository.cs` |
| Удалить животное                              | `DELETE /api/animal/{id}`             | `AnimalController.cs`                            |
| Добавить вольер                                | `POST /api/enclosure`                 | `EnclosureController.cs`, `InMemoryEnclosureRepository.cs` |
| Удалить вольер                                 | `DELETE /api/enclosure/{id}`         | `EnclosureController.cs`                         |
| Переместить животное между вольерами          | `POST /api/animal/{id}/transfer`      | `AnimalTransferService.cs`, `AnimalMovedEvent.cs` |
| Просмотреть расписание кормлений              | `GET /api/feeding`, `/animal/{id}`    | `FeedingController.cs`, `InMemoryFeedingScheduleRepository.cs` |
| Добавить кормление в расписание               | `POST /api/feeding`                   | `FeedingController.cs`, `FeedingOrganizationService.cs` |
| Отметить кормление как выполненное            | `POST /api/feeding/{id}/execute`      | `FeedingOrganizationService.cs`, `FeedingTimeEvent.cs` |

---

Приложение реализовано в соответствии с принципами **Clean Architecture**:


ZooManagementSystem/

├── Domain/         # Модели, интерфейсы, Value Objects, события

├── Application/    # Use Case сервисы, DTO

├── Infrastructure/ # Реализация репозиториев и событий

└── Presentation/   # Web API контроллеры



### Зависимости направлены внутрь:
- Presentation → Application → Domain
- Все внешние зависимости инвертированы через интерфейсы

---

##  Domain-Driven Design (DDD)

| Концепция            | Применение в проекте                            | Файл / Модуль                                |
|----------------------|--------------------------------------------------|----------------------------------------------|
| **Сущности (Entities)**         | Животное, Вольер, Кормление                 | `Animal.cs`, `Enclosure.cs`, `FeedingSchedule.cs` |
| **Value Objects**              | Тип, статус животного, время кормления       | `AnimalType.cs`, `AnimalStatus.cs`, `FeedingTime.cs` |
| **Доменные события**           | `AnimalMovedEvent`, `FeedingTimeEvent`       | `Domain/Events/`                              |
| **Инкапсуляция поведения**     | Методы: `MoveToEnclosure`, `Feed`, `Heal`    | Внутри классов сущностей                      |

---

## Clean Architecture

| Принцип                                 | Как реализован                                      |
|-----------------------------------------|-----------------------------------------------------|
| **Изоляция бизнес-логики**             | `AnimalTransferService`, `FeedingOrganizationService`, `ZooStatisticsService` |
| **Интерфейсы для инфраструктуры**      | `IAnimalRepository`, `IEnclosureRepository`, `IFeedingScheduleRepository` |
| **In-memory реализация**               | `InMemory*Repository.cs`                           |
| **События и их обработка**             | `EventPublisher.cs`, `AnimalMovedHandler.cs`, `FeedingTimeHandler.cs` |
| **Web API контроллеры**                | `AnimalController`, `EnclosureController`, `FeedingController` |

---

