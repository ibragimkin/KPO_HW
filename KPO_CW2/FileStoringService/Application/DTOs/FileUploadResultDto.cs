using System;

namespace FileStoringService.Application.DTOs
{
    /// <summary>
    /// Результат выполнения сценария загрузки файла.
    /// </summary>
    public class FileUploadResultDto
    {
        /// <summary>
        /// Идентификатор файла в системе.
        /// Если файл уже существовал (по совпадению хеша), это Id ранее загруженного файла.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Шестнадцатеричное представление хеша загруженного файла.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// Время (UTC) загрузки файла.
        /// Для существующего файла — время его первоначальной загрузки.
        /// </summary>
        public DateTime UploadTime { get; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Флаг, указывающий, что файл с таким же хешем уже загружался ранее.
        /// </summary>
        public bool IsDuplicate { get; }

        public FileUploadResultDto(Guid id, string hash, DateTime uploadTime, string name, bool isDuplicate)
        {
            Id = id;
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            UploadTime = uploadTime;
            Name = name;
            IsDuplicate = isDuplicate;
        }
    }
}
