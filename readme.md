# GRC Banking Solution

Sistema integral de Gobernanza, Riesgo y Cumplimiento (GRC) para instituciones financieras, construido con arquitectura de microservicios y Domain-Driven Design (DDD).

## 🏗️ Arquitectura

- **Patrón**: Microservicios con DDD
- **Framework**: .NET 8.0
- **Base de Datos**: SQL Server
- **Event Bus**: RabbitMQ (planificado)
- **API Gateway**: Ocelot (planificado)

## 📦 Módulos Implementados

### ✅ Identity & Access Management (IAM)
Sistema completo de autenticación y autorización con gestión de roles y permisos.

**Características:**
- ✅ Registro y autenticación de usuarios (JWT)
- ✅ Gestión de usuarios (CRUD completo)
- ✅ Sistema de roles jerárquicos
- ✅ Sistema de permisos granulares basado en recursos y acciones
- ✅ Asignación dinámica de permisos a roles
- ✅ Bloqueo automático por intentos fallidos
- ✅ Auditoría de accesos

**Endpoints Principales:**
```
POST   /api/Auth/register          # Registro de usuarios
POST   /api/Auth/login             # Autenticación
POST   /api/Auth/change-password   # Cambio de contraseña
GET    /api/Auth/verify-token      # Verificar token
GET    /api/Users                  # Listar usuarios
GET    /api/Users/{id}             # Obtener usuario
PUT    /api/Users/{id}             # Actualizar usuario
POST   /api/Users/{id}/deactivate  # Desactivar usuario
PUT    /api/Users/{id}/roles       # Cambiar roles
GET    /api/Roles                  # Listar roles
POST   /api/Roles                  # Crear rol
PUT    /api/Roles/{id}             # Actualizar rol
DELETE /api/Roles/{id}             # Eliminar rol
POST   /api/Roles/{id}/permissions # Asignar permisos
GET    /api/Permissions            # Listar permisos
POST   /api/Permissions            # Crear permiso
```

### 🚧 Governance (En desarrollo)
- Gestión de políticas corporativas
- Comités de gobernanza
- Workflow de aprobación

### 🚧 Risk Management (En desarrollo)
- Catálogo de riesgos
- Evaluación de riesgos
- Controles y mitigación

### 🚧 Compliance (Planificado)
- Catálogo de regulaciones
- Gestión de incidentes
- Reportes regulatorios

## 🛠️ Tecnologías

### Backend
- **.NET 8.0** - Framework principal
- **Entity Framework Core 8.0** - ORM
- **MediatR 12.2.0** - CQRS pattern
- **FluentValidation 11.9.0** - Validaciones
- **AutoMapper 12.0.1** - Object mapping
- **BCrypt.Net 4.0.3** - Password hashing
- **JWT Bearer 8.0.0** - Autenticación

### Testing
- **xUnit 2.5.3** - Framework de testing
- **Moq 4.20.69** - Mocking
- **FluentAssertions 6.12.0** - Assertions

### Documentación
- **Swashbuckle 6.6.2** - Swagger/OpenAPI

## 📁 Estructura del Proyecto

```
GRC.BankingSolution/
├── src/
│   ├── BuildingBlocks/           # Componentes compartidos
│   │   ├── Domain/               # Interfaces y base classes de dominio
│   │   ├── EventBus/             # RabbitMQ event bus
│   │   ├── Infrastructure/       # Infraestructura compartida
│   │   └── IntegrationEvents/    # Eventos de integración
│   │
│   ├── Services/
│   │   ├── Identity/             # Módulo de Identity & Access
│   │   │   ├── Domain/           # Entidades, Value Objects, Aggregates
│   │   │   ├── Application/      # Commands, Queries, DTOs
│   │   │   ├── Infrastructure/   # Repositories, DbContext
│   │   │   └── API/              # Controllers, Middleware
│   │   │
│   │   ├── Governance/           # Módulo de Gobernanza
│   │   ├── Risk/                 # Módulo de Riesgos
│   │   └── Compliance/           # Módulo de Cumplimiento (planificado)
│   │
│   └── ApiGateway/               # API Gateway (planificado)
│
├── tests/
│   ├── Identity.Tests/           # Tests del módulo Identity
│   ├── Governance.Tests/
│   └── Risk.Tests/
│
└── docs/                         # Documentación
    ├── architecture/
    └── api-specs/
```

## 🚀 Inicio Rápido

### Prerrequisitos
- .NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 o VS Code

### Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/grc-banking-solution.git
cd grc-banking-solution
```

2. **Configurar base de datos**

Actualiza la cadena de conexión en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "IdentityConnection": "Server=localhost;Database=GRC_Identity_Dev;User Id=sa;Password=TuPassword;TrustServerCertificate=True"
  }
}
```

3. **Aplicar migraciones**
```bash
cd src/Services/Identity/GRC.Identity.API
dotnet ef database update --project ../GRC.Identity.Infrastructure
```

4. **Ejecutar la aplicación**
```bash
dotnet run
```

5. **Acceder a Swagger**
```
https://localhost:7002
```

### Usuarios de Prueba

Después del seed inicial:

**Administrador:**
- Email: `admin@grc.com`
- Password: `Admin@123`

**Usuario Regular:**
- Email: `user@grc.com`
- Password: `User@123`

## 🔑 Sistema de Permisos

El sistema implementa un modelo de permisos basado en:

**Formato**: `Recurso.Acción`

**Recursos disponibles:**
- Users, Roles, Permissions
- Policies, Committees, Meetings
- Risks, Controls, Incidents
- Regulations, Reports

**Acciones disponibles:**
- Create, Read, Update, Delete
- Approve, Reject, Publish, Execute

**Ejemplo de permisos:**
- `Users.Create` - Crear usuarios
- `Roles.Update` - Actualizar roles
- `Policies.Approve` - Aprobar políticas
- `Reports.Execute` - Ejecutar reportes

## 📚 Guía de Desarrollo

### Crear una nueva migración
```bash
dotnet ef migrations add NombreMigracion \
  --project src/Services/Identity/GRC.Identity.Infrastructure \
  --startup-project src/Services/Identity/GRC.Identity.API \
  --context IdentityContext
```

### Aplicar migraciones
```bash
dotnet ef database update \
  --project src/Services/Identity/GRC.Identity.Infrastructure \
  --startup-project src/Services/Identity/GRC.Identity.API \
  --context IdentityContext
```

### Ejecutar tests
```bash
dotnet test
```

### Build de la solución
```bash
dotnet build
```

## 🎯 Roadmap

### Fase 1: Identity ✅ (Completado)
- [x] Autenticación JWT
- [x] Gestión de usuarios
- [x] Sistema de roles
- [x] Sistema de permisos

### Fase 2: Governance 🚧 (En Progreso)
- [ ] Gestión de políticas
- [ ] Workflow de aprobación
- [ ] Comités de gobernanza
- [ ] Dashboard ejecutivo

### Fase 3: Risk Management 📋 (Planificado)
- [ ] Catálogo de riesgos
- [ ] Matriz de riesgos
- [ ] Gestión de controles
- [ ] Planes de mitigación

### Fase 4: Compliance 📋 (Planificado)
- [ ] Catálogo de regulaciones
- [ ] Gestión de incidentes
- [ ] Reportes regulatorios
- [ ] Calendario de cumplimiento

### Fase 5: Integración 📋 (Planificado)
- [ ] Event Bus (RabbitMQ)
- [ ] API Gateway (Ocelot)
- [ ] Dashboards integrados
- [ ] Reporting centralizado

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto es privado y confidencial.

## 👥 Equipo

- **Desarrolladores**: [Tu Nombre]
- **Arquitecto**: [Nombre]

## 📞 Contacto

Para preguntas o soporte: [tu-email@ejemplo.com]

---

**Última actualización**: Enero 2026
**Versión**: 1.0.0