using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Comentarios;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface ISolicitudService
{
    Task<Result<PaginatedList<SolicitudDto>>> GetSolicitudesAsync(FiltroSolicitudesDto filtro, int currentUserId, RolEnum currentRol);
    Task<Result<SolicitudDetalleDto>> GetSolicitudByIdAsync(int id, int currentUserId, RolEnum currentRol);
    Task<Result<SolicitudDto>> CrearSolicitudAsync(CrearSolicitudDto dto, int currentUserId);
    Task<Result<SolicitudDto>> ActualizarSolicitudAsync(int id, ActualizarSolicitudDto dto, int currentUserId, RolEnum currentRol);
    Task<Result<SolicitudDto>> CambiarEstadoAsync(int id, CambiarEstadoDto dto, int currentUserId, RolEnum currentRol);
    Task<Result<SolicitudDto>> AsignarResponsableAsync(int id, AsignarResponsableDto dto, int currentUserId, RolEnum currentRol);
    Task<Result<ComentarioDto>> AgregarComentarioAsync(int solicitudId, CrearComentarioDto dto, int currentUserId, RolEnum currentRol);
}
