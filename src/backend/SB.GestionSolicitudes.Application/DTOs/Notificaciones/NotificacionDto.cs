using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Notificaciones;

public class NotificacionDto
{
    public int Id { get; set; }
    public int? SolicitudId { get; set; }
    public int UsuarioDestinoId { get; set; }
    public string UsuarioDestinoNombre { get; set; } = string.Empty;
    public CanalNotificacionEnum Canal { get; set; }
    public string CanalNombre => Canal.ToString();
    public string Asunto { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public bool Enviado { get; set; }
    public DateTime Fecha { get; set; }
}
