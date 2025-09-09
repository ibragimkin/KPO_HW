// File: FileStoringService.API/Program.cs
using FileStoringService.Application.UseCases;
using FileStoringService.Infrastructure.Configuration;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

// Конфигурация
IConfiguration configuration = builder.Configuration;

// DbContext для PostgreSQL
builder.Services.AddDbContext<FileMetadataContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(FileMetadataContext).Assembly.FullName);
        })
);

// Инфраструктурные сервисы
builder.Services.AddInfrastructureServices(configuration);

// Application слой
builder.Services.AddScoped<UploadFileUseCase>();
builder.Services.AddScoped<GetFileUseCase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Автомиграция
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FileMetadataContext>();

    const int maxRetries = 5;
    const int delaySeconds = 5;
    int retries = 0;

    while (true)
    {
        try
        {
            db.Database.Migrate();
            break; // Успех — выходим из цикла
        }
        catch (Exception ex)
        {
            retries++;
            if (retries >= maxRetries)
            {
                throw; // После 5 попыток кидаем ошибку дальше
            }

            Console.WriteLine($"Database migration failed. Retrying in {delaySeconds} seconds... ({retries}/{maxRetries})");
            Thread.Sleep(delaySeconds * 1000); // Подождать перед новой попыткой
        }
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.Run();