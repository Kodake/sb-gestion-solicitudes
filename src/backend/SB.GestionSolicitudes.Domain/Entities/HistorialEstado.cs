using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Entities;

public class HistorialEstado : BaseEntity
{
    public int SolicitudId { get; set; }
    public EstadoSolicitudEnum? EstadoAnterior { get; set; }
    public EstadoSolicitudEnum EstadoNuevo { get; set; }
    public int UsuarioId { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Solicitud Solicitud { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
