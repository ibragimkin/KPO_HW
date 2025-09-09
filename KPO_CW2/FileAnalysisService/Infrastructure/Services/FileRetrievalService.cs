// File: FileAnalysisService.Infrastructure/Services/FileRetrievalService.cs
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Exceptions;
namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Реализация IFileRetrievalService, использующая HttpClient для запроса к FileStoringService API.
    /// Ожидается, что HttpClient будет сконфигурирован с BaseAddress = "{FileStoringServiceUrl}".
    /// </summary>
    public class FileRetrievalService : IFileRetrievalService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор. HttpClient должен быть зарегистрирован как Typed Client или через IHttpClientFactory
        /// с базовым адресом, указывающим на FileStoringService.
        /// </summary>
        /// <param name="httpClient">HttpClient, настроенный на FileStoringService.</param>
        public FileRetrievalService(HttpClient httpClient)
        {
            _httpClient = httpClient
                ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc />
        public async Task<(Stream Content, string Hash, DateTime UploadTime)> GetFileAsync(Guid fileId)
        {
            if (fileId == Guid.Empty)
                throw new AnalysisException("fileId не может быть пустым.");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync($"/api/Files/download?id={fileId}");
            }
            catch (Exception ex)
            {
                throw new AnalysisException(
                    $"Ошибка при запросе к FileStoringService для файла с Id '{fileId}'.", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new AnalysisException($"FileStoringService вернул 404: файл с Id '{fileId}' не найден. {response.ToString()} {response.Content}");
                }
                throw new AnalysisException(
                    $"FileStoringService вернул ошибку {(int)response.StatusCode} для файла с Id '{fileId}'.");
            }

            if (!response.Headers.TryGetValues("X-File-Hash", out var hashValues))
            {
                throw new AnalysisException("Ответ FileStoringService не содержит заголовка X-File-Hash.");
            }
            if (!response.Headers.TryGetValues("X-File-UploadTime", out var timeValues))
            {
                throw new AnalysisException("Ответ FileStoringService не содержит заголовка X-File-UploadTime.");
            }

            var hash = hashValues.FirstOrDefault();
            var uploadTimeRaw = timeValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(hash))
                throw new AnalysisException("Значение заголовка X-File-Hash пустое.");

            if (string.IsNullOrWhiteSpace(uploadTimeRaw))
                throw new AnalysisException("Значение заголовка X-File-UploadTime пустое.");

            if (!DateTime.TryParse(uploadTimeRaw, null, System.Globalization.DateTimeStyles.RoundtripKind, out var uploadTime))
            {
                throw new AnalysisException($"Не удалось распарсить время '{uploadTimeRaw}' из X-File-UploadTime.");
            }

            Stream contentStream;
            try
            {
                contentStream = await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw new AnalysisException("Ошибка при чтении тела ответа FileStoringService.", ex);
            }

            return (contentStream, hash, uploadTime);
        }
    }
}
