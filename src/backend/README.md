# SB.GestionSolicitudes - Backend (.NET 8 Web API)
**Arquitectura Limpia (Onion Architecture) para el Sistema de Gestión de Solicitudes de la Superintendencia de Bancos**

Solución empresarial modular basada en principios de diseño orientado al dominio (*DDD*), encapsulación de invariantes, validación declarativa y separación estricta de responsabilidades.

---

## 🏛️ Estructura del Proyecto

```text
src/backend/
├── SB.GestionSolicitudes.Domain/         # Entidades de Dominio, Reglas de Negocio, Matriz de Transiciones
├── SB.GestionSolicitudes.Application/    # DTOs, Validaciones FluentValidation, Interfaces, Result Pattern
├── SB.GestionSolicitudes.Services/       # Servicios de Lógica de Negocio, Event Handlers de Notificaciones
├── SB.GestionSolicitudes.Infrastructure/ # ApplicationDbContext (EF Core SQL Server), Repositorio Genérico, UnitOfWork, JWT, Seeder
├── SB.GestionSolicitudes.Api/            # Controladores REST, Middleware Global RFC 7807, Serilog, Swagger UI
└── SB.GestionSolicitudes.Tests/          # Pruebas Unitarias e Integración (xUnit - 36 pruebas passing)
```

---

## 📋 Requisitos y Ejecución Local

### Prerrequisitos del Sistema

- **.NET 8 SDK** (`v8.0` o superior).
  - Verificar instalación con: `dotnet --version`
- **Motor BD:** SQL Server / LocalDB (`(localdb)\MSSQLLocalDB`) o SQL Server Express/Developer.
  - La base de datos se inicializa y siembra automáticamente al iniciar la API.
  - Script DDL T-SQL disponible en `database/schema.sql`.

### Configuración de Ambiente (Sin Secretos Reales en Repositorio)

La solución incluye plantillas de configuración de ambiente según el **Punto 12.1 de la Prueba Técnica**:
- [`SB.GestionSolicitudes.Api/appsettings.Example.json`](SB.GestionSolicitudes.Api/appsettings.Example.json): Plantilla de `appsettings.json` sin secretos expuestos.
- [`.env.example`](.env.example): Variables de entorno para ejecución local o en contenedores.

Para configurar en producción/staging:
```powershell
# Opción 1: Variable de entorno
$env:JWT_SECRET = "SuClaveSecretaPersonalizadaMinimo32CaracteresDeLongitud!"

# Opción 2: User-Secrets de .NET
dotnet user-secrets set "JwtSettings:Secret" "SuClaveSecretaPersonalizadaMinimo32CaracteresDeLongitud!" --project SB.GestionSolicitudes.Api
```

---

## 🚀 Compilación y Ejecución

```powershell
# Restaurar dependencias y compilar solución
dotnet restore SB.GestionSolicitudes.sln
dotnet build SB.GestionSolicitudes.sln

# Ejecutar Web API
dotnet run --project SB.GestionSolicitudes.Api --urls http://localhost:5000
```

- **Swagger UI interactivo:** [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **API Base:** `http://localhost:5000/api/v1`

---

## 🧪 Pruebas Automatizadas

```powershell
dotnet test SB.GestionSolicitudes.sln
```

La suite cuenta con **36 pruebas unitarias e integración en verde** que cubren:
- Matriz de transiciones legales de estado (`SolicitudTransiciones`).
- Dominio e invariantes de cambio de estado y asignación de responsables.
- Servicios de gestión de solicitudes, catálogos, entidades gubernamentales y usuarios.
- Event handlers de notificaciones y autenticación JWT con control de acceso por rol (RBAC).
