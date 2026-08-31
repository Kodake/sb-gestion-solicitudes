using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Usuarios;
using SB.GestionSolicitudes.Application.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : BaseApiController
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<UsuarioDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PaginatedList<UsuarioDto>>>> GetUsuarios([FromQuery] FiltroUsuariosDto filtro)
    {
        var result = await _usuarioService.GetUsuariosAsync(filtro);
        return ProcessResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> GetUsuarioById(int id)
    {
        var result = await _usuarioService.GetUsuarioByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> CrearUsuario([FromBody] CrearUsuarioDto dto)
    {
        var result = await _usuarioService.CrearUsuarioAsync(dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UsuarioDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioDto dto)
    {
        var result = await _usuarioService.ActualizarUsuarioAsync(id, dto, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/toggle-estado")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> ToggleEstadoUsuario(int id)
    {
        var result = await _usuarioService.ToggleEstadoUsuarioAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> EliminarUsuario(int id)
    {
        var result = await _usuarioService.EliminarUsuarioAsync(id, CurrentUserId);
        return ProcessResult(result);
    }
}
