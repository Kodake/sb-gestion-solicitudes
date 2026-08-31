namespace SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;

public class ActualizarEntidadGubernamentalDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string PoderEstado { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string? Siglas { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? SitioWeb { get; set; }
    public bool Activo { get; set; }
}
