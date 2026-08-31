using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Comentarios;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Application.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize]
public class SolicitudesController : BaseApiController
{
    private readonly ISolicitudService _solicitudService;

    public SolicitudesController(ISolicitudService solicitudService)
    {
        _solicitudService = solicitudService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<SolicitudDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedList<SolicitudDto>>>> GetSolicitudes([FromQuery] FiltroSolicitudesDto filtro)
    {
        var result = await _solicitudService.GetSolicitudesAsync(filtro, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDetalleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDetalleDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SolicitudDetalleDto>>> GetSolicitudById(int id)
    {
        var result = await _solicitudService.GetSolicitudByIdAsync(id, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SolicitudDto>>> CrearSolicitud([FromBody] CrearSolicitudDto dto)
    {
        var result = await _solicitudService.CrearSolicitudAsync(dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPut("{id:int}")]
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SolicitudDto>>> ActualizarSolicitud(int id, [FromBody] ActualizarSolicitudDto dto)
    {
        var result = await _solicitudService.ActualizarSolicitudAsync(id, dto, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/estado")]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SolicitudDto>>> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
    {
        var result = await _solicitudService.CambiarEstadoAsync(id, dto, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/asignacion")]
    [Authorize(Roles = "Administrador,Analista")]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<SolicitudDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SolicitudDto>>> AsignarResponsable(int id, [FromBody] AsignarResponsableDto dto)
    {
        var result = await _solicitudService.AsignarResponsableAsync(id, dto, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }

    [HttpPost("{id:int}/comentarios")]
    [ProducesResponseType(typeof(ApiResponse<ComentarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComentarioDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ComentarioDto>>> AgregarComentario(int id, [FromBody] CrearComentarioDto dto)
    {
        var result = await _solicitudService.AgregarComentarioAsync(id, dto, CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }
}
