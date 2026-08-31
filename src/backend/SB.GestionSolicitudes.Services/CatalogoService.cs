using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Catalogos;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class CatalogoService : ICatalogoService
{
    private readonly IUnitOfWork _unitOfWork;

    public CatalogoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AreaDto>>> GetAreasAsync(bool soloActivas = false)
    {
        var query = _unitOfWork.Repository<Area>().Query();
        if (soloActivas)
        {
            query = query.Where(a => a.Activa);
        }

        var items = await query
            .OrderBy(a => a.Nombre)
            .Select(a => new AreaDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                Activa = a.Activa
            })
            .ToListAsync();

        return Result<List<AreaDto>>.Success(items);
    }

    public async Task<Result<AreaDto>> GetAreaByIdAsync(int id)
    {
        var area = await _unitOfWork.Repository<Area>().GetByIdAsync(id);
        if (area == null)
        {
            return Result<AreaDto>.Failure(MensajesSistema.Catalogo.AREA_NO_ENCONTRADA);
        }

        return Result<AreaDto>.Success(new AreaDto
        {
            Id = area.Id,
            Nombre = area.Nombre,
            Descripcion = area.Descripcion,
            Activa = area.Activa
        });
    }

    public async Task<Result<AreaDto>> CrearAreaAsync(CrearAreaDto dto, int currentUserId)
    {
        var nombreTrim = dto.Nombre.Trim();
        var existe = await _unitOfWork.Repository<Area>()
            .Query()
            .AnyAsync(a => a.Nombre.ToLower() == nombreTrim.ToLower());

        if (existe)
        {
            return Result<AreaDto>.Failure(MensajesSistema.Catalogo.REGISTRO_DUPLICADO);
        }

        var area = new Area
        {
            Nombre = nombreTrim,
            Descripcion = dto.Descripcion?.Trim(),
            Activa = dto.Activa,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCreacionId = currentUserId
        };

        await _unitOfWork.Repository<Area>().AddAsync(area);
        await _unitOfWork.SaveChangesAsync();

        area.AddDomainEvent(new Domain.Events.CatalogoItemCreadoEvent("Área", area.Id, area.Nombre, currentUserId));
        await _unitOfWork.SaveChangesAsync();

        return Result<AreaDto>.Success(new AreaDto
        {
            Id = area.Id,
            Nombre = area.Nombre,
            Descripcion = area.Descripcion,
            Activa = area.Activa
        }, MensajesSistema.Catalogo.AREA_CREADA_EXITOSAMENTE);
    }

    public async Task<Result<AreaDto>> ActualizarAreaAsync(int id, ActualizarAreaDto dto, int currentUserId)
    {
        var area = await _unitOfWork.Repository<Area>().GetByIdAsync(id);
        if (area == null)
        {
            return Result<AreaDto>.Failure(MensajesSistema.Catalogo.AREA_NO_ENCONTRADA);
        }

        var nombreTrim = dto.Nombre.Trim();
        var existe = await _unitOfWork.Repository<Area>()
            .Query()
            .AnyAsync(a => a.Id != id && a.Nombre.ToLower() == nombreTrim.ToLower());

        if (existe)
        {
            return Result<AreaDto>.Failure(MensajesSistema.Catalogo.REGISTRO_DUPLICADO);
        }

        area.Nombre = nombreTrim;
        area.Descripcion = dto.Descripcion?.Trim();
        area.Activa = dto.Activa;
        area.FechaModificacion = DateTime.UtcNow;
        area.UsuarioModificacionId = currentUserId;

        area.AddDomainEvent(new Domain.Events.CatalogoItemActualizadoEvent("Área", area.Id, area.Nombre, currentUserId));
        _unitOfWork.Repository<Area>().Update(area);
        await _unitOfWork.SaveChangesAsync();

        return Result<AreaDto>.Success(new AreaDto
        {
            Id = area.Id,
            Nombre = area.Nombre,
            Descripcion = area.Descripcion,
            Activa = area.Activa
        }, MensajesSistema.Catalogo.AREA_ACTUALIZADA_EXITOSAMENTE);
    }

    public async Task<Result<bool>> ToggleEstadoAreaAsync(int id, int currentUserId)
    {
        var area = await _unitOfWork.Repository<Area>().GetByIdAsync(id);
        if (area == null)
        {
            return Result<bool>.Failure(MensajesSistema.Catalogo.AREA_NO_ENCONTRADA);
        }

        area.Activa = !area.Activa;
        area.FechaModificacion = DateTime.UtcNow;
        area.UsuarioModificacionId = currentUserId;

        area.AddDomainEvent(new Domain.Events.CatalogoItemEstadoCambiadoEvent("Área", area.Id, area.Nombre, area.Activa, currentUserId));
        _unitOfWork.Repository<Area>().Update(area);
        await _unitOfWork.SaveChangesAsync();

        var estadoStr = area.Activa ? "activada" : "desactivada";
        return Result<bool>.Success(area.Activa, string.Format(MensajesSistema.Catalogo.AREA_ESTADO_CAMBIADO, estadoStr));
    }

    public async Task<Result<List<TipoSolicitudDto>>> GetTiposSolicitudAsync(bool soloActivas = false)
    {
        var query = _unitOfWork.Repository<TipoSolicitud>().Query();
        if (soloActivas)
        {
            query = query.Where(t => t.Activo);
        }

        var items = await query
            .OrderBy(t => t.Nombre)
            .Select(t => new TipoSolicitudDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                Activo = t.Activo
            })
            .ToListAsync();

        return Result<List<TipoSolicitudDto>>.Success(items);
    }

    public async Task<Result<TipoSolicitudDto>> GetTipoSolicitudByIdAsync(int id)
    {
        var tipo = await _unitOfWork.Repository<TipoSolicitud>().GetByIdAsync(id);
        if (tipo == null)
        {
            return Result<TipoSolicitudDto>.Failure(MensajesSistema.Catalogo.TIPO_NO_ENCONTRADO);
        }

        return Result<TipoSolicitudDto>.Success(new TipoSolicitudDto
        {
            Id = tipo.Id,
            Nombre = tipo.Nombre,
            Descripcion = tipo.Descripcion,
            Activo = tipo.Activo
        });
    }

    public async Task<Result<TipoSolicitudDto>> CrearTipoSolicitudAsync(CrearTipoSolicitudDto dto, int currentUserId)
    {
        var nombreTrim = dto.Nombre.Trim();
        var existe = await _unitOfWork.Repository<TipoSolicitud>()
            .Query()
            .AnyAsync(t => t.Nombre.ToLower() == nombreTrim.ToLower());

        if (existe)
        {
            return Result<TipoSolicitudDto>.Failure(MensajesSistema.Catalogo.REGISTRO_DUPLICADO);
        }

        var tipo = new TipoSolicitud
        {
            Nombre = nombreTrim,
            Descripcion = dto.Descripcion?.Trim(),
            Activo = dto.Activo,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCreacionId = currentUserId
        };

        await _unitOfWork.Repository<TipoSolicitud>().AddAsync(tipo);
        await _unitOfWork.SaveChangesAsync();

        tipo.AddDomainEvent(new Domain.Events.CatalogoItemCreadoEvent("Tipo de Solicitud", tipo.Id, tipo.Nombre, currentUserId));
        await _unitOfWork.SaveChangesAsync();

        return Result<TipoSolicitudDto>.Success(new TipoSolicitudDto
        {
            Id = tipo.Id,
            Nombre = tipo.Nombre,
            Descripcion = tipo.Descripcion,
            Activo = tipo.Activo
        }, MensajesSistema.Catalogo.TIPO_CREADO_EXITOSAMENTE);
    }

    public async Task<Result<TipoSolicitudDto>> ActualizarTipoSolicitudAsync(int id, ActualizarTipoSolicitudDto dto, int currentUserId)
    {
        var tipo = await _unitOfWork.Repository<TipoSolicitud>().GetByIdAsync(id);
        if (tipo == null)
        {
            return Result<TipoSolicitudDto>.Failure(MensajesSistema.Catalogo.TIPO_NO_ENCONTRADO);
        }

        var nombreTrim = dto.Nombre.Trim();
        var existe = await _unitOfWork.Repository<TipoSolicitud>()
            .Query()
            .AnyAsync(t => t.Id != id && t.Nombre.ToLower() == nombreTrim.ToLower());

        if (existe)
        {
            return Result<TipoSolicitudDto>.Failure(MensajesSistema.Catalogo.REGISTRO_DUPLICADO);
        }

        tipo.Nombre = nombreTrim;
        tipo.Descripcion = dto.Descripcion?.Trim();
        tipo.Activo = dto.Activo;
        tipo.FechaModificacion = DateTime.UtcNow;
        tipo.UsuarioModificacionId = currentUserId;

        tipo.AddDomainEvent(new Domain.Events.CatalogoItemActualizadoEvent("Tipo de Solicitud", tipo.Id, tipo.Nombre, currentUserId));
        _unitOfWork.Repository<TipoSolicitud>().Update(tipo);
        await _unitOfWork.SaveChangesAsync();

        return Result<TipoSolicitudDto>.Success(new TipoSolicitudDto
        {
            Id = tipo.Id,
            Nombre = tipo.Nombre,
            Descripcion = tipo.Descripcion,
            Activo = tipo.Activo
        }, MensajesSistema.Catalogo.TIPO_ACTUALIZADO_EXITOSAMENTE);
    }

    public async Task<Result<bool>> ToggleEstadoTipoSolicitudAsync(int id, int currentUserId)
    {
        var tipo = await _unitOfWork.Repository<TipoSolicitud>().GetByIdAsync(id);
        if (tipo == null)
        {
            return Result<bool>.Failure(MensajesSistema.Catalogo.TIPO_NO_ENCONTRADO);
        }

        tipo.Activo = !tipo.Activo;
        tipo.FechaModificacion = DateTime.UtcNow;
        tipo.UsuarioModificacionId = currentUserId;

        tipo.AddDomainEvent(new Domain.Events.CatalogoItemEstadoCambiadoEvent("Tipo de Solicitud", tipo.Id, tipo.Nombre, tipo.Activo, currentUserId));
        _unitOfWork.Repository<TipoSolicitud>().Update(tipo);
        await _unitOfWork.SaveChangesAsync();

        var estadoStr = tipo.Activo ? "activado" : "desactivado";
        return Result<bool>.Success(tipo.Activo, string.Format(MensajesSistema.Catalogo.TIPO_ESTADO_CAMBIADO, estadoStr));
    }
}
