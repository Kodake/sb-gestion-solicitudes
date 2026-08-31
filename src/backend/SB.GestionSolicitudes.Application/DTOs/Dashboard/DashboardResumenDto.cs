using SB.GestionSolicitudes.Application.DTOs.Solicitudes;

namespace SB.GestionSolicitudes.Application.DTOs.Dashboard;

public class DashboardResumenDto
{
    public int TotalSolicitudes { get; set; }
    public int SolicitudesAbiertas { get; set; }
    public int SolicitudesCerradas { get; set; }
    public int SolicitudesVencidas { get; set; }
    public List<MetricaEstadoDto> PorEstado { get; set; } = new();
    public List<MetricaPrioridadDto> PorPrioridad { get; set; } = new();
    public List<SolicitudDto> UltimasSolicitudes { get; set; } = new();
}
