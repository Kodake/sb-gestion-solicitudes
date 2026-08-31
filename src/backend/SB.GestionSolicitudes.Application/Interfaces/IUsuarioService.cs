using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Usuarios;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface IUsuarioService
{
    Task<Result<PaginatedList<UsuarioDto>>> GetUsuariosAsync(FiltroUsuariosDto filtro);
    Task<Result<UsuarioDto>> GetUsuarioByIdAsync(int id);
    Task<Result<UsuarioDto>> CrearUsuarioAsync(CrearUsuarioDto dto, int currentUserId);
    Task<Result<UsuarioDto>> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto, int currentUserId);
    Task<Result<bool>> ToggleEstadoUsuarioAsync(int id, int currentUserId);
    Task<Result<bool>> EliminarUsuarioAsync(int id, int currentUserId);
}
