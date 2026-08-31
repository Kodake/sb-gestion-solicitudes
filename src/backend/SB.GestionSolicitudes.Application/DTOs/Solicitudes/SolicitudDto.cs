using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class SolicitudDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public PrioridadEnum Prioridad { get; set; }
    public string PrioridadNombre => Prioridad.ToString();
    public EstadoSolicitudEnum Estado { get; set; }
    public string EstadoNombre => Estado.ToString();
    public string? ReferenciaEvidencia { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaCompromiso { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    public int SolicitanteId { get; set; }
    public string SolicitanteNombre { get; set; } = string.Empty;
    public int? ResponsableId { get; set; }
    public string? ResponsableNombre { get; set; }

    public int AreaId { get; set; }
    public string AreaNombre { get; set; } = string.Empty;
    public int TipoSolicitudId { get; set; }
    public string TipoSolicitudNombre { get; set; } = string.Empty;
    public bool EstaVencida => FechaCompromiso < DateTime.UtcNow && Estado != EstadoSolicitudEnum.Resuelta && Estado != EstadoSolicitudEnum.Cerrada;
}
