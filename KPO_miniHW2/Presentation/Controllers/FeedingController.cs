using Microsoft.AspNetCore.Mvc;
using ZooManagementSystem.Application.DTOs;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Application.Services;
using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingController : ControllerBase
    {
        private readonly IFeedingScheduleRepository _feedRepo;
        private readonly FeedingOrganizationService _feedingService;

        public FeedingController(
            IFeedingScheduleRepository feedRepo,
            FeedingOrganizationService feedingService)
        {
            _feedRepo = feedRepo;
            _feedingService = feedingService;
        }

        /// <summary>
        /// Получить все записи расписания кормлений.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedingScheduleDto>>> GetAll()
        {
            var list = await _feedRepo.GetAllAsync();
            var dtos = list.Select(s => new FeedingScheduleDto
            {
                Id = s.Id,
                AnimalId = s.AnimalId,
                Time = s.FeedingTime.Time.ToString("HH:mm"),
                FoodType = s.FoodType,
                IsCompleted = s.IsCompleted
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Получить расписание кормлений для конкретного животного.
        /// </summary>
        [HttpGet("animal/{animalId:guid}")]
        public async Task<ActionResult<IEnumerable<FeedingScheduleDto>>> GetByAnimal(Guid animalId)
        {
            var list = await _feedRepo.GetByAnimalIdAsync(animalId);
            var dtos = list.Select(s => new FeedingScheduleDto
            {
                Id = s.Id,
                AnimalId = s.AnimalId,
                Time = s.FeedingTime.Time.ToString("HH:mm"),
                FoodType = s.FoodType,
                IsCompleted = s.IsCompleted
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Добавить запись в расписание кормлений.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateFeedingRequest req)
        {
            var time = TimeOnly.Parse(req.Time);
            await _feedingService.AddFeedingAsync(req.AnimalId, new FeedingTime(time), req.FoodType);
            return CreatedAtAction(nameof(GetByAnimal), new { animalId = req.AnimalId }, null);
        }

        /// <summary>
        /// Отметить кормление как выполненное.
        /// </summary>
        [HttpPost("{id:guid}/execute")]
        public async Task<IActionResult> Execute(Guid id)
        {
            await _feedingService.ExecuteFeedingAsync(id);
            return NoContent();
        }

        #region Request Models

        /// <summary>Модель для создания записи кормления.</summary>
        public class CreateFeedingRequest
        {
            public Guid AnimalId { get; set; }
            public string Time { get; set; } = default!;       // "HH:mm"
            public string FoodType { get; set; } = default!;
        }

        #endregion
    }
}
