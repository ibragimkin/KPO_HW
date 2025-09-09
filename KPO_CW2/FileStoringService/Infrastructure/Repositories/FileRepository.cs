using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.ValueObjects;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Infrastructure.Persistence;

namespace FileStoringService.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для хранения файлов и их метаданных в PostgreSQL + локальной файловой системе.
    /// </summary>
    public class FileRepository : IFileRepository
    {
        private readonly FileMetadataContext _context;
        private readonly string _basePath;

        /// <summary>
        /// Конструктор репозитория.
        /// Ожидает, что в конфигурации (appsettings.json) указано:
        /// "FileStorage": { "BasePath": "<путь_до_папки>" }
        /// </summary>
        public FileRepository(FileMetadataContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _basePath = configuration.GetValue<string>("FileStorage:BasePath");
            if (string.IsNullOrWhiteSpace(_basePath))
                throw new ArgumentException("Конфигурация FileStorage:BasePath не указана или пуста.");

            // Создаем директорию при первом вызове, если не существует
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        /// <summary>
        /// Сохраняет файл на диск и сохраняет его метаданные в базу данных.
        /// </summary>
        /// <param name="fileId">Идентификатор нового файла.</param>
        /// <param name="content">Поток с содержимым файла. Должен быть позиционируемым.</param>
        /// <param name="hash">Предварительно вычисленный хеш (SHA-256).</param>
        /// <param name="uploadTime">Время загрузки (UTC).</param>
        /// <returns>Объект FileMetadata, сохранённый в базе.</returns>
        public async Task<FileMetadata> SaveAsync(Guid fileId, Stream content, FileHash hash, DateTime uploadTime, string name)
        {
            if (fileId == Guid.Empty)
                throw new ArgumentException("fileId не может быть пустым.", nameof(fileId));
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            ArgumentNullException.ThrowIfNull(hash);

            // Формируем имя файла: <fileId>.txt
            string fileName = name;
            string fullPath = Path.Combine(_basePath, fileName);

            // Сбрасываем поток на начало перед записью
            if (content.CanSeek)
                content.Position = 0;

            // Записываем файл на диск (FileMode.CreateNew, чтобы упасть, если файл уже есть)
            using (var fileStream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await content.CopyToAsync(fileStream);
            }

            // Создаём сущность метаданных
            var metadata = new FileMetadata(
                id: fileId,
                hash: hash,
                uploadTime: uploadTime,
                filePath: fullPath,
                name: name
            );

            // Сохраняем метаданные в БД
            _context.FileMetadatas.Add(metadata);
            await _context.SaveChangesAsync();

            return metadata;
        }

        /// <summary>
        /// Возвращает поток для чтения содержимого файла по его идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Поток (FileStream) для чтения файла.</returns>
        /// <exception cref="FileNotFoundException">Если файл не найден в БД или на диске.</exception>
        public async Task<Stream> GetContentAsync(Guid fileId)
        {
            if (fileId == Guid.Empty)
                throw new ArgumentException("fileId не может быть пустым.", nameof(fileId));

            // Получаем метаданные из БД
            var metadata = await _context.FileMetadatas.FindAsync(fileId);
            if (metadata == null)
                throw new Domain.Exceptions.FileNotFoundException(fileId);

            // Проверяем, что файл действительно существует в файловой системе
            if (!System.IO.File.Exists(metadata.FilePath))
                throw new Domain.Exceptions.FileNotFoundException(fileId);

            // Открываем поток на чтение и возвращаем
            var stream = new FileStream(metadata.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream;
        }

        /// <summary>
        /// Возвращает метаданные файла по идентификатору.
        /// </summary>
        /// <param name="fileId">Идентификатор файла.</param>
        /// <returns>Сущность FileMetadata.</returns>
        /// <exception cref="FileNotFoundException">Если метаданные с таким Id отсутствуют.</exception>
        public async Task<FileMetadata> GetMetadataAsync(Guid fileId)
        {
            if (fileId == Guid.Empty)
                throw new ArgumentException("fileId не может быть пустым.", nameof(fileId));

            var metadata = await _context.FileMetadatas.FindAsync(fileId);
            if (metadata == null)
                throw new Domain.Exceptions.FileNotFoundException(fileId);

            return metadata;
        }

        /// <summary>
        /// Ищет в базе метаданные по хешу файла (для обнаружения дубликатов).
        /// </summary>
        /// <param name="hash">Value Object FileHash.</param>
        /// <returns>Сущность FileMetadata, если найдена, или null, если не найдено.</returns>
        public async Task<FileMetadata?> GetByHashAsync(FileHash hash)
        {
            ArgumentNullException.ThrowIfNull(hash);
            return await _context.FileMetadatas
                                 .SingleOrDefaultAsync(f => f.Hash == hash);
        }
    }
}
