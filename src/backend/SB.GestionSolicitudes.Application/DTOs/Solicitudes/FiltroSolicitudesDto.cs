using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class FiltroSolicitudesDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public EstadoSolicitudEnum? Estado { get; set; }
    public PrioridadEnum? Prioridad { get; set; }
    public int? AreaId { get; set; }
    public int? SolicitanteId { get; set; }
    public int? ResponsableId { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? SearchTerm { get; set; }
}
