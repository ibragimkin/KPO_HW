using System;
using System.Threading.Tasks;
using FileStoringService.Application.DTOs;
using FileStoringService.Application.UseCases;
using FileStoringService.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FileStoringService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly UploadFileUseCase _uploadUseCase;
        private readonly GetFileUseCase _getUseCase;

        public FilesController(
            UploadFileUseCase uploadUseCase,
            GetFileUseCase getUseCase)
        {
            _uploadUseCase = uploadUseCase
                ?? throw new ArgumentNullException(nameof(uploadUseCase));
            _getUseCase = getUseCase
                ?? throw new ArgumentNullException(nameof(getUseCase));
        }

        /// <summary>
        /// �������� ������ .txt-����� (multipart/form-data).
        /// ���� ����� �� ��� ��� ���������� � ������������ duplicate.
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
                return BadRequest(new ProblemDetails { Title = "Can't process an empty request." });

            //if (!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            //    return BadRequest(new ProblemDetails { Title = "Only \".txt\" file extensions supported." });

            await using var stream = file.OpenReadStream();
            FileUploadResultDto result;
            
            try
            {
                result = await _uploadUseCase.ExecuteAsync(stream, file.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message });
            }

            return Ok(result);
        }

        /// <summary>
        /// ��������� .txt-����� �� ��� GUID.
        /// </summary>
        [HttpGet("download")]
        public async Task<IActionResult> Download(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ProblemDetails { Title = "Id �� ����� ���� ������." });

            FileDownloadResultDto dto;
            try
            {
                dto = await _getUseCase.ExecuteAsync(id);
            }
            catch (Domain.Exceptions.FileNotFoundException)
            {
                return NotFound(new ProblemDetails { Title = $"���� � Id '{id}' �� ������." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message });
            }

            Response.Headers.Add("X-File-Hash", dto.Hash);
            Response.Headers.Add("X-File-UploadTime", dto.UploadTime.ToString("o"));

            return File(dto.Content, "text/plain", $"{dto.Name}");
        }
    }
}
