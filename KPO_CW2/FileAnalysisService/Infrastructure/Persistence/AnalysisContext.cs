using FileAnalysisService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FileAnalysisService.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core DbContext для хранения результатов анализа файлов.
    /// </summary>
    public class AnalysisContext : DbContext
    {
        public DbSet<FileAnalysisResult> FileAnalysisResults { get; set; }

        public AnalysisContext(DbContextOptions<AnalysisContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FileAnalysisResult>();

            // Таблица и ключ
            entity.ToTable("FileAnalysisResults");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                  .ValueGeneratedNever();

            // Поля статистики
            entity.Property(r => r.WordCount)
                  .IsRequired()
                  .HasMaxLength(50); 
            entity.Property(r => r.ParagraphCount)
                  .IsRequired()
                  .HasMaxLength(50);
            entity.Property(r => r.CharCount)
                  .IsRequired()
                  .HasMaxLength(50);

            // Время загрузки
            entity.Property(r => r.UploadTime)
                  .IsRequired();

            // Хеш файла
            entity.Property(r => r.Hash)
                  .IsRequired()
                  .HasMaxLength(128); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
