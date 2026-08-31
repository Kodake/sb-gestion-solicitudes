using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Entities;

public class Notificacion : BaseEntity
{
    public int? SolicitudId { get; set; }
    public int UsuarioDestinoId { get; set; }
    public CanalNotificacionEnum Canal { get; set; } = CanalNotificacionEnum.Database;
    public string Asunto { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public bool Enviado { get; set; } = true;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Solicitud? Solicitud { get; set; }
    public Usuario UsuarioDestino { get; set; } = null!;
}
