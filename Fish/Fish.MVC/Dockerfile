FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіювання всіх проектів
COPY ["Fish.MVC/Fish.MVC.csproj", "Fish.MVC/"]
COPY ["Fish.Infrastructure/Fish.Infrastructure.csproj", "Fish.Infrastructure/"]
COPY ["Fish.Common/Fish.Common.csproj", "Fish.Common/"]

# Відновлення залежностей
RUN dotnet restore "Fish.MVC/Fish.MVC.csproj"

# Копіювання всього коду
COPY . .

# Збірка проекту
WORKDIR "/src/Fish.MVC"
RUN dotnet build "Fish.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fish.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fish.MVC.dll"]

