using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Dashboard;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface IDashboardService
{
    Task<Result<DashboardResumenDto>> GetResumenAsync(int currentUserId, RolEnum currentRol);
}
