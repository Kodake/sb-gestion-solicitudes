using SB.GestionSolicitudes.Domain.Common;

namespace SB.GestionSolicitudes.Domain.Entities;

public class TipoSolicitud : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    // Navigation properties
    public ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
