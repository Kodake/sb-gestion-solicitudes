using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class EntidadGubernamentalService : IEntidadGubernamentalService
{
    private readonly IUnitOfWork _unitOfWork;

    public EntidadGubernamentalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedList<EntidadGubernamentalDto>>> GetEntidadesAsync(FiltroEntidadesGubernamentalesDto filtro)
    {
        var query = _unitOfWork.Repository<EntidadGubernamental>().Query();

        if (filtro.Activo.HasValue)
        {
            query = query.Where(e => e.Activo == filtro.Activo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Sector))
        {
            query = query.Where(e => e.Sector == filtro.Sector);
        }

        if (!string.IsNullOrWhiteSpace(filtro.PoderEstado))
        {
            query = query.Where(e => e.PoderEstado == filtro.PoderEstado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Categoria))
        {
            query = query.Where(e => e.Categoria == filtro.Categoria);
        }

        if (!string.IsNullOrWhiteSpace(filtro.SearchTerm))
        {
            var term = filtro.SearchTerm.Trim().ToLower();
            query = query.Where(e => e.Nombre.ToLower().Contains(term) || (e.Siglas != null && e.Siglas.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(e => e.Nombre)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ToListAsync();

        var dtos = items.Select(MapToDto).ToList();
        var paginated = new PaginatedList<EntidadGubernamentalDto>(dtos, totalCount, filtro.PageNumber, filtro.PageSize);

        return Result<PaginatedList<EntidadGubernamentalDto>>.Success(paginated);
    }

    public async Task<Result<EntidadGubernamentalDto>> GetEntidadByIdAsync(int id)
    {
        var entidad = await _unitOfWork.Repository<EntidadGubernamental>().GetByIdAsync(id);
        if (entidad == null)
        {
            return Result<EntidadGubernamentalDto>.Failure(MensajesSistema.EntidadGubernamentalMensajes.ENTIDAD_NO_EXISTE);
        }

        return Result<EntidadGubernamentalDto>.Success(MapToDto(entidad));
    }

    public async Task<Result<EntidadGubernamentalDto>> CrearEntidadAsync(CrearEntidadGubernamentalDto dto, int currentUserId)
    {
        var nombreTrim = dto.Nombre.Trim();
        var existe = await _unitOfWork.Repository<EntidadGubernamental>()
            .Query()
            .AnyAsync(e => e.Nombre.ToLower() == nombreTrim.ToLower());

        if (existe)
        {
            return Result<EntidadGubernamentalDto>.Failure(MensajesSistema.EntidadGubernamentalMensajes.REGISTRO_DUPLICADO);
        }

        var entidad = new EntidadGubernamental
        {
            Nombre = nombreTrim,
            Categoria = dto.Categoria.Trim(),
            PoderEstado = dto.PoderEstado.Trim(),
            Sector = dto.Sector.Trim(),
            Siglas = dto.Siglas?.Trim(),
            Direccion = dto.Direccion?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            SitioWeb = dto.SitioWeb?.Trim(),
            Activo = dto.Activo,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCreacionId = currentUserId
        };

        await _unitOfWork.Repository<EntidadGubernamental>().AddAsync(entidad);
        await _unitOfWork.SaveChangesAsync();

        entidad.AddDomainEvent(new Domain.Events.EntidadGubernamentalCreadaEvent(entidad.Id, entidad.Nombre, entidad.Sector, currentUserId));
        await _unitOfWork.SaveChangesAsync();

        return Result<EntidadGubernamentalDto>.Success(MapToDto(entidad), MensajesSistema.EntidadGubernamentalMensajes.CREACION_EXITOSA);
    }

    public async Task<Result<EntidadGubernamentalDto>> ActualizarEntidadAsync(int id, ActualizarEntidadGubernamentalDto dto, int currentUserId)
    {
        var entidad = await _unitOfWork.Repository<EntidadGubernamental>().GetByIdAsync(id);
        if (entidad == null)
        {
            return Result<EntidadGubernamentalDto>.Failure(MensajesSistema.EntidadGubernamentalMensajes.ENTIDAD_NO_EXISTE);
        }

        var nombreTrim = dto.Nombre.Trim();
        var duplicado = await _unitOfWork.Repository<EntidadGubernamental>()
            .Query()
            .AnyAsync(e => e.Id != id && e.Nombre.ToLower() == nombreTrim.ToLower());

        if (duplicado)
        {
            return Result<EntidadGubernamentalDto>.Failure(MensajesSistema.EntidadGubernamentalMensajes.REGISTRO_DUPLICADO);
        }

        entidad.Nombre = nombreTrim;
        entidad.Categoria = dto.Categoria.Trim();
        entidad.PoderEstado = dto.PoderEstado.Trim();
        entidad.Sector = dto.Sector.Trim();
        entidad.Siglas = dto.Siglas?.Trim();
        entidad.Direccion = dto.Direccion?.Trim();
        entidad.Telefono = dto.Telefono?.Trim();
        entidad.SitioWeb = dto.SitioWeb?.Trim();
        entidad.Activo = dto.Activo;
        entidad.FechaModificacion = DateTime.UtcNow;
        entidad.UsuarioModificacionId = currentUserId;

        entidad.AddDomainEvent(new Domain.Events.EntidadGubernamentalActualizadaEvent(entidad.Id, entidad.Nombre, currentUserId));
        _unitOfWork.Repository<EntidadGubernamental>().Update(entidad);
        await _unitOfWork.SaveChangesAsync();

        return Result<EntidadGubernamentalDto>.Success(MapToDto(entidad), MensajesSistema.EntidadGubernamentalMensajes.ACTUALIZACION_EXITOSA);
    }

    public async Task<Result<bool>> ToggleEstadoEntidadAsync(int id, int currentUserId)
    {
        var entidad = await _unitOfWork.Repository<EntidadGubernamental>().GetByIdAsync(id);
        if (entidad == null)
        {
            return Result<bool>.Failure(MensajesSistema.EntidadGubernamentalMensajes.ENTIDAD_NO_EXISTE);
        }

        entidad.Activo = !entidad.Activo;
        entidad.FechaModificacion = DateTime.UtcNow;
        entidad.UsuarioModificacionId = currentUserId;

        entidad.AddDomainEvent(new Domain.Events.EntidadGubernamentalEstadoCambiadoEvent(entidad.Id, entidad.Nombre, entidad.Activo, currentUserId));
        _unitOfWork.Repository<EntidadGubernamental>().Update(entidad);
        await _unitOfWork.SaveChangesAsync();

        var estadoStr = entidad.Activo ? "activada" : "desactivada";
        return Result<bool>.Success(entidad.Activo, string.Format(MensajesSistema.EntidadGubernamentalMensajes.ESTADO_CAMBIADO, estadoStr));
    }

    public async Task<Result<bool>> EliminarEntidadAsync(int id, int currentUserId)
    {
        var entidad = await _unitOfWork.Repository<EntidadGubernamental>().GetByIdAsync(id);
        if (entidad == null)
        {
            return Result<bool>.Failure(MensajesSistema.EntidadGubernamentalMensajes.ENTIDAD_NO_EXISTE);
        }

        entidad.Activo = false;
        entidad.FechaModificacion = DateTime.UtcNow;
        entidad.UsuarioModificacionId = currentUserId;

        entidad.AddDomainEvent(new Domain.Events.EntidadGubernamentalEstadoCambiadoEvent(entidad.Id, entidad.Nombre, false, currentUserId));
        _unitOfWork.Repository<EntidadGubernamental>().Update(entidad);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, MensajesSistema.EntidadGubernamentalMensajes.DESACTIVACION_EXITOSA);
    }

    public async Task<Result<List<string>>> GetSectoresAsync()
    {
        var sectores = await _unitOfWork.Repository<EntidadGubernamental>()
            .Query()
            .Select(e => e.Sector)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();

        return Result<List<string>>.Success(sectores);
    }

    public async Task<Result<List<string>>> GetPoderesEstadoAsync()
    {
        var poderes = await _unitOfWork.Repository<EntidadGubernamental>()
            .Query()
            .Select(e => e.PoderEstado)
            .Distinct()
            .OrderBy(p => p)
            .ToListAsync();

        return Result<List<string>>.Success(poderes);
    }

    public async Task<Result<List<string>>> GetCategoriasAsync()
    {
        var categorias = await _unitOfWork.Repository<EntidadGubernamental>()
            .Query()
            .Select(e => e.Categoria)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return Result<List<string>>.Success(categorias);
    }

    private static EntidadGubernamentalDto MapToDto(EntidadGubernamental e)
    {
        return new EntidadGubernamentalDto
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Categoria = e.Categoria,
            PoderEstado = e.PoderEstado,
            Sector = e.Sector,
            Siglas = e.Siglas,
            Direccion = e.Direccion,
            Telefono = e.Telefono,
            SitioWeb = e.SitioWeb,
            Activo = e.Activo,
            FechaCreacion = e.FechaCreacion,
            FechaModificacion = e.FechaModificacion
        };
    }
}
