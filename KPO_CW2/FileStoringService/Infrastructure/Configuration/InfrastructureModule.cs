using System;
using FileStoringService.Application.Interfaces;
using FileStoringService.Infrastructure.Persistence;
using FileStoringService.Infrastructure.Repositories;
using FileStoringService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.Infrastructure.Configuration
{
    /// <summary>
    /// Модуль для регистрации инфраструктурных сервисов и DbContext в DI-контейнер.
    /// </summary>
    public static class InfrastructureModule
    {
        /// <summary>
        /// Регистрирует все необходимые сервисы инфраструктурного слоя:
        /// - DbContext для PostgreSQL
        /// - Реализации IFileRepository и IHashService
        /// </summary>
        /// <param name="services">Сборка сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Читаем connection string из конфигурации (appsettings.json):
            // "ConnectionStrings": { "DefaultConnection": "<postgres-connection-string>" }
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection не настроен.");

            // Регистрируем DbContext для PostgreSQL
            services.AddDbContext<FileMetadataContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(typeof(FileMetadataContext).Assembly.FullName);
                    });
            });

            // Регистрируем репозиторий для работы с файлами и их метаданными
            services.AddScoped<IFileRepository, FileRepository>();

            // Регистрируем сервис для вычисления хеша
            services.AddSingleton<IHashService, HashService>();

            return services;
        }
    }
}
