// File: FileAnalysisService.Application/UseCases/AnalyzeFileUseCase.cs
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAnalysisService.Application.DTOs;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Domain.Exceptions;

namespace FileAnalysisService.Application.UseCases
{
    /// <summary>
    /// Сценарий анализа текстового файла по его идентификатору.
    /// Если результат уже есть в базе, возвращает его.
    /// Иначе получает файл из FileStoringService, выполняет анализ,
    /// сохраняет результат и возвращает DTO.
    /// </summary>
    public class AnalyzeFileUseCase
    {
        private readonly IFileRetrievalService _fileRetrievalService;
        private readonly IAnalysisRepository _analysisRepository;

        public AnalyzeFileUseCase(
            IFileRetrievalService fileRetrievalService,
            IAnalysisRepository analysisRepository)
        {
            _fileRetrievalService = fileRetrievalService
                ?? throw new ArgumentNullException(nameof(fileRetrievalService));
            _analysisRepository = analysisRepository
                ?? throw new ArgumentNullException(nameof(analysisRepository));
        }

        /// <summary>
        /// Выполняет анализ файла по его GUID.
        /// </summary>
        /// <param name="fileId">Идентификатор файла (GUID).</param>
        /// <returns>DTO с результатами анализа.</returns>
        /// <exception cref="AnalysisException">
        /// Выбрасывается, если файл не найден, не удалось считать его, 
        /// или произошла ошибка при сохранении результата.
        /// </exception>
        public async Task<FileAnalysisResultDto> ExecuteAsync(Guid fileId)
        {
            if (fileId == Guid.Empty)
                throw new AnalysisException("fileId не может быть пустым.");

            // 1. Проверяем, есть ли результат анализа в базе
            var existing = await _analysisRepository.GetByIdAsync(fileId);
            if (existing != null)
            {
                // Возвращаем DTO, строя из сущности
                return new FileAnalysisResultDto(
                    id: existing.Id,
                    wordCount: existing.WordCount,
                    paragraphCount: existing.ParagraphCount,
                    charCount: existing.CharCount,
                    uploadTime: existing.UploadTime.ToString("o"),
                    hash: existing.Hash
                );
            }

            // 2. Получаем файл и метаданные (хеш + время загрузки) из FileStoringService
            Stream contentStream;
            string hash;
            DateTime uploadTime;
            try
            {
                (contentStream, hash, uploadTime) = await _fileRetrievalService.GetFileAsync(fileId);
            }
            catch (Exception ex)
            {
                throw new AnalysisException($"Не удалось получить файл с Id '{fileId}'.\n{ex.Message}", ex);
            }

            // 3. Считываем весь текст из потока
            string text;
            try
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8, true, 1024, leaveOpen: false))
                {
                    text = await reader.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                throw new AnalysisException("Ошибка при чтении содержимого файла.", ex);
            }

            // 4. Подсчитываем статистику: абзацы, слова, символы
            string paragraphCountStr = CountParagraphs(text).ToString();
            string wordCountStr = CountWords(text).ToString();
            string charCountStr = CountCharacters(text).ToString();

            // 5. Создаём сущность результата анализа
            var analysisResult = new FileAnalysisResult(
                id: fileId,
                wordCount: wordCountStr,
                paragraphCount: paragraphCountStr,
                charCount: charCountStr,
                uploadTime: uploadTime,
                hash: hash
            );

            // 6. Сохраняем результат в базе
            try
            {
                await _analysisRepository.SaveAsync(analysisResult);
            }
            catch (Exception ex)
            {
                throw new AnalysisException("Не удалось сохранить результат анализа.", ex);
            }

            // 7. Возвращаем DTO
            return new FileAnalysisResultDto(
                id: analysisResult.Id,
                wordCount: analysisResult.WordCount,
                paragraphCount: analysisResult.ParagraphCount,
                charCount: analysisResult.CharCount,
                uploadTime: analysisResult.UploadTime.ToString("o"),
                hash: analysisResult.Hash
            );
        }

        /// <summary>
        /// Подсчитывает количество слов в тексте (разделитель – любые пробельные символы).
        /// </summary>
        private static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var words = text
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        /// <summary>
        /// Подсчитывает количество символов в тексте (включая пробелы и знаки препинания).
        /// </summary>
        private static int CountCharacters(string text)
        {
            return text?.Length ?? 0;
        }

        /// <summary>
        /// Подсчитывает количество абзацев:  
        /// абзацем считается блок текста, разделённый двумя последовательными символами новой строки ("\n\n").
        /// </summary>
        private static int CountParagraphs(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            string normalized = text.Replace("\r\n", "\n").Trim();

            var parts = normalized
                .Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            return parts.Length;
        }
    }
}
