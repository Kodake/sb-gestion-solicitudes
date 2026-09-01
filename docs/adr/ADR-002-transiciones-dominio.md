# ADR-002: Matriz Formal de Transiciones de Estado en Servicios de Dominio

## Estado
Aceptado

## Contexto
El flujo operativo de las solicitudes en la SB debe apegarse a reglas de negocio estrictas para evitar cambios de estado arbitrarios (por ejemplo, una solicitud cerrada no puede pasar directamente a resuelta, ni una solicitud recién registrada puede saltar directamente a resuelta sin análisis previo).

## Decisión
Se implementa una clase estática y pura de dominio `SolicitudTransiciones` dentro de `Domain.Services`, definiendo una matriz formal de transiciones permitidas:
- `Registrada` $\rightarrow$ `EnAnalisis`, `Cerrada`
- `EnAnalisis` $\rightarrow$ `EnProgreso`, `EnEsperaDelSolicitante`, `Cerrada`
- `EnProgreso` $\rightarrow$ `EnAnalisis`, `EnEsperaDelSolicitante`, `Resuelta`, `Cerrada`
- `EnEsperaDelSolicitante` $\rightarrow$ `EnProgreso`, `EnAnalisis`, `Resuelta`, `Cerrada`
- `Resuelta` $\rightarrow$ `Cerrada`, `EnProgreso`
- `Cerrada` $\rightarrow$ `EnAnalisis`, `EnProgreso` (Reapertura técnica justificada)

La entidad `Solicitud` encapsula el método `CambiarEstado(nuevoEstado, usuarioId, comentario)` que invoca la matriz y valida que no se cierre una solicitud sin comentario de cierre.

## Consecuencias
- **Positivas:** Las reglas de transición residen en el modelo de dominio y no pueden ser burladas por capas superiores ni controladores.
- **Negativas:** Cualquier nuevo estado requiere actualizar explícitamente la matriz de transiciones y sus correspondientes pruebas unitarias.
