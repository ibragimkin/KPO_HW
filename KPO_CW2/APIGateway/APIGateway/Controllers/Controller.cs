using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class APIGatewayController(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<APIGatewayController> logger) : ControllerBase
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<APIGatewayController> _logger = logger;

    [HttpPost("upload")]
    public async Task UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("A non-empty file is required");
            return;
        }

        var fileStoringServiceUrl = _configuration["FileStoringService:Url"];

        try
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();

            var fileBytes = new byte[file.Length];
            await fileStream.ReadExactlyAsync(fileBytes, 0, (int)file.Length);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            var responseMessage = await _httpClient.PostAsync($"{fileStoringServiceUrl}/api/Files/upload", content);

            // Прокидываем статус
            Response.StatusCode = (int)responseMessage.StatusCode;

            // Прокидываем заголовки
            foreach (var header in responseMessage.Headers)
            {
                Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                Response.Headers[header.Key] = header.Value.ToArray();
            }

            Response.Headers.Remove("transfer-encoding"); // чтобы избежать ошибок с chunked transfer

            // Прокидываем тело
            await responseMessage.Content.CopyToAsync(Response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception occurred while proxying upload");
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await Response.WriteAsync($"Error proxying file upload: {ex.Message}");
        }
    }


    /// <summary>
    /// Analyzes a file using the FileAnalysisService.
    /// </summary>
    [HttpPost("analyze/{fileId:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AnalyzeFile(Guid fileId)
    {
        var fileAnalysisServiceUrl = _configuration["FileAnalysisService:Url"];

        var response = await _httpClient.PostAsync($"{fileAnalysisServiceUrl}/api/Analysis/{fileId}", null);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return NotFound("File not found in analysis service");

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error analyzing file");

        var analysisResult = await response.Content.ReadFromJsonAsync<object>();
        return Ok(analysisResult);
    }

    /// <summary>
    /// Retrieves a file from the FileStoringService.
    /// </summary>
    [HttpGet("file/{fileId:guid}")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var fileStoringServiceUrl = _configuration["FileStoringService:Url"];
        var response = await _httpClient.GetAsync($"{fileStoringServiceUrl}/api/Files/download?id={fileId}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return NotFound("File not found");

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Error retrieving file");

        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"{fileId}.txt";

        var stream = await response.Content.ReadAsStreamAsync();
        return File(stream, contentType, fileName);
    }


    private class UploadResult
    {
        public Guid FileId { get; set; }
    }
}
