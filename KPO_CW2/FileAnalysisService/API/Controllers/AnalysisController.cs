using System;
using System.Threading.Tasks;
using FileAnalysisService.Application.DTOs;
using FileAnalysisService.Application.UseCases;
using FileAnalysisService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly AnalyzeFileUseCase _analyzeUseCase;

        public AnalysisController(AnalyzeFileUseCase analyzeUseCase)
        {
            _analyzeUseCase = analyzeUseCase ?? throw new ArgumentNullException(nameof(analyzeUseCase));
        }

        /// <summary>
        /// Анализирует текстовый файл по его идентификатору (GUID).
        /// Если результат уже сохранён в БД, возвращает существующий.
        /// Иначе подтягивает файл из FileStoringService, считает статистику, сохраняет и возвращает результат.
        /// </summary>
        /// <param name="id">GUID файла, заранее загруженного в FileStoringService.</param>
        [HttpPost("{id:guid}")]
        [ProducesResponseType(typeof(FileAnalysisResultDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        public async Task<IActionResult> Analyze([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails { Title = "Id не может быть пустым." });
            }

            FileAnalysisResultDto result;
            try
            {
                result = await _analyzeUseCase.ExecuteAsync(id);
            }
            catch (AnalysisException ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = $"Неожиданная ошибка: {ex.Message}" });
            }

            return Ok(result);
        }
    }
}
