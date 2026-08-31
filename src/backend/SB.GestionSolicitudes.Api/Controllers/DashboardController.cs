using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Dashboard;
using SB.GestionSolicitudes.Application.Interfaces;

namespace SB.GestionSolicitudes.Api.Controllers;

[Authorize]
public class DashboardController : BaseApiController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("resumen")]
    [ProducesResponseType(typeof(ApiResponse<DashboardResumenDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<DashboardResumenDto>>> GetResumen()
    {
        var result = await _dashboardService.GetResumenAsync(CurrentUserId, CurrentRol);
        return ProcessResult(result);
    }
}
