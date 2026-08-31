using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Usuarios;

public class FiltroUsuariosDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public RolEnum? Rol { get; set; }
    public bool? Activo { get; set; }
}
