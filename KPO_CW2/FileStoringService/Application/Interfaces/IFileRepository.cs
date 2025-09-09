using System;
using System.IO;
using System.Threading.Tasks;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.ValueObjects;

namespace FileStoringService.Application.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с файлами и их метаданными.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Сохраняет содержимое файла и его метаданные.
        /// </summary>
        /// <param name="fileId">Уникальный идентификатор создаваемого файла.</param>
        /// <param name="content">Поток с данными файла.</param>
        /// <param name="hash">Предварительно вычисленный хеш файла.</param>
        /// <param name="uploadTime">Время загрузки файла.</param>
        /// <param name="name">Имя файла.</param>
        /// <returns>Сущность метаданных сохранённого файла.</returns>
        Task<FileMetadata> SaveAsync(Guid fileId, Stream content, FileHash hash, DateTime uploadTime, string name);

        /// <summary>
        /// Возвращает поток содержимого файла по его идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Поток с данными файла.</returns>
        Task<Stream> GetContentAsync(Guid fileId);

        /// <summary>
        /// Возвращает метаданные файла по его идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Сущность метаданных файла.</returns>
        Task<FileMetadata> GetMetadataAsync(Guid fileId);

        /// <summary>
        /// Ищет файл по его хешу (для выявления 100% совпадения).
        /// </summary>
        /// <param name="hash">Хеш искомого файла.</param>
        /// <returns>Сущность метаданных найденного файла или null, если не найден.</returns>
        Task<FileMetadata?> GetByHashAsync(FileHash hash);
    }
}
