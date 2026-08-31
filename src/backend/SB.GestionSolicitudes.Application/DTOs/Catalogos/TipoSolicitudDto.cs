namespace SB.GestionSolicitudes.Application.DTOs.Catalogos;

public class TipoSolicitudDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}
