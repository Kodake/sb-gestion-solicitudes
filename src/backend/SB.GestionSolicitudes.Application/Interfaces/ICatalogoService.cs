using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Catalogos;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface ICatalogoService
{
    Task<Result<List<AreaDto>>> GetAreasAsync(bool soloActivas = false);
    Task<Result<AreaDto>> GetAreaByIdAsync(int id);
    Task<Result<AreaDto>> CrearAreaAsync(CrearAreaDto dto, int currentUserId);
    Task<Result<AreaDto>> ActualizarAreaAsync(int id, ActualizarAreaDto dto, int currentUserId);
    Task<Result<bool>> ToggleEstadoAreaAsync(int id, int currentUserId);

    Task<Result<List<TipoSolicitudDto>>> GetTiposSolicitudAsync(bool soloActivas = false);
    Task<Result<TipoSolicitudDto>> GetTipoSolicitudByIdAsync(int id);
    Task<Result<TipoSolicitudDto>> CrearTipoSolicitudAsync(CrearTipoSolicitudDto dto, int currentUserId);
    Task<Result<TipoSolicitudDto>> ActualizarTipoSolicitudAsync(int id, ActualizarTipoSolicitudDto dto, int currentUserId);
    Task<Result<bool>> ToggleEstadoTipoSolicitudAsync(int id, int currentUserId);
}
