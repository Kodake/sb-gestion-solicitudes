# Arquitectura del Sistema de Gestión de Solicitudes (SB)

Documento de diseño arquitectónico y técnico del **Sistema de Gestión de Solicitudes Internas** para la **Superintendencia de Bancos de la República Dominicana (SB)**.

---

## 🏛️ 1. Arquitectura General de la Solución

El sistema se compone de dos subsistemas principales desacoplados:
1. **Backend RESTful (.NET 8):** Diseñado bajo **Onion Architecture (Arquitectura en Cebolla)** siguiendo la nomenclatura estándar institucional `[SB].[NombreProyecto].[Capa]`.
2. **Frontend SPA (Next.js 16 + React 19 + TypeScript):** Interfaz moderna configurada como **SPA de cliente estática pura** (`output: 'export'`) sin dependencias de runtime de servidor Node.js.

```text
┌────────────────────────────────────────────────────────┐
│                   Cliente Web (SPA)                    │
│        React / Next.js (Tailwind + TypeScript)         │
└───────────────────────────┬────────────────────────────┘
                            │ HTTP / REST / JSON (JWT)
                            ▼
┌────────────────────────────────────────────────────────┐
│             SB.GestionSolicitudes.Api                  │
│       Controladores REST, Middleware RFC 7807          │
├────────────────────────────────────────────────────────┤
│           SB.GestionSolicitudes.Services               │
│          Servicios de Aplicación & MediatR             │
├────────────────────────────────────────────────────────┤
│         SB.GestionSolicitudes.Application              │
│       DTOs, FluentValidation, Result Pattern           │
├────────────────────────────────────────────────────────┤
│            SB.GestionSolicitudes.Domain                │
│     Dominio Rico, Invariantes, Matriz Transiciones     │
├────────────────────────────────────────────────────────┤
│        SB.GestionSolicitudes.Infrastructure            │
│       EF Core, SQL Server / LocalDB, Repositorios      │
└────────────────────────────────────────────────────────┘
```

---

## 🧩 2. Capas del Backend (.NET 8)

### 2.1 Dominio (`SB.GestionSolicitudes.Domain`)
- **Entidades:** `Solicitud`, `Usuario`, `Area`, `TipoSolicitud`, `Comentario`, `Notificacion`, `EntidadGubernamental`.
- **Dominio Rico:** Métodos de negocio encapsulados en la entidad (`CambiarEstado`, `AsignarResponsable`, `ActualizarInformacion`).
- **Matriz de Transiciones (`SolicitudTransiciones.cs`):** Valida formalmente el ciclo de vida de las solicitudes:
  - `Registrada` $\rightarrow$ `EnAnalisis`, `EnProgreso`, `Cerrada`
  - `EnAnalisis` $\rightarrow$ `EnProgreso`, `EnEsperaDelSolicitante`, `Resuelta`, `Cerrada`
  - `EnProgreso` $\rightarrow$ `EnEsperaDelSolicitante`, `Resuelta`, `Cerrada`
  - `EnEsperaDelSolicitante` $\rightarrow$ `EnProgreso`, `EnAnalisis`, `Resuelta`, `Cerrada`
  - `Resuelta` $\rightarrow$ `Cerrada`, `EnProgreso`
  - `Cerrada` $\rightarrow$ `EnProgreso` (Reapertura exclusiva para Admin o Analista)

### 2.2 Aplicación (`SB.GestionSolicitudes.Application`)
- **DTOs:** Modelos de transferencia de datos segregados por comando y consulta.
- **Validadores:** Implementados con **FluentValidation** para garantizar la integridad de las entradas.
- **Result Pattern (`Result<T>`):** Control determinista de éxito/fallo sin basarse en excepciones para el flujo normal.

### 2.3 Servicios (`SB.GestionSolicitudes.Services`)
- **Orquestación:** `SolicitudService`, `UsuarioService`, `CatalogoService`, `EntidadGubernamentalService`, `DashboardService`.
- **Event Handlers:** Suscripción a eventos de dominio mediante MediatR (`SolicitudCreadaEvent`, `EstadoSolicitudCambiadoEvent`, etc.).

### 2.4 Infraestructura (`SB.GestionSolicitudes.Infrastructure`)
- **Persistencia:** Entity Framework Core con **Microsoft.EntityFrameworkCore.SqlServer**.
- **Seguridad:** Generación y validación de tokens JWT con Claims institucionales (`Id`, `Nombre`, `Correo`, `Rol`).
- **Notificaciones:** Persistencia de mensajes dirigidos por usuario en base de datos.

### 2.5 API Web (`SB.GestionSolicitudes.Api`)
- Controladores delgados que heredan de `BaseApiController`.
- Middleware global `GlobalExceptionHandlerMiddleware` con formato estándar RFC 7807 (`ProblemDetails`).
- Documentación interactiva Swagger / OpenAPI con soporte para Authorization Bearer.

---

## 🔒 3. Seguridad y Manejo de Secretos

- **Sin secretos versionados:** `appsettings.json` utiliza marcadores de posición sin exponer claves de producción.
- **Variables de Entorno:** Soporte prioritario para `JWT_SECRET` y `.env.example`.
- **RBAC (Role-Based Access Control):** Tres roles claramente diferenciados:
  - `Administrador` (Rol 1): Acceso y control total de la plataforma.
  - `Analista` (Rol 2): Gestión técnica de solicitudes asignadas y disponibles.
  - `Solicitante` (Rol 3): Registro de requerimientos y consulta exclusiva de solicitudes propias.

---

## 💾 4. Base de Datos (SQL Server)

- **Esquema Relacional:** Script DDL T-SQL oficial en [`database/schema_sqlserver.sql`](../database/schema_sqlserver.sql).
- **Semilla Institucional:** Inicializador `DbInitializer.cs` con usuarios de prueba, áreas, tipos de requerimiento y las **Entidades Gubernamentales oficiales del Estado Dominicano** basadas en el catálogo nacional.
