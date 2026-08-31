using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Solicitudes;

public class HistorialEstadoDto
{
    public int Id { get; set; }
    public int SolicitudId { get; set; }
    public EstadoSolicitudEnum? EstadoAnterior { get; set; }
    public string? EstadoAnteriorNombre => EstadoAnterior?.ToString();
    public EstadoSolicitudEnum EstadoNuevo { get; set; }
    public string EstadoNuevoNombre => EstadoNuevo.ToString();
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string Comentario { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}
