# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проект и восстанавливаем зависимости
COPY APIGateway/APIGateway.csproj APIGateway/
RUN dotnet restore APIGateway/APIGateway.csproj

# Копируем всё и публикуем
COPY . .
WORKDIR /src/APIGateway
RUN dotnet publish -c Release -o /app/publish

# Стадия рантайма
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5002
ENTRYPOINT ["dotnet", "APIGateway.dll"]
