using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface IEntidadGubernamentalService
{
    Task<Result<PaginatedList<EntidadGubernamentalDto>>> GetEntidadesAsync(FiltroEntidadesGubernamentalesDto filtro);
    Task<Result<EntidadGubernamentalDto>> GetEntidadByIdAsync(int id);
    Task<Result<EntidadGubernamentalDto>> CrearEntidadAsync(CrearEntidadGubernamentalDto dto, int currentUserId);
    Task<Result<EntidadGubernamentalDto>> ActualizarEntidadAsync(int id, ActualizarEntidadGubernamentalDto dto, int currentUserId);
    Task<Result<bool>> ToggleEstadoEntidadAsync(int id, int currentUserId);
    Task<Result<bool>> EliminarEntidadAsync(int id, int currentUserId);
    Task<Result<List<string>>> GetSectoresAsync();
    Task<Result<List<string>>> GetPoderesEstadoAsync();
    Task<Result<List<string>>> GetCategoriasAsync();
}
