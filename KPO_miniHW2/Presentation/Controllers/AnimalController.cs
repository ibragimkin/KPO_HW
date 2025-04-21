using Microsoft.AspNetCore.Mvc;
using ZooManagementSystem.Application.DTOs;
using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Application.Services;

namespace ZooManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly AnimalTransferService _transferService;

        public AnimalController(
            IAnimalRepository animalRepo,
            AnimalTransferService transferService)
        {
            _animalRepo = animalRepo;
            _transferService = transferService;
        }

        /// <summary>
        /// Получить список всех животных.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAll()
        {
            var animals = await _animalRepo.GetAllAsync();
            var dtos = animals.Select(a => new AnimalDto
            {
                Id = a.Id,
                Species = a.Species,
                Name = a.Name,
                BirthDate = a.BirthDate,
                Gender = a.Gender,
                FavoriteFood = a.FavoriteFood,
                Status = a.Status.ToString(),
                EnclosureId = a.EnclosureId
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Получить животное по идентификатору.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AnimalDto>> GetById(Guid id)
        {
            var a = await _animalRepo.GetByIdAsync(id);
            if (a == null) return NotFound();
            var dto = new AnimalDto
            {
                Id = a.Id,
                Species = a.Species,
                Name = a.Name,
                BirthDate = a.BirthDate,
                Gender = a.Gender,
                FavoriteFood = a.FavoriteFood,
                Status = a.Status.ToString(),
                EnclosureId = a.EnclosureId
            };
            return Ok(dto);
        }

        /// <summary>
        /// Добавить новое животное.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AnimalDto>> Create([FromBody] CreateAnimalRequest req)
        {
            var animal = new Animal(
                req.Species,
                req.Name,
                req.BirthDate,
                req.Gender,
                req.FavoriteFood);

            await _animalRepo.AddAsync(animal);

            var dto = new AnimalDto
            {
                Id = animal.Id,
                Species = animal.Species,
                Name = animal.Name,
                BirthDate = animal.BirthDate,
                Gender = animal.Gender,
                FavoriteFood = animal.FavoriteFood,
                Status = animal.Status.ToString(),
                EnclosureId = animal.EnclosureId
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Удалить животное.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _animalRepo.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Переместить животное в другой вольер.
        /// </summary>
        [HttpPost("{id:guid}/transfer")]
        public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferAnimalRequest req)
        {
            await _transferService.TransferAnimalAsync(id, req.TargetEnclosureId);
            return NoContent();
        }

        #region Request Models

        /// <summary>Модель для создания животного.</summary>
        public class CreateAnimalRequest
        {
            public string Species { get; set; } = default!;
            public string Name { get; set; } = default!;
            public DateOnly BirthDate { get; set; }
            public string Gender { get; set; } = default!;
            public string FavoriteFood { get; set; } = default!;
        }

        /// <summary>Модель для перемещения животного.</summary>
        public class TransferAnimalRequest
        {
            public Guid TargetEnclosureId { get; set; }
        }

        #endregion
    }
}
