# 1. Используем официальный SDK-образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Установим рабочую директорию
WORKDIR /src

# Копируем sln и proj файлы
COPY FileAnalysisService.sln .
COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Восстановление зависимостей (чтобы использовать кеш слоев докера)
RUN dotnet restore FileAnalysisService.sln

# Копируем все остальные исходники
COPY API/ API/
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/

# Публикуем приложение
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# 2. Используем более легкий рантайм образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Установим рабочую директорию
WORKDIR /app

# Копируем опубликованные файлы из build stage
COPY --from=build /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001
# Запускаем приложение
ENTRYPOINT ["dotnet", "API.dll"]
