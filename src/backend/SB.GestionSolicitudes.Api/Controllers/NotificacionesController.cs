using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Notificaciones;
using SB.GestionSolicitudes.Application.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize]
public class NotificacionesController : BaseApiController
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<NotificacionDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<NotificacionDto>>>> GetNotificaciones()
    {
        var result = await _notificacionService.GetNotificacionesUsuarioAsync(CurrentUserId);
        return ProcessResult(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> EliminarNotificacion(int id)
    {
        var result = await _notificacionService.EliminarNotificacionAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpDelete]
    [HttpDelete("limpiar")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> LimpiarNotificaciones()
    {
        var result = await _notificacionService.LimpiarNotificacionesUsuarioAsync(CurrentUserId);
        return ProcessResult(result);
    }
}
