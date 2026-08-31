using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Dashboard;

public class MetricaPrioridadDto
{
    public PrioridadEnum Prioridad { get; set; }
    public string PrioridadNombre => Prioridad.ToString();
    public int Cantidad { get; set; }
}
