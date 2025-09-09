using System;
using System.IO;
using System.Threading.Tasks;
using FileStoringService.Application.DTOs;
using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.ValueObjects;

namespace FileStoringService.Application.UseCases
{
    /// <summary>
    /// Сценарий (use case) загрузки файла: 
    /// вычисление хеша, проверка на дубликат и сохранение метаданных и содержимого.
    /// </summary>
    public class UploadFileUseCase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IHashService _hashService;

        public UploadFileUseCase(IFileRepository fileRepository, IHashService hashService)
        {
            _fileRepository = fileRepository
                ?? throw new ArgumentNullException(nameof(fileRepository));
            _hashService = hashService
                ?? throw new ArgumentNullException(nameof(hashService));
        }

        /// <summary>
        /// Выполняет загрузку файла.
        /// </summary>
        /// <param name="content">
        /// Поток с данными файла. Должен поддерживать Seek (для пересчёта хеша и сохранения).
        /// </param>
        /// <returns>DTO с результатами загрузки.</returns>
        public async Task<FileUploadResultDto> ExecuteAsync(Stream content, string name)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            // Вычисляем хеш (читая поток до конца)
            var hash = _hashService.ComputeHash(content);

            // Ищем существующий файл по хешу
            var existing = await _fileRepository.GetByHashAsync(hash);
            if (existing != null)
            {
                // Если найден дубликат, возвращаем его метаданные
                return new FileUploadResultDto(
                    existing.Id,
                    existing.Hash.Value,
                    existing.UploadTime,
                    existing.Name,
                    isDuplicate: true);
            }

            // Сбросить позицию в потоке для записи
            if (content.CanSeek)
                content.Position = 0;

            // Сохраняем новый файл
            var newId = Guid.NewGuid();
            var uploadTime = DateTime.UtcNow; // либо DateTime.Now, в зависимости от требований
            FileMetadata metadata = await _fileRepository.SaveAsync(newId, content, hash, uploadTime, name);

            return new FileUploadResultDto(
                metadata.Id,
                metadata.Hash.Value,
                metadata.UploadTime,
                metadata.Name,
                isDuplicate: false);
        }
    }
}
