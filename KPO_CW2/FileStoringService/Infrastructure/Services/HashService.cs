using System;
using System.IO;
using System.Security.Cryptography;
using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.ValueObjects;

namespace FileStoringService.Infrastructure.Services
{
    /// <summary>
    /// Реализация сервиса вычисления хеша файлов с использованием SHA-256.
    /// </summary>
    public class HashService : IHashService
    {
        public FileHash ComputeHash(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            // Сохраняем текущую позицию, чтобы не нарушать внешний поток
            long originalPosition = 0;
            if (content.CanSeek)
            {
                originalPosition = content.Position;
                content.Position = 0;
            }

            // Вычисляем SHA-256 хеш
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(content);

            // Восстанавливаем позицию потока
            if (content.CanSeek)
                content.Position = originalPosition;

            // Преобразуем в hex-строку
            string hex = BitConverter
                .ToString(hashBytes)
                .Replace("-", "")
                .ToLowerInvariant();

            return FileHash.FromString(hex);
        }
    }
}
