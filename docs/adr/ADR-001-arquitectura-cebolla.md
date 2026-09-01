# ADR-001: Implementación de Arquitectura en Cebolla (Onion Architecture)

## Estado
Aceptado

## Contexto
El sistema de gestión de solicitudes de la Superintendencia de Bancos (SB) requiere un alto grado de desacoplamiento, mantenibilidad, testabilidad y separación clara de responsabilidades entre el modelo de negocio, los casos de uso, la persistencia y los puntos de entrada (API REST).

## Decisión
Se adopta la **Arquitectura en Cebolla (Onion Architecture)** organizada en 6 proyectos bajo la nomenclatura institucional estándar `[SB].[NombreProyecto].[Capa]`:
1. `SB.GestionSolicitudes.Domain`: Núcleo de entidades, agregados, eventos de dominio e invariantes de negocio sin dependencias externas.
2. `SB.GestionSolicitudes.Application`: DTOs, validaciones con FluentValidation, interfaces de repositorios y Result Pattern (`Result<T>`).
3. `SB.GestionSolicitudes.Services`: Lógica de orquestación, manejo de comandos/consultas y despacho de eventos de dominio con MediatR.
4. `SB.GestionSolicitudes.Infrastructure`: Persistencia relacional con EF Core y SQL Server, autenticación JWT y acceso a datos.
5. `SB.GestionSolicitudes.Api`: Controladores delgados, configuración de middleware RFC 7807 (`ProblemDetails`), Swagger y CORS.
6. `SB.GestionSolicitudes.Tests`: Suite de pruebas unitarias e integración con xUnit, Moq y EF Core InMemory.

## Consecuencias
- **Positivas:** Máxima independencia de frameworks en el núcleo de dominio, facilidad para sustituir motores de bases de datos o servicios de notificación, y alta cobertura de pruebas automatizadas.
- **Negativas:** Mayor número de proyectos en la solución y necesidad de mapeo entre entidades y DTOs.
