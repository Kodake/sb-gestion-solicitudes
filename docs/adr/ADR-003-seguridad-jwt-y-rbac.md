# ADR-003: Seguridad, Autenticación JWT y Control de Acceso Basado en Roles (RBAC)

## Estado
Aceptado

## Contexto
El sistema gestiona solicitudes internas que requieren confidencialidad y control estricto de acceso. Se deben distinguir tres roles institucionales:
1. `Administrador`: Control absoluto sobre solicitudes, áreas, tipos de solicitud, catálogo de entidades gubernamentales y gestión de cuentas de usuario.
2. `Analista IT`: Asignación, análisis, progreso y resolución de solicitudes técnicas asignadas o disponibles.
3. `Solicitante`: Creación de requerimientos (`SOL-2026-XXXX`) y consulta/edición restringida a sus solicitudes propias.

## Decisión
- **Tokens JWT Bearer:** Se emiten tokens firmados con HMAC-SHA256 conteniendo claims institucionales (`NameIdentifier`, `Email`, `Name`, `Role`).
- **Validación Estricta de Secretos:** La API exige `JWT_SECRET` mediante variable de entorno o configuración de producción. Si la clave está ausente o contiene un marcador de posición, el sistema aborta de inmediato (`throw InvalidOperationException`), previniendo despliegues inseguros.
- **Autorización por Endpoint:** Se aplican atributos `[Authorize(Roles = "Administrador")]` y filtros de alcance en servicios para que los analistas solo operen sobre sus asignaciones y los solicitantes solo modifiquen sus requerimientos en estados iniciales.
- **Auditoría:** Cada entidad registra `FechaCreacion`, `UsuarioCreacionId`, `FechaModificacion` y `UsuarioModificacionId`.

## Consecuencias
- **Positivas:** Seguridad robusta alineada a estándares bancarios, trazabilidad completa de cada acción y cumplimiento del principio de menor privilegio.
- **Negativas:** Se requiere propagación del token JWT en todas las peticiones autenticadas desde el cliente.
