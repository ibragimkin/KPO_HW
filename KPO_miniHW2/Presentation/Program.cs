using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZooManagementSystem.Application.Services;
using ZooManagementSystem.Domain.Events;
using ZooManagementSystem.Infrastructure.Events;
using ZooManagementSystem.Infrastructure.Events.EventHandlers;
using ZooManagementSystem.Infrastructure.Persistence;
using ZooManagementSystem.Domain.Interfaces;

namespace ZooManagementSystem.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Infrastructure: in-memory repositories
            services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
            services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
            services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

            // Infrastructure: event publisher and handlers
            services.AddSingleton<EventPublisher>();
            services.AddSingleton<AnimalMovedHandler>();
            services.AddSingleton<FeedingTimeHandler>();

            // Application services
            services.AddScoped<AnimalTransferService>();
            services.AddScoped<FeedingOrganizationService>();
            services.AddScoped<ZooStatisticsService>();

            // Presentation
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Register domain event handlers
            var publisher = app.ApplicationServices.GetRequiredService<EventPublisher>();
            var movedHandler = app.ApplicationServices.GetRequiredService<AnimalMovedHandler>();
            var feedingHandler = app.ApplicationServices.GetRequiredService<FeedingTimeHandler>();

            publisher.Register<AnimalMovedEvent>(movedHandler.HandleAsync);
            publisher.Register<FeedingTimeEvent>(feedingHandler.HandleAsync);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
