using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Usuarios;

public class ActualizarUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public RolEnum Rol { get; set; }
    public bool Activo { get; set; }
    public string? NuevoPassword { get; set; }
}
