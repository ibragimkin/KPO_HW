// File: FileAnalysisService.Infrastructure/Configuration/InfrastructureModule.cs
using System;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Infrastructure.Persistence;
using FileAnalysisService.Infrastructure.Repositories;
using FileAnalysisService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysisService.Infrastructure.Configuration
{
    /// <summary>
    /// Модуль для регистрации инфраструктурных сервисов и DbContext в DI-контейнер.
    /// </summary>
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection не настроен.");

            services.AddDbContext<AnalysisContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(typeof(AnalysisContext).Assembly.FullName);
                    }));

            services.AddScoped<IAnalysisRepository, AnalysisRepository>();

            var storageBaseUrl = configuration.GetValue<string>("FileStoringService:BaseUrl");
            if (string.IsNullOrWhiteSpace(storageBaseUrl))
                throw new InvalidOperationException("Configuration: FileStoringService:BaseUrl не настроен.");

            services.AddHttpClient<IFileRetrievalService, FileRetrievalService>(client =>
            {
                client.BaseAddress = new Uri(storageBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/octet-stream");
            });

            return services;
        }
    }
}
