using System;

namespace FileStoringService.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое когда запрашиваемый файл не найден в хранилище.
    /// </summary>
    public sealed class FileNotFoundException : Exception
    {
        /// <summary>
        /// Идентификатор файла, который не удалось найти.
        /// </summary>
        public Guid FileId { get; }

        /// <summary>
        /// Создаёт новое исключение FileNotFoundException для заданного ID.
        /// </summary>
        /// <param name="fileId">Идентификатор отсутствующего файла.</param>
        public FileNotFoundException(Guid fileId)
            : base($"Файл с идентификатором '{fileId}' не найден.")
        {
            FileId = fileId;
        }

        /// <summary>
        /// Создаёт новое исключение FileNotFoundException для заданного ID и внутреннего исключения.
        /// </summary>
        /// <param name="fileId">Идентификатор отсутствующего файла.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public FileNotFoundException(Guid fileId, Exception innerException)
            : base($"Файл с идентификатором '{fileId}' не найден.", innerException)
        {
            FileId = fileId;
        }
    }
}
