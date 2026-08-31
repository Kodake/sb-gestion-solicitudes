using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Notificaciones;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface INotificacionService
{
    Task EnviarNotificacionAsync(int usuarioDestinoId, int? solicitudId, string asunto, string mensaje, CanalNotificacionEnum canal = CanalNotificacionEnum.Database);
    Task<Result<IReadOnlyList<NotificacionDto>>> GetNotificacionesUsuarioAsync(int usuarioId);
    Task<Result<bool>> EliminarNotificacionAsync(int notificacionId, int usuarioId);
    Task<Result<bool>> LimpiarNotificacionesUsuarioAsync(int usuarioId);
}
