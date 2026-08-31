using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Dashboard;

public class MetricaEstadoDto
{
    public EstadoSolicitudEnum Estado { get; set; }
    public string EstadoNombre => Estado.ToString();
    public int Cantidad { get; set; }
}
