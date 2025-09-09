using System;

namespace FileAnalysisService.Domain.Entities
{
    /// <summary>
    /// Сущность, описывающая результат анализа текстового файла.
    /// </summary>
    public class FileAnalysisResult
    {
        /// <summary>
        /// Идентификатор файла (GUID), совпадает с Id из FileStoringService.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Количество слов, посчитанное в файле (строка, например: "123").
        /// </summary>
        public string WordCount { get; }

        /// <summary>
        /// Количество абзацев, посчитанное в файле (строка, например: "10").
        /// </summary>
        public string ParagraphCount { get; }

        /// <summary>
        /// Количество символов, посчитанное в файле (строка, например: "2048").
        /// </summary>
        public string CharCount { get; }

        /// <summary>
        /// UTC-время, когда файл был загружен в FileStoringService.
        /// </summary>
        public DateTime UploadTime { get; }

        /// <summary>
        /// SHA-256 хеш файла, получаемый из FileStoringService (строкой).
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// Конструктор для создания экземпляра результата анализа.
        /// </summary>
        /// <param name="id">GUID файла (должен быть ненулевым).</param>
        /// <param name="wordCount">Количество слов в виде строки.</param>
        /// <param name="paragraphCount">Количество абзацев в виде строки.</param>
        /// <param name="charCount">Количество символов в виде строки.</param>
        /// <param name="uploadTime">UTC-время загрузки (берётся из FileStoringService).</param>
        /// <param name="hash">SHA-256 хеш (строковое представление).</param>
        public FileAnalysisResult(
            Guid id,
            string wordCount,
            string paragraphCount,
            string charCount,
            DateTime uploadTime,
            string hash)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id не может быть пустым.", nameof(id));

            WordCount = !string.IsNullOrWhiteSpace(wordCount)
                ? wordCount
                : throw new ArgumentNullException(nameof(wordCount), "WordCount не может быть пустым.");

            ParagraphCount = !string.IsNullOrWhiteSpace(paragraphCount)
                ? paragraphCount
                : throw new ArgumentNullException(nameof(paragraphCount), "ParagraphCount не может быть пустым.");

            CharCount = !string.IsNullOrWhiteSpace(charCount)
                ? charCount
                : throw new ArgumentNullException(nameof(charCount), "CharCount не может быть пустым.");

            UploadTime = uploadTime;

            Hash = !string.IsNullOrWhiteSpace(hash)
                ? hash
                : throw new ArgumentNullException(nameof(hash), "Hash не может быть пустым.");

            Id = id;
        }
    }
}
