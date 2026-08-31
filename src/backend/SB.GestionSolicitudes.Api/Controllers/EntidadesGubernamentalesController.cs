using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;
using SB.GestionSolicitudes.Application.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize]
[Route("api/v1/entidades-gubernamentales")]
public class EntidadesGubernamentalesController : BaseApiController
{
    private readonly IEntidadGubernamentalService _entidadService;

    public EntidadesGubernamentalesController(IEntidadGubernamentalService entidadService)
    {
        _entidadService = entidadService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<EntidadGubernamentalDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedList<EntidadGubernamentalDto>>>> GetEntidades([FromQuery] FiltroEntidadesGubernamentalesDto filtro)
    {
        var result = await _entidadService.GetEntidadesAsync(filtro);
        return ProcessResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<EntidadGubernamentalDto>>> GetEntidadById(int id)
    {
        var result = await _entidadService.GetEntidadByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpGet("sectores")]
    [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetSectores()
    {
        var result = await _entidadService.GetSectoresAsync();
        return ProcessResult(result);
    }

    [HttpGet("poderes")]
    [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetPoderes()
    {
        var result = await _entidadService.GetPoderesEstadoAsync();
        return ProcessResult(result);
    }

    [HttpGet("categorias")]
    [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetCategorias()
    {
        var result = await _entidadService.GetCategoriasAsync();
        return ProcessResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<EntidadGubernamentalDto>>> CrearEntidad([FromBody] CrearEntidadGubernamentalDto dto)
    {
        var result = await _entidadService.CrearEntidadAsync(dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EntidadGubernamentalDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<EntidadGubernamentalDto>>> ActualizarEntidad(int id, [FromBody] ActualizarEntidadGubernamentalDto dto)
    {
        var result = await _entidadService.ActualizarEntidadAsync(id, dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/toggle-activo")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> ToggleEstadoEntidad(int id)
    {
        var result = await _entidadService.ToggleEstadoEntidadAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> EliminarEntidad(int id)
    {
        var result = await _entidadService.EliminarEntidadAsync(id, CurrentUserId);
        return ProcessResult(result);
    }
}
