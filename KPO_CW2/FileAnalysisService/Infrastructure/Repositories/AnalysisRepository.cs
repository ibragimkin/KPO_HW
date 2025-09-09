// File: FileAnalysisService.Infrastructure/Repositories/AnalysisRepository.cs
using System;
using System.Threading.Tasks;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для хранения и получения результатов анализа файлов в PostgreSQL.
    /// </summary>
    public class AnalysisRepository : IAnalysisRepository
    {
        private readonly AnalysisContext _context;

        public AnalysisRepository(AnalysisContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Сохраняет результат анализа в базе данных.
        /// Если запись с таким Id уже существует, выбрасывает исключение.
        /// </summary>
        /// <param name="result">Сущность результата анализа.</param>
        /// <returns>Сохранённая сущность FileAnalysisResult.</returns>
        /// <exception cref="AnalysisException">Если произошла ошибка при сохранении.</exception>
        public async Task<FileAnalysisResult> SaveAsync(FileAnalysisResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var existing = await _context.FileAnalysisResults
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(r => r.Id == result.Id);
            if (existing != null)
            {
                throw new AnalysisException($"Результат анализа для файла с Id '{result.Id}' уже сохранён.");
            }

            try
            {
                _context.FileAnalysisResults.Add(result);
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new AnalysisException("Ошибка при сохранении результата анализа в базу данных.", ex);
            }
        }

        /// <summary>
        /// Возвращает результат анализа по идентификатору файла.
        /// Если не найден, возвращает null.
        /// </summary>
        /// <param name="id">GUID файла.</param>
        /// <returns>Сущность FileAnalysisResult или null.</returns>
        /// <exception cref="AnalysisException">Если произошла ошибка при чтении из БД.</exception>
        public async Task<FileAnalysisResult?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AnalysisException("Id не может быть пустым.");

            try
            {
                return await _context.FileAnalysisResults
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new AnalysisException($"Ошибка при получении результата анализа для файла с Id '{id}'.", ex);
            }
        }
    }
}
