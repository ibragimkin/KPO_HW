// File: FileAnalysisService.Application/DTOs/FileAnalysisResultDto.cs
using System;

namespace FileAnalysisService.Application.DTOs
{
    /// <summary>
    /// DTO, возвращаемый после анализа файла.
    /// </summary>
    public class FileAnalysisResultDto
    {
        /// <summary>
        /// Идентификатор файла (GUID).
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Количество слов в формате строки.
        /// </summary>
        public string WordCount { get; }

        /// <summary>
        /// Количество абзацев в формате строки.
        /// </summary>
        public string ParagraphCount { get; }

        /// <summary>
        /// Количество символов в формате строки.
        /// </summary>
        public string CharCount { get; }

        /// <summary>
        /// UTC-время загрузки файла (в формате ISO 8601, строка).
        /// </summary>
        public string UploadTime { get; }

        /// <summary>
        /// SHA-256 хеш файла (hex-строка).
        /// </summary>
        public string Hash { get; }

        public FileAnalysisResultDto(
            Guid id,
            string wordCount,
            string paragraphCount,
            string charCount,
            string uploadTime,
            string hash)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id не может быть пустым.", nameof(id));

            Id = id;
            WordCount = !string.IsNullOrWhiteSpace(wordCount)
                ? wordCount
                : throw new ArgumentNullException(nameof(wordCount), "WordCount не может быть пустым.");

            ParagraphCount = !string.IsNullOrWhiteSpace(paragraphCount)
                ? paragraphCount
                : throw new ArgumentNullException(nameof(paragraphCount), "ParagraphCount не может быть пустым.");

            CharCount = !string.IsNullOrWhiteSpace(charCount)
                ? charCount
                : throw new ArgumentNullException(nameof(charCount), "CharCount не может быть пустым.");

            UploadTime = !string.IsNullOrWhiteSpace(uploadTime)
                ? uploadTime
                : throw new ArgumentNullException(nameof(uploadTime), "UploadTime не может быть пустым.");

            Hash = !string.IsNullOrWhiteSpace(hash)
                ? hash
                : throw new ArgumentNullException(nameof(hash), "Hash не может быть пустым.");
        }
    }
}
