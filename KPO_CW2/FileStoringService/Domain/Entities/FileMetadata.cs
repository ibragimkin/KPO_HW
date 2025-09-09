using System;
using FileStoringService.Domain.ValueObjects;

namespace FileStoringService.Domain.Entities
{
    /// <summary>
    /// Сущность, описывающая метаданные загруженного файла.
    /// </summary>
    public class FileMetadata
    {
        /// <summary>
        /// Уникальный идентификатор файла.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Хеш файла (Value Object).
        /// </summary>
        public FileHash Hash { get; }

        /// <summary>
        /// Время загрузки файла.
        /// </summary>
        public DateTime UploadTime { get; }

        /// <summary>
        /// Путь к физическому расположению файла в хранилище.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Конструктор для создания нового экземпляра метаданных файла.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <param name="hash">Хеш файла.</param>
        /// <param name="uploadTime">Время загрузки.</param>
        /// <param name="filePath">Путь к файлу в хранилище.</param>
        /// <param name="name">Имя файла..</param>
        /// 
        public FileMetadata(Guid id, FileHash hash, DateTime uploadTime, string filePath, string name)
        {
            Id = id != Guid.Empty
                ? id
                : throw new ArgumentException("Id не может быть пустым.", nameof(id));

            Hash = hash
                ?? throw new ArgumentNullException(nameof(hash), "Hash не может быть null.");

            UploadTime = uploadTime;

            FilePath = !string.IsNullOrWhiteSpace(filePath)
                ? filePath
                : throw new ArgumentException("FilePath не может быть пустым.", nameof(filePath));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Название файла не может быть пустым.", nameof(Name));
        }
    }
}
