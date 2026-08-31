using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolEnum Rol { get; set; } = RolEnum.Solicitante;
    public bool Activo { get; set; } = true;

    // Navigation properties
    public ICollection<Solicitud> SolicitudesCreadas { get; set; } = new List<Solicitud>();
    public ICollection<Solicitud> SolicitudesAsignadas { get; set; } = new List<Solicitud>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public ICollection<HistorialEstado> HistorialesEstado { get; set; } = new List<HistorialEstado>();
}
