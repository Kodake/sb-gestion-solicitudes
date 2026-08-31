using SB.GestionSolicitudes.Domain.Common;

namespace SB.GestionSolicitudes.Domain.Entities;

public class Comentario : BaseEntity
{
    public int SolicitudId { get; set; }
    public int UsuarioId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public bool EsPublico { get; set; } = true;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Solicitud Solicitud { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
