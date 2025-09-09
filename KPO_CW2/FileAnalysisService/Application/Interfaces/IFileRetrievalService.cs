using System;
using System.IO;
using System.Threading.Tasks;

namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Сервис для получения содержимого файла и сопутствующих метаданных из FileStoringService.
    /// </summary>
    public interface IFileRetrievalService
    {
        /// <summary>
        /// Возвращает поток содержимого файла по его идентификатору,
        /// а также строки с SHA-256-хешем и временем загрузки (UTC),
        /// которые FileStoringService передаёт в заголовках ответа.
        /// </summary>
        /// <param name="fileId">GUID файла в FileStoringService.</param>
        /// <returns>
        /// Кортеж, содержащий:
        /// - Stream Content: поток для чтения содержимого файла,
        /// - string Hash: SHA-256-хеш как hex-строка (берётся из заголовка X-File-Hash),
        /// - DateTime UploadTime: время загрузки в UTC (берётся из заголовка X-File-UploadTime).
        /// </returns>
        Task<(Stream Content, string Hash, DateTime UploadTime)> GetFileAsync(Guid fileId);
    }
}
