using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Auth;
using SB.GestionSolicitudes.Application.DTOs.Catalogos;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize]
public class CatalogosController : BaseApiController
{
    private readonly ICatalogoService _catalogoService;
    private readonly IUnitOfWork _unitOfWork;

    public CatalogosController(ICatalogoService catalogoService, IUnitOfWork unitOfWork)
    {
        _catalogoService = catalogoService;
        _unitOfWork = unitOfWork;
    }

    // --- ÁREAS ---

    [HttpGet("areas")]
    [ProducesResponseType(typeof(ApiResponse<List<AreaDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<AreaDto>>>> GetAreas([FromQuery] bool? soloActivas)
    {
        var result = await _catalogoService.GetAreasAsync(soloActivas ?? false);
        return ProcessResult(result);
    }

    [HttpGet("areas/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AreaDto>>> GetAreaById(int id)
    {
        var result = await _catalogoService.GetAreaByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost("areas")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AreaDto>>> CrearArea([FromBody] CrearAreaDto dto)
    {
        var result = await _catalogoService.CrearAreaAsync(dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPut("areas/{id:int}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AreaDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AreaDto>>> ActualizarArea(int id, [FromBody] ActualizarAreaDto dto)
    {
        var result = await _catalogoService.ActualizarAreaAsync(id, dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("areas/{id:int}/toggle-activo")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> ToggleEstadoArea(int id)
    {
        var result = await _catalogoService.ToggleEstadoAreaAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    // --- TIPOS DE SOLICITUD ---

    [HttpGet("tipos-solicitud")]
    [ProducesResponseType(typeof(ApiResponse<List<TipoSolicitudDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<TipoSolicitudDto>>>> GetTiposSolicitud([FromQuery] bool? soloActivos)
    {
        var result = await _catalogoService.GetTiposSolicitudAsync(soloActivos ?? false);
        return ProcessResult(result);
    }

    [HttpGet("tipos-solicitud/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TipoSolicitudDto>>> GetTipoSolicitudById(int id)
    {
        var result = await _catalogoService.GetTipoSolicitudByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost("tipos-solicitud")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TipoSolicitudDto>>> CrearTipoSolicitud([FromBody] CrearTipoSolicitudDto dto)
    {
        var result = await _catalogoService.CrearTipoSolicitudAsync(dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPut("tipos-solicitud/{id:int}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TipoSolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TipoSolicitudDto>>> ActualizarTipoSolicitud(int id, [FromBody] ActualizarTipoSolicitudDto dto)
    {
        var result = await _catalogoService.ActualizarTipoSolicitudAsync(id, dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("tipos-solicitud/{id:int}/toggle-activo")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> ToggleEstadoTipoSolicitud(int id)
    {
        var result = await _catalogoService.ToggleEstadoTipoSolicitudAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    // --- LISTADO SIMPLE DE USUARIOS ACTIVOS PARA SELECTORES ---

    [HttpGet("usuarios")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserDto>>>> GetUsuarios([FromQuery] RolEnum? rol)
    {
        var usuarios = await _unitOfWork.Usuarios.FindAsync(u => u.Activo && (!rol.HasValue || u.Rol == rol.Value));
        var dtos = usuarios.Select(u => new UserDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo,
            Rol = u.Rol,
            Activo = u.Activo
        }).ToList();

        return Ok(ApiResponse<IReadOnlyList<UserDto>>.Ok(dtos));
    }
}
