using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.DTOs.Usuarios;

public class CrearUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RolEnum Rol { get; set; } = RolEnum.Solicitante;
    public bool Activo { get; set; } = true;
}
