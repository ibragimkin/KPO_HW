// File: FileStoringService.Application/UseCases/GetFileUseCase.cs
using System;
using System.Threading.Tasks;
using FileStoringService.Application.DTOs;
using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Exceptions;

namespace FileStoringService.Application.UseCases
{
    /// <summary>
    /// Сценарий (use case) получения файла по его идентификатору:
    /// извлечение метаданных и содержимого.
    /// </summary>
    public class GetFileUseCase
    {
        private readonly IFileRepository _fileRepository;

        public GetFileUseCase(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository
                ?? throw new ArgumentNullException(nameof(fileRepository));
        }

        /// <summary>
        /// Выполняет получение файла.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>DTO с метаданными и потоком содержимого.</returns>
        /// <exception cref="FileNotFoundException">
        /// Если файл с таким Id не найден.
        /// </exception>
        public async Task<FileDownloadResultDto> ExecuteAsync(Guid fileId)
        {
            if (fileId == Guid.Empty)
                throw new ArgumentException("fileId не может быть пустым.", nameof(fileId));

            // Получаем метаданные — может выбросить FileNotFoundException
            var metadata = await _fileRepository.GetMetadataAsync(fileId);

            // Получаем содержимое файла
            var contentStream = await _fileRepository.GetContentAsync(fileId);

            return new FileDownloadResultDto(
                metadata.Id,
                metadata.Hash.Value,
                metadata.UploadTime,
                metadata.Name,
                contentStream);
        }
    }
}
