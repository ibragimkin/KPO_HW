// File: FileAnalysisService.Application/Interfaces/IAnalysisRepository.cs
using System;
using System.Threading.Tasks;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Interfaces
{
    /// <summary>
    /// Репозиторий для хранения и поиска результатов анализа.
    /// </summary>
    public interface IAnalysisRepository
    {
        /// <summary>
        /// Возвращает результат анализа по идентификатору файла.
        /// Если результат отсутствует, возвращает null.
        /// </summary>
        /// <param name="id">GUID файла (идентификатор из FileStoringService).</param>
        /// <returns>Сущность FileAnalysisResult или null, если не найдено.</returns>
        Task<FileAnalysisResult?> GetByIdAsync(Guid id);

        /// <summary>
        /// Сохраняет результат анализа в базу данных.
        /// </summary>
        /// <param name="result">Сущность результата анализа.</param>
        /// <returns>Сохранённая сущность (с тем же Id).</returns>
        Task<FileAnalysisResult> SaveAsync(FileAnalysisResult result);
    }
}
