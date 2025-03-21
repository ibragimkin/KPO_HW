using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Domain.Factories;
using Domain.Services;
using Infrastructure;
using Domain.Entities;
using System;

public static class DependencyInjection
{
    public static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Регистрируем общую фабрику для создания доменных сущностей
        serviceCollection.AddSingleton<CategoryFactory>();

        // Регистрируем сервисы (фасады)
        serviceCollection.AddSingleton<BankAccountService>();
        serviceCollection.AddSingleton<CategoryService>();
        serviceCollection.AddSingleton<OperationService>();
        serviceCollection.AddSingleton<AnalyticsService>();

        // Регистрируем репозитории
        serviceCollection.AddSingleton<IRepository<BankAccount>, BankAccountRepository>();
        serviceCollection.AddSingleton<IRepository<Category>, CategoryRepository>(); 
        serviceCollection.AddSingleton<IRepository<Operation>, OperationRepository>();

        return serviceCollection.BuildServiceProvider();
    }
}
