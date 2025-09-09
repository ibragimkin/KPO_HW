using FileStoringService.Domain.Entities;
using FileStoringService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FileStoringService.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core DbContext для хранения метаданных файлов.
    /// </summary>
    public class FileMetadataContext : DbContext
    {
        public DbSet<FileMetadata>? FileMetadatas { get; set; }

        public FileMetadataContext(DbContextOptions<FileMetadataContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<FileMetadata>();

            entity.ToTable("FileMetadatas");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id)
                  .ValueGeneratedNever(); // Мы генерируем GUID в коде

            entity.Property(f => f.FilePath)
                  .IsRequired();

            entity.Property(f => f.UploadTime)
                  .IsRequired();
            entity.Property(f => f.Name).IsRequired();
            var hashConverter = new ValueConverter<FileHash, string>(
                v => v.Value,
                v => FileHash.FromString(v));

            entity.Property(f => f.Hash)
                  .HasConversion(hashConverter)
                  .HasColumnName("Hash")
                  .IsRequired();
        }
    }
}
