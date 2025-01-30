FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solo los archivos .csproj primero
COPY ["src/TaskManager.Core/TaskManager.Core.csproj", "src/TaskManager.Core/"]
COPY ["src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj", "src/TaskManager.Infrastructure/"]
COPY ["src/TaskManager.WebApi/TaskManager.WebApi.csproj", "src/TaskManager.WebApi/"]

# Restaurar dependencias
RUN dotnet restore "src/TaskManager.WebApi/TaskManager.WebApi.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar
RUN dotnet publish "src/TaskManager.WebApi/TaskManager.WebApi.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TaskManager.WebApi.dll"]
