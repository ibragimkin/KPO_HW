using System;

namespace FileAnalysisService.Domain.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибках получения или анализа файла.
    /// </summary>
    public sealed class AnalysisException : Exception
    {
        /// <summary>
        /// Создаёт новое исключение AnalysisException с сообщением.
        /// </summary>
        /// <param name="message">Описание ошибки.</param>
        public AnalysisException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Создаёт новое исключение AnalysisException с сообщением и внутренним исключением.
        /// </summary>
        /// <param name="message">Описание ошибки.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public AnalysisException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
