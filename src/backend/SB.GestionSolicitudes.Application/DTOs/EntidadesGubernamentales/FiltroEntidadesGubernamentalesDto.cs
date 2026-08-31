namespace SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;

public class FiltroEntidadesGubernamentalesDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Categoria { get; set; }
    public string? PoderEstado { get; set; }
    public string? Sector { get; set; }
    public bool? Activo { get; set; }
}
