using SB.GestionSolicitudes.Application.DTOs.Comentarios;
using SB.GestionSolicitudes.Application.DTOs.Notificaciones;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class SolicitudDetalleDto : SolicitudDto
{
    public List<HistorialEstadoDto> HistorialEstados { get; set; } = new();
    public List<ComentarioDto> Comentarios { get; set; } = new();
    public List<NotificacionDto> Notificaciones { get; set; } = new();
}
