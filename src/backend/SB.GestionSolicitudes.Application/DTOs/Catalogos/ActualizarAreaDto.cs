namespace SB.GestionSolicitudes.Application.DTOs.Catalogos;

public class ActualizarAreaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activa { get; set; }
}
