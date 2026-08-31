using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Dashboard;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DashboardResumenDto>> GetResumenAsync(int currentUserId, RolEnum currentRol)
    {        int? filterSolicitanteId = currentRol == RolEnum.Solicitante ? currentUserId : null;

        var (allSolicitudes, total) = await _unitOfWork.Solicitudes.GetPagedAsync(
            pageNumber: 1,
            pageSize: 1000,
            solicitanteId: filterSolicitanteId
        );

        var now = DateTime.UtcNow;

        var abiertas = allSolicitudes.Count(s => s.Estado != EstadoSolicitudEnum.Cerrada && s.Estado != EstadoSolicitudEnum.Resuelta);
        var cerradas = allSolicitudes.Count(s => s.Estado == EstadoSolicitudEnum.Cerrada || s.Estado == EstadoSolicitudEnum.Resuelta);
        var vencidas = allSolicitudes.Count(s => s.FechaCompromiso < now && s.Estado != EstadoSolicitudEnum.Cerrada && s.Estado != EstadoSolicitudEnum.Resuelta);

        var porEstado = Enum.GetValues<EstadoSolicitudEnum>()
            .Select(e => new MetricaEstadoDto
            {
                Estado = e,
                Cantidad = allSolicitudes.Count(s => s.Estado == e)
            })
            .ToList();

        var porPrioridad = Enum.GetValues<PrioridadEnum>()
            .Select(p => new MetricaPrioridadDto
            {
                Prioridad = p,
                Cantidad = allSolicitudes.Count(s => s.Prioridad == p)
            })
            .ToList();

        var ultimas = allSolicitudes
            .OrderByDescending(s => s.FechaCreacion)
            .Take(5)
            .Select(s => new SolicitudDto
            {
                Id = s.Id,
                Codigo = s.Codigo,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                Prioridad = s.Prioridad,
                Estado = s.Estado,
                ReferenciaEvidencia = s.ReferenciaEvidencia,
                FechaCreacion = s.FechaCreacion,
                FechaCompromiso = s.FechaCompromiso,
                FechaActualizacion = s.FechaActualizacion,
                SolicitanteId = s.SolicitanteId,
                SolicitanteNombre = s.Solicitante?.Nombre ?? string.Empty,
                ResponsableId = s.ResponsableId,
                ResponsableNombre = s.Responsable?.Nombre,
                AreaId = s.AreaId,
                AreaNombre = s.Area?.Nombre ?? string.Empty,
                TipoSolicitudId = s.TipoSolicitudId,
                TipoSolicitudNombre = s.TipoSolicitud?.Nombre ?? string.Empty
            })
            .ToList();

        var dto = new DashboardResumenDto
        {
            TotalSolicitudes = total,
            SolicitudesAbiertas = abiertas,
            SolicitudesCerradas = cerradas,
            SolicitudesVencidas = vencidas,
            PorEstado = porEstado,
            PorPrioridad = porPrioridad,
            UltimasSolicitudes = ultimas
        };

        return Result<DashboardResumenDto>.Success(dto);
    }
}
