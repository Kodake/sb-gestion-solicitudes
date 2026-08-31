using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class ActualizarSolicitudDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int AreaId { get; set; }
    public int TipoSolicitudId { get; set; }
    public PrioridadEnum Prioridad { get; set; }
    public DateTime? FechaCompromiso { get; set; }
    public string? ReferenciaEvidencia { get; set; }
}
