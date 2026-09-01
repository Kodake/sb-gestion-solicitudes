# Sistema de Gestión de Solicitudes Internas (SB)
**Prueba Técnica 2 — Superintendencia de Bancos de la República Dominicana (SB)**

Solución Full Stack empresarial desarrollada en **.NET 8 (C#)** con Arquitectura Limpia (*Onion Architecture*) y **React / Next.js 16 (TypeScript)** para el registro, categorización, asignación, seguimiento, transiciones de estado legales y control operativo de requerimientos internos de tecnología.

---

## 📋 Requisitos Previos

Para compilar y ejecutar la solución completa localmente, se requieren los siguientes entornos:

| Componente | Versión Recomendada | Propósito | Comando de Verificación |
|---|---|---|---|
| **.NET 8 SDK** | `v8.0.x` o superior | Compilación y ejecución de la API RESTful Backend. | `dotnet --version` |
| **Node.js** | `v20.x` o superior | Entorno de ejecución para el cliente Frontend Next.js. | `node -v` |
| **npm** | `v10.x` o superior | Gestor de paquetes y dependencias del Frontend. | `npm -v` |
| **Motor BD** | Microsoft SQL Server / LocalDB | Almacenamiento relacional institucional y auditoría. | `sqllocaldb info MSSQLLocalDB` |

---

## 🛡️ Cumplimiento del Criterio 12.1: Configuración de Ambiente de Ejemplo (Sin Secretos Reales)

El proyecto cumple estrictamente con el requerimiento de seguridad **Punto 12.1 ("Configuración de ambiente de ejemplo, sin secretos reales")**:

1. **Sin Secretos en Control de Versiones**:
   - Ninguna clave privada real, token JWT o credencial confidencial está expuesta en los repositorios o archivos de configuración.
   - En [`appsettings.json`](src/backend/SB.GestionSolicitudes.Api/appsettings.json) y [`appsettings.Example.json`](src/backend/SB.GestionSolicitudes.Api/appsettings.Example.json), las claves usan marcadores genéricos de reemplazo (`REPLACE_WITH_ENV_VAR_OR_USER_SECRETS_JWT_SECRET_32_CHARS`).
2. **Validación Estricta en Producción (*Fail-Fast*)**:
   - En `Program.cs`, el sistema valida la presencia de `JWT_SECRET`. En entornos productivos, si la clave falta o mantiene un marcador, el arranque aborta con `InvalidOperationException`. En desarrollo, emite una advertencia de seguridad explícita en los registros.
3. **Plantillas de Entorno Provistas**:
   - **Raíz:** [`.env.example`](.env.example)
   - **Backend:** [`src/backend/.env.example`](src/backend/.env.example)
   - **Frontend:** [`src/frontend/.env.example`](src/frontend/.env.example)
4. **Protección en `.gitignore`**:
   - Archivos de secretos locales (`.env`, `.env.local`, `.env.production.local`, `*.user`) están completamente ignorados.

---

## 🚀 Guía Paso a Paso de Ejecución Local

### Paso 1: Iniciar el Backend (.NET 8 Web API)

1. Abrir una terminal y posicionarse en la carpeta del backend:
   ```powershell
   cd src/backend
   ```
2. Restaurar dependencias y compilar la solución:
   ```powershell
   dotnet build SB.GestionSolicitudes.sln
   ```
3. Ejecutar la API RESTful:
   ```powershell
   dotnet run --project SB.GestionSolicitudes.Api --urls http://localhost:5000
   ```
4. **Verificación Backend**:
   - **API Base:** `http://localhost:5000/api/v1`
   - **Documentación Swagger UI interactiva:** `http://localhost:5000/swagger`
   - *(La base de datos y las 181 entidades gubernamentales oficiales se inicializan automáticamente al primer arranque mediante `DbInitializer.cs`).*

---

### Paso 2: Iniciar el Frontend (Next.js 16 + TypeScript)

1. En una **segunda terminal**, posicionarse en la carpeta del frontend:
   ```powershell
   cd src/frontend
   ```
2. Instalar las dependencias de paquetes:
   ```powershell
   npm install
   ```
3. Iniciar el servidor de desarrollo:
   ```powershell
   npm run dev
   ```
4. **Acceso Frontend**: Abrir en el navegador [http://localhost:3000](http://localhost:3000)

*(Opcional) Para validar compilación limpia para producción:*
```powershell
npm run build
```

---

### Paso 3: Ejecución de Pruebas Automatizadas

#### 1. Pruebas Unitarias e Integración Backend (xUnit)
```powershell
dotnet test src/backend/SB.GestionSolicitudes.sln
```
- **36 pruebas automatizadas (100% Passing)** que validan:
  - Matriz formal de transiciones de estado de solicitudes (`SolicitudTransiciones`).
  - Servicios de Solicitudes, Usuarios, Catálogos y Entidades Gubernamentales.
  - Eventos de Dominio y emisión centralizada de Notificaciones.
  - Validación de reglas de negocio con FluentValidation y AuthService (JWT).

#### 2. Pruebas E2E Frontend con Playwright
```powershell
cd src/frontend
npm run test:e2e
```
- Valida con aserciones formales (`expect`) el comportamiento de autenticación, renderizado de componentes institucionales y control de acceso.

---

## 🔑 Credenciales para Pruebas del Evaluador

Para agilizar la evaluación, en la pantalla de inicio de sesión se incluyen botones de selección rápida de cuenta institucional para autocompletar el correo electrónico:

| Rol | Correo Electrónico | Contraseña de Prueba | Capacidades y Alcance en la Plataforma |
|---|---|---|---|
| **Administrador** | `admin@sb.gob.do` | `Admin123!` | Control total: Catálogos, Gestión de Usuarios, Entidades Gubernamentales, cambio de estados, asignación técnica y configuración general. |
| **Analista IT** | `analista.tech@sb.gob.do` | `Analista123!` | Atención técnica: Asignación de solicitudes, cambios de estado según matriz legal, comentarios públicos y notas internas. Alcance delimitado a asignadas o sin asignar. |
| **Solicitante** | `juan.perez@sb.gob.do` | `User123!` | Usuario final: Registro de requerimientos (`SOL-2026-XXXX`), edición en estado inicial/espera, visualización y seguimiento de solicitudes propias. |

---

## 📊 Matriz de Autoevaluación de Criterios

> **Nota:** Esta matriz refleja una autoevaluación honesta del alcance implementado frente a los criterios de la rúbrica oficial (sección 10). Las limitaciones conocidas se documentan transparentemente.

| Criterio | Peso | Autoevaluación | Evidencia y Limitaciones Conocidas |
| :--- | :---: | :---: | :--- |
| **Funcionalidad Implementada** | **20%** | **18 / 20** | Ciclo completo de solicitudes (`SOL-2026-XXXX`), matriz de transiciones ([`SolicitudTransiciones.cs`](src/backend/SB.GestionSolicitudes.Domain/Services/SolicitudTransiciones.cs)), comentarios públicos/internos, edición, asignación, notificaciones y catálogo de **181 entidades gubernamentales**. *Limitación:* el mecanismo de notificación tiene una sola implementación (base de datos); el canal es un parámetro sin patrón de estrategia extensible. |
| **Diseño Backend y Arquitectura** | **20%** | **17.5 / 20** | Onion Architecture 6 capas (`[SB].[NombreProyecto].[Capa]`), Dominio Rico (`CambiarEstado`, `AsignarResponsable`, `ActualizarInformacion`), Result Pattern, FluentValidation, MediatR, RFC 7807 y [ADRs](docs/adr/). *Limitación:* las propiedades de las entidades mantienen `{ get; set; }` públicos; las notificaciones no usan patrón Outbox transaccional. |
| **Frontend y UX Responsiva** | **15%** | **14 / 15** | Next.js 16 App Router Standalone, paleta institucional SB (`#0D3048`, `#00A86B`, `#E64A19`), diseño responsivo, modales de confirmación con portal y campana interactiva. *Limitación:* Next.js introduce runtime de servidor; se eligió Standalone por soporte a rutas dinámicas (`/solicitudes/[id]`). |
| **Base de Datos y Consultas** | **15%** | **14 / 15** | SQL Server con `EnableRetryOnFailure`, [`DbInitializer.cs`](src/backend/SB.GestionSolicitudes.Infrastructure/Persistence/DbInitializer.cs), script DDL T-SQL ([`schema.sql`](database/schema.sql)) y semilla completa con 181 entidades ([`seed.sql`](database/seed.sql)). *Limitación:* no se implementaron migraciones incrementales de EF Core; se usa `EnsureCreated` + scripts SQL. |
| **Seguridad y Trazabilidad** | **10%** | **9 / 10** | JWT Bearer con validación *fail-fast* en producción, BCrypt, auditoría completa, alcance por rol y `.env.example` sin secretos reales. *Limitación:* en modo `Development`, se usa una clave de firma local de fallback (documentada con advertencia en logs). |
| **Pruebas y Calidad de Código** | **10%** | **8.5 / 10** | **36 pruebas unitarias e integración** en xUnit ([`SB.GestionSolicitudes.Tests`](src/backend/SB.GestionSolicitudes.Tests/)) y pruebas E2E con Playwright (`expect`). *Limitación:* las pruebas E2E validan renderizado y protección de rutas; no cubren flujos completos de negocio contra una API activa. |
| **Documentación y Entrega** | **10%** | **9 / 10** | README, [API REST](API.md), [Arquitectura](docs/arquitectura.md), [Evidencias](docs/evidencias.md), [ADRs](docs/adr/) y 21 capturas por rol. *Limitación:* las evidencias de compilación se generaron en una máquina de desarrollo específica; los resultados pueden variar según la versión de Node.js/Next.js instalada. |
| **TOTAL ESTIMADO** | **100%** | **~90 / 100** | |

---

## 📑 Documentación Adicional del Repositorio

- 📘 [Especificación Técnica de la API REST](API.md)
- 🏛️ [Manual de Arquitectura y Decisiones de Diseño](docs/arquitectura.md)
- 🧪 [Registro de Evidencias de Ejecución](docs/evidencias.md)
- 📜 [Registro de Decisiones de Arquitectura (ADRs)](docs/adr/)
- 📸 [Evidencias Visuales y Flujos por Rol](docs/evidencias_flujos_roles.md)
- 💾 [Script DDL T-SQL para SQL Server](database/schema.sql)
- 🌱 [Script de Semilla Inicial SQL Server](database/seed.sql)
