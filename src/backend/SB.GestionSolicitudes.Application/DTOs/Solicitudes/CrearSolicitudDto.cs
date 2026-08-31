using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class CrearSolicitudDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public PrioridadEnum Prioridad { get; set; } = PrioridadEnum.Media;
    public int AreaId { get; set; }
    public int TipoSolicitudId { get; set; }
    public string? ReferenciaEvidencia { get; set; }
    public DateTime? FechaCompromiso { get; set; }
}
