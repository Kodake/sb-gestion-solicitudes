using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected int CurrentUserId
    {
        get
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }

    protected RolEnum CurrentRol
    {
        get
        {
            var claim = User.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.TryParse<RolEnum>(claim, out var rol) ? rol : RolEnum.Solicitante;
        }
    }

    protected ActionResult<ApiResponse<T>> ProcessResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<T>.Ok(result.Value!, result.Message));
        }

        return BadRequest(ApiResponse<T>.Fail(result.Message, result.Errors));
    }

    protected ActionResult<ApiResponse<string>> ProcessResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<string>.Ok(string.Empty, result.Message));
        }

        return BadRequest(ApiResponse<string>.Fail(result.Message, result.Errors));
    }
}
