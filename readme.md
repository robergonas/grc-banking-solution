# GRC Banking Solution

Sistema de Gobernanza, Riesgo y Cumplimiento (GRC) para entidades bancarias.

## 🏗️ Arquitectura

- **Estilo**: Microservicios con Domain-Driven Design (DDD)
- **Backend**: .NET 8.0, C#, Entity Framework Core
- **Frontend**: Angular 17
- **Base de Datos**: SQL Server 2022
- **Message Broker**: RabbitMQ
- **Contenedores**: Docker & Docker Compose

## 📦 Microservicios

1. **Identity API** - Autenticación y autorización
2. **Governance API** - Políticas y estructura de gobernanza
3. **Risk API** - Gestión de riesgos
4. **Compliance API** - Cumplimiento regulatorio

## 🚀 Inicio Rápido

### Prerrequisitos

- .NET 8.0 SDK
- Docker Desktop
- SQL Server 2022
- Node.js 20.x
- Visual Studio 2022 o VS Code

### Clonar y ejecutar

```bash
# Clonar repositorio
git clone 

# Navegar al directorio
cd GRC.BankingSolution

# Restaurar dependencias
dotnet restore

# Ejecutar con Docker
docker-compose up -d
```

## 📚 Documentación

La documentación completa está en la carpeta `/docs`.

## 👥 Equipo de Desarrollo

- Arquitecto de Software: Róber Goñas Portocarrero

## 📄 Licencia