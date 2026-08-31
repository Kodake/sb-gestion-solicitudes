using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Usuarios;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedList<UsuarioDto>>> GetUsuariosAsync(FiltroUsuariosDto filtro)
    {
        var query = _unitOfWork.Usuarios.Query();

        if (filtro.Rol.HasValue)
        {
            query = query.Where(u => u.Rol == filtro.Rol.Value);
        }

        if (filtro.Activo.HasValue)
        {
            query = query.Where(u => u.Activo == filtro.Activo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.SearchTerm))
        {
            var term = filtro.SearchTerm.Trim().ToLower();
            query = query.Where(u => u.Nombre.ToLower().Contains(term) || u.Correo.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.Nombre)
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ToListAsync();

        var dtos = items.Select(MapToDto).ToList();
        var paginated = new PaginatedList<UsuarioDto>(dtos, totalCount, filtro.PageNumber, filtro.PageSize);

        return Result<PaginatedList<UsuarioDto>>.Success(paginated);
    }

    public async Task<Result<UsuarioDto>> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<UsuarioDto>.Failure(MensajesSistema.UsuarioMensajes.USUARIO_NO_EXISTE);
        }

        return Result<UsuarioDto>.Success(MapToDto(usuario));
    }

    public async Task<Result<UsuarioDto>> CrearUsuarioAsync(CrearUsuarioDto dto, int currentUserId)
    {
        var emailClean = dto.Correo.Trim().ToLower();
        var existente = await _unitOfWork.Usuarios.GetByCorreoAsync(emailClean);
        if (existente != null)
        {
            return Result<UsuarioDto>.Failure(MensajesSistema.UsuarioMensajes.CORREO_DUPLICADO);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Correo = emailClean,
            PasswordHash = passwordHash,
            Rol = dto.Rol,
            Activo = dto.Activo,
            FechaCreacion = DateTime.UtcNow,
            UsuarioCreacionId = currentUserId
        };

        await _unitOfWork.Usuarios.AddAsync(usuario);
        await _unitOfWork.SaveChangesAsync();

        usuario.AddDomainEvent(new Domain.Events.UsuarioCreadoEvent(usuario.Id, usuario.Nombre, usuario.Correo, usuario.Rol, currentUserId));
        await _unitOfWork.SaveChangesAsync();

        return Result<UsuarioDto>.Success(MapToDto(usuario), MensajesSistema.UsuarioMensajes.CREACION_EXITOSA);
    }

    public async Task<Result<UsuarioDto>> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto, int currentUserId)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<UsuarioDto>.Failure(MensajesSistema.UsuarioMensajes.USUARIO_NO_EXISTE);
        }

        var emailClean = dto.Correo.Trim().ToLower();
        if (!string.Equals(usuario.Correo, emailClean, StringComparison.OrdinalIgnoreCase))
        {
            var existente = await _unitOfWork.Usuarios.GetByCorreoAsync(emailClean);
            if (existente != null && existente.Id != id)
            {
                return Result<UsuarioDto>.Failure(MensajesSistema.UsuarioMensajes.CORREO_DUPLICADO);
            }
        }

        usuario.Nombre = dto.Nombre.Trim();
        usuario.Correo = emailClean;
        usuario.Rol = dto.Rol;
        usuario.Activo = dto.Activo;
        usuario.FechaModificacion = DateTime.UtcNow;
        usuario.UsuarioModificacionId = currentUserId;

        if (!string.IsNullOrWhiteSpace(dto.NuevoPassword))
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevoPassword);
        }

        usuario.AddDomainEvent(new Domain.Events.UsuarioActualizadoEvent(usuario.Id, usuario.Nombre, usuario.Rol, currentUserId));
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveChangesAsync();

        return Result<UsuarioDto>.Success(MapToDto(usuario), MensajesSistema.UsuarioMensajes.ACTUALIZACION_EXITOSA);
    }

    public async Task<Result<bool>> ToggleEstadoUsuarioAsync(int id, int currentUserId)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<bool>.Failure(MensajesSistema.UsuarioMensajes.USUARIO_NO_EXISTE);
        }

        usuario.Activo = !usuario.Activo;
        usuario.FechaModificacion = DateTime.UtcNow;
        usuario.UsuarioModificacionId = currentUserId;

        usuario.AddDomainEvent(new Domain.Events.UsuarioEstadoCambiadoEvent(usuario.Id, usuario.Nombre, usuario.Activo, currentUserId));
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveChangesAsync();

        var estadoStr = usuario.Activo ? "activado" : "desactivado";
        return Result<bool>.Success(usuario.Activo, string.Format(MensajesSistema.UsuarioMensajes.ESTADO_CAMBIADO, estadoStr));
    }

    public async Task<Result<bool>> EliminarUsuarioAsync(int id, int currentUserId)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuario == null)
        {
            return Result<bool>.Failure(MensajesSistema.UsuarioMensajes.USUARIO_NO_EXISTE);
        }

        // Logical delete by deactivating or remove
        usuario.Activo = false;
        usuario.FechaModificacion = DateTime.UtcNow;
        usuario.UsuarioModificacionId = currentUserId;

        usuario.AddDomainEvent(new Domain.Events.UsuarioEstadoCambiadoEvent(usuario.Id, usuario.Nombre, false, currentUserId));
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, MensajesSistema.UsuarioMensajes.DESACTIVACION_EXITOSA);
    }

    private static UsuarioDto MapToDto(Usuario u)
    {
        return new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Correo = u.Correo,
            Rol = u.Rol,
            Activo = u.Activo,
            FechaCreacion = u.FechaCreacion,
            FechaModificacion = u.FechaModificacion
        };
    }
}
