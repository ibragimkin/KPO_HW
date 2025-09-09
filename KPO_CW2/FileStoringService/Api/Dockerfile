# 1. Используем официальный SDK-образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Установим рабочую директорию
WORKDIR /src

# Копируем sln и proj файлы
COPY FileStoringService.sln .
COPY Api/Api.csproj Api/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Восстановление зависимостей (чтобы использовать кеш слоев докера)
RUN dotnet restore FileStoringService.sln

# Копируем все остальные исходники
COPY Api/ Api/
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/

# Публикуем приложение
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

# 2. Используем более легкий рантайм образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Установим рабочую директорию
WORKDIR /app

# Копируем опубликованные файлы из build stage
COPY --from=build /app/publish .

# Открываем порт (необязательно, но хорошо для читаемости)
EXPOSE 80
EXPOSE 443
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
# Запускаем приложение
ENTRYPOINT ["dotnet", "Api.dll"]
