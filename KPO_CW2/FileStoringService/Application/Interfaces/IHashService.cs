using System.IO;
using FileStoringService.Domain.ValueObjects;

namespace FileStoringService.Application.Interfaces
{
    /// <summary>
    /// Сервис для вычисления хеша содержимого файла.
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        /// Вычисляет и возвращает объект-значение хеша на основе переданного потока.
        /// </summary>
        /// <param name="content">Поток с данными файла.</param>
        /// <returns>Объект FileHash с шестнадцатеричным представлением хеша.</returns>
        FileHash ComputeHash(Stream content);
    }
}
