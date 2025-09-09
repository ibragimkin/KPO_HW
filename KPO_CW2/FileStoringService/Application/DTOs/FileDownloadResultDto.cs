// File: FileStoringService.Application/DTOs/FileDownloadResultDto.cs
using System;
using System.IO;

namespace FileStoringService.Application.DTOs
{
    /// <summary>
    /// Результат выполнения сценария получения файла:
    /// метаданные + поток с содержимым.
    /// </summary>
    public class FileDownloadResultDto
    {
        /// <summary>
        /// Идентификатор файла.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Шестнадцатеричное представление хеша файла.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// UTC-время первоначальной загрузки файла.
        /// </summary>
        public DateTime UploadTime { get; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Поток для чтения содержимого файла.
        /// </summary>
        public Stream Content { get; }

        public FileDownloadResultDto(Guid id, string hash, DateTime uploadTime, string name, Stream content)
        {
            Id = id;
            Hash = !string.IsNullOrWhiteSpace(hash)
                ? hash
                : throw new ArgumentNullException(nameof(hash));
            UploadTime = uploadTime;
            Content = content
                ?? throw new ArgumentNullException(nameof(content));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Название файла не может быть пустым.", nameof(Name));
        }
    }
}
