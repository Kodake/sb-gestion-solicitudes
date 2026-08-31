using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class CambiarEstadoDto
{
    public EstadoSolicitudEnum NuevoEstado { get; set; }
    public string Comentario { get; set; } = string.Empty;
}
