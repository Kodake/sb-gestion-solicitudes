using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Interfaces;

public interface ISolicitudRepository : IRepository<Solicitud>
{
    Task<Solicitud?> GetByIdWithDetailsAsync(int id);
    Task<(IReadOnlyList<Solicitud> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        EstadoSolicitudEnum? estado = null,
        PrioridadEnum? prioridad = null,
        int? areaId = null,
        int? solicitanteId = null,
        int? responsableId = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        string? searchTerm = null,
        int? analistaScopeId = null
    );
    Task<string> GenerateNextCodigoAsync();
}
