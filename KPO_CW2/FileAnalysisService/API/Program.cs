// File: FileAnalysisService.API/Program.cs
using FileAnalysisService.Application.UseCases;
using FileAnalysisService.Infrastructure.Configuration;
using FileAnalysisService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Чтение конфигурации (appsettings.json, переменные окружения)
IConfiguration configuration = builder.Configuration;

// 1. Регистрируем DbContext для PostgreSQL
builder.Services.AddDbContext<AnalysisContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(AnalysisContext).Assembly.FullName);
        }
    )
);

// 2. Регистрируем инфраструктурные сервисы (IAnalysisRepository, IFileRetrievalService)
builder.Services.AddInfrastructureServices(configuration);

// 3. Регистрируем UseCase
builder.Services.AddScoped<AnalyzeFileUseCase>();

// 4. Добавляем контроллеры
builder.Services.AddControllers();

// 5. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Применяем миграции при старте (для разработки)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AnalysisContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
