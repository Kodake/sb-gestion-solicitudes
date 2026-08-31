using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Infrastructure.Persistence.Repositories;

public class SolicitudRepository : Repository<Solicitud>, ISolicitudRepository
{
    public SolicitudRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Solicitud?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Solicitante)
            .Include(s => s.Responsable)
            .Include(s => s.Area)
            .Include(s => s.TipoSolicitud)
            .Include(s => s.HistorialesEstado.OrderByDescending(h => h.Fecha))
                .ThenInclude(h => h.Usuario)
            .Include(s => s.Comentarios.OrderByDescending(c => c.Fecha))
                .ThenInclude(c => c.Usuario)
            .Include(s => s.Notificaciones.OrderByDescending(n => n.Fecha))
                .ThenInclude(n => n.UsuarioDestino)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<(IReadOnlyList<Solicitud> Items, int TotalCount)> GetPagedAsync(
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
        int? analistaScopeId = null)
    {
        var query = _dbSet
            .Include(s => s.Solicitante)
            .Include(s => s.Responsable)
            .Include(s => s.Area)
            .Include(s => s.TipoSolicitud)
            .AsNoTracking();

        if (estado.HasValue)
            query = query.Where(s => s.Estado == estado.Value);

        if (prioridad.HasValue)
            query = query.Where(s => s.Prioridad == prioridad.Value);

        if (areaId.HasValue)
            query = query.Where(s => s.AreaId == areaId.Value);

        if (solicitanteId.HasValue)
            query = query.Where(s => s.SolicitanteId == solicitanteId.Value);

        if (responsableId.HasValue)
            query = query.Where(s => s.ResponsableId == responsableId.Value);
        else if (analistaScopeId.HasValue)
            query = query.Where(s => s.ResponsableId == analistaScopeId.Value || s.ResponsableId == null);

        if (fechaInicio.HasValue)
            query = query.Where(s => s.FechaCreacion >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(s => s.FechaCreacion <= fechaFin.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(s =>
                s.Codigo.ToLower().Contains(term) ||
                s.Titulo.ToLower().Contains(term) ||
                s.Descripcion.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(s => s.FechaCreacion)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<string> GenerateNextCodigoAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"SOL-{year}-";

        var countThisYear = await _dbSet
            .Where(s => s.Codigo.StartsWith(prefix))
            .CountAsync();

        var nextSequence = countThisYear + 1;
        return $"{prefix}{nextSequence:D4}";
    }
}
