# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["src/Services/Identity/GRC.Identity.API/GRC.Identity.API.csproj", "Services/Identity/GRC.Identity.API/"]
COPY ["src/Services/Identity/GRC.Identity.Application/GRC.Identity.Application.csproj", "Services/Identity/GRC.Identity.Application/"]
COPY ["src/Services/Identity/GRC.Identity.Domain/GRC.Identity.Domain.csproj", "Services/Identity/GRC.Identity.Domain/"]
COPY ["src/Services/Identity/GRC.Identity.Infrastructure/GRC.Identity.Infrastructure.csproj", "Services/Identity/GRC.Identity.Infrastructure/"]
COPY ["src/BuildingBlocks/GRC.BuildingBlocks.Domain/GRC.BuildingBlocks.Domain.csproj", "BuildingBlocks/GRC.BuildingBlocks.Domain/"]
COPY ["src/BuildingBlocks/GRC.BuildingBlocks.Infrastructure/GRC.BuildingBlocks.Infrastructure.csproj", "BuildingBlocks/GRC.BuildingBlocks.Infrastructure/"]
COPY ["src/BuildingBlocks/GRC.BuildingBlocks.EventBus/GRC.BuildingBlocks.EventBus.csproj", "BuildingBlocks/GRC.BuildingBlocks.EventBus/"]
COPY ["src/BuildingBlocks/GRC.BuildingBlocks.IntegrationEvents/GRC.BuildingBlocks.IntegrationEvents.csproj", "BuildingBlocks/GRC.BuildingBlocks.IntegrationEvents/"]

# Restaurar dependencias
RUN dotnet restore "Services/Identity/GRC.Identity.API/GRC.Identity.API.csproj"

# Copiar el resto de los archivos
COPY src/ .

# Compilar
WORKDIR "/src/Services/Identity/GRC.Identity.API"
RUN dotnet build "GRC.Identity.API.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "GRC.Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GRC.Identity.API.dll"]