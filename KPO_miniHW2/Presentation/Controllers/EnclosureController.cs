using Microsoft.AspNetCore.Mvc;
using ZooManagementSystem.Application.DTOs;
using ZooManagementSystem.Domain.Entities;
using ZooManagementSystem.Domain.Interfaces;
using ZooManagementSystem.Domain.ValueObjects;

namespace ZooManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosureController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepo;

        public EnclosureController(IEnclosureRepository enclosureRepo)
        {
            _enclosureRepo = enclosureRepo;
        }

        /// <summary>
        /// Получить список всех вольеров.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnclosureDto>>> GetAll()
        {
            var list = await _enclosureRepo.GetAllAsync();
            var dtos = list.Select(e => new EnclosureDto
            {
                Id = e.Id,
                Type = e.Type.ToString(),
                Size = e.Size,
                MaxCapacity = e.MaxCapacity,
                AnimalIds = e.AnimalIds.ToList()
            });
            return Ok(dtos);
        }

        /// <summary>
        /// Получить вольер по идентификатору.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EnclosureDto>> GetById(Guid id)
        {
            var e = await _enclosureRepo.GetByIdAsync(id);
            if (e == null) return NotFound();
            var dto = new EnclosureDto
            {
                Id = e.Id,
                Type = e.Type.ToString(),
                Size = e.Size,
                MaxCapacity = e.MaxCapacity,
                AnimalIds = e.AnimalIds.ToList()
            };
            return Ok(dto);
        }

        /// <summary>
        /// Добавить новый вольер.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EnclosureDto>> Create([FromBody] CreateEnclosureRequest req)
        {
            var enclosure = new Enclosure(
                req.Type,
                req.Size,
                req.MaxCapacity);

            await _enclosureRepo.AddAsync(enclosure);

            var dto = new EnclosureDto
            {
                Id = enclosure.Id,
                Type = enclosure.Type.ToString(),
                Size = enclosure.Size,
                MaxCapacity = enclosure.MaxCapacity,
                AnimalIds = enclosure.AnimalIds.ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Удалить вольер.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _enclosureRepo.DeleteAsync(id);
            return NoContent();
        }

        #region Request Models

        /// <summary>Модель для создания вольера.</summary>
        public class CreateEnclosureRequest
        {
            public string Type { get; set; } = default!;  // "Predator", "Herbivore", etc.
            public int Size { get; set; }
            public int MaxCapacity { get; set; }
        }

        #endregion
    }
}
