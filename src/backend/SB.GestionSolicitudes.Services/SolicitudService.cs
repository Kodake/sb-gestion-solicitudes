using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Comentarios;
using SB.GestionSolicitudes.Application.DTOs.Notificaciones;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Events;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class SolicitudService : ISolicitudService
{
    private readonly IUnitOfWork _unitOfWork;

    public SolicitudService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedList<SolicitudDto>>> GetSolicitudesAsync(FiltroSolicitudesDto filtro, int currentUserId, RolEnum currentRol)
    {
        // Enforce Security Scoping based on Role
        int? filterSolicitanteId = null;
        int? analistaScopeId = null;

        if (currentRol == RolEnum.Solicitante)
        {
            filterSolicitanteId = currentUserId;
        }
        else if (currentRol == RolEnum.Analista && !filtro.ResponsableId.HasValue)
        {
            // Alcance restringido para Analista: solo asignadas a él o disponibles para gestión (sin asignar)
            analistaScopeId = currentUserId;
        }

        var (items, totalCount) = await _unitOfWork.Solicitudes.GetPagedAsync(
            filtro.PageNumber,
            filtro.PageSize,
            filtro.Estado,
            filtro.Prioridad,
            filtro.AreaId,
            filterSolicitanteId,
            filtro.ResponsableId,
            filtro.FechaInicio,
            filtro.FechaFin,
            filtro.SearchTerm,
            analistaScopeId
        );

        var dtos = items.Select(MapToDto).ToList();
        var paginated = new PaginatedList<SolicitudDto>(dtos, totalCount, filtro.PageNumber, filtro.PageSize);

        return Result<PaginatedList<SolicitudDto>>.Success(paginated);
    }

    public async Task<Result<SolicitudDetalleDto>> GetSolicitudByIdAsync(int id, int currentUserId, RolEnum currentRol)
    {
        var solicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        if (solicitud == null)
        {
            return Result<SolicitudDetalleDto>.Failure(MensajesSistema.Solicitud.NO_EXISTE);
        }

        // Security check
        if (currentRol == RolEnum.Solicitante && solicitud.SolicitanteId != currentUserId)
        {
            return Result<SolicitudDetalleDto>.Failure(MensajesSistema.Solicitud.SIN_PERMISOS_CONSULTA);
        }

        var dto = MapToDetalleDto(solicitud, currentRol);
        return Result<SolicitudDetalleDto>.Success(dto);
    }

    public async Task<Result<SolicitudDto>> CrearSolicitudAsync(CrearSolicitudDto dto, int currentUserId)
    {
        var area = await _unitOfWork.Repository<Area>().GetByIdAsync(dto.AreaId);
        if (area == null || !area.Activa)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.AREA_NO_EXISTE_O_INACTIVA);
        }

        var tipo = await _unitOfWork.Repository<TipoSolicitud>().GetByIdAsync(dto.TipoSolicitudId);
        if (tipo == null || !tipo.Activo)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.TIPO_SOLICITUD_NO_EXISTE_O_INACTIVO);
        }

        var codigo = await _unitOfWork.Solicitudes.GenerateNextCodigoAsync();
        var fechaCompromiso = dto.FechaCompromiso ?? DateTime.UtcNow.AddDays(5);

        var solicitud = new Solicitud
        {
            Codigo = codigo,
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Prioridad = dto.Prioridad,
            Estado = EstadoSolicitudEnum.Registrada,
            ReferenciaEvidencia = dto.ReferenciaEvidencia?.Trim(),
            FechaCreacion = DateTime.UtcNow,
            FechaCompromiso = fechaCompromiso,
            SolicitanteId = currentUserId,
            AreaId = dto.AreaId,
            TipoSolicitudId = dto.TipoSolicitudId
        };

        // Initial History
        solicitud.HistorialesEstado.Add(new HistorialEstado
        {
            EstadoAnterior = null,
            EstadoNuevo = EstadoSolicitudEnum.Registrada,
            UsuarioId = currentUserId,
            Comentario = MensajesSistema.Solicitud.HISTORIAL_INICIAL_COMENTARIO,
            Fecha = DateTime.UtcNow
        });

        await _unitOfWork.Solicitudes.AddAsync(solicitud);
        await _unitOfWork.SaveChangesAsync();

        // Domain Event with generated ID
        solicitud.AddDomainEvent(new SolicitudCreadaEvent(solicitud.Id, codigo, solicitud.Titulo, currentUserId));
        await _unitOfWork.SaveChangesAsync();

        var createdSolicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(solicitud.Id);
        return Result<SolicitudDto>.Success(MapToDto(createdSolicitud!), MensajesSistema.Solicitud.REGISTRO_EXITOSO);
    }

    public async Task<Result<SolicitudDto>> ActualizarSolicitudAsync(int id, ActualizarSolicitudDto dto, int currentUserId, RolEnum currentRol)
    {
        var solicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        if (solicitud == null)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.NO_EXISTE);
        }

        var area = await _unitOfWork.Repository<Area>().GetByIdAsync(dto.AreaId);
        if (area == null || !area.Activa)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.AREA_NO_EXISTE_O_INACTIVA);
        }

        var tipo = await _unitOfWork.Repository<TipoSolicitud>().GetByIdAsync(dto.TipoSolicitudId);
        if (tipo == null || !tipo.Activo)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.TIPO_SOLICITUD_NO_EXISTE_O_INACTIVO);
        }

        try
        {
            // Encapsulated in Domain Entity method
            solicitud.ActualizarInformacion(
                dto.Titulo,
                dto.Descripcion,
                dto.AreaId,
                dto.TipoSolicitudId,
                dto.Prioridad,
                dto.FechaCompromiso,
                dto.ReferenciaEvidencia,
                currentUserId,
                currentRol
            );
        }
        catch (InvalidOperationException ex)
        {
            return Result<SolicitudDto>.Failure(ex.Message);
        }

        _unitOfWork.Solicitudes.Update(solicitud);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        return Result<SolicitudDto>.Success(MapToDto(updated!), MensajesSistema.Solicitud.ACTUALIZACION_EXITOSA);
    }

    public async Task<Result<SolicitudDto>> CambiarEstadoAsync(int id, CambiarEstadoDto dto, int currentUserId, RolEnum currentRol)
    {
        var solicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        if (solicitud == null)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.NO_EXISTE);
        }

        // Security check for Solicitante
        if (currentRol == RolEnum.Solicitante && solicitud.SolicitanteId != currentUserId)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.SIN_PERMISOS_MODIFICACION);
        }

        try
        {
            // Validated and mutated via Domain Entity (eliminates domain anemia & enforces legal state transitions)
            solicitud.CambiarEstado(dto.NuevoEstado, currentUserId, currentRol, dto.Comentario);
        }
        catch (InvalidOperationException ex)
        {
            return Result<SolicitudDto>.Failure(ex.Message);
        }

        _unitOfWork.Solicitudes.Update(solicitud);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        return Result<SolicitudDto>.Success(MapToDto(updated!), string.Format(MensajesSistema.Solicitud.ESTADO_ACTUALIZADO, dto.NuevoEstado));
    }

    public async Task<Result<SolicitudDto>> AsignarResponsableAsync(int id, AsignarResponsableDto dto, int currentUserId, RolEnum currentRol)
    {
        if (currentRol != RolEnum.Administrador && currentRol != RolEnum.Analista)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.ASIGNAR_RESPONSABLE_SOLO_ADMIN_ANALISTA);
        }

        var solicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        if (solicitud == null)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.NO_EXISTE);
        }

        var responsable = await _unitOfWork.Usuarios.GetByIdAsync(dto.ResponsableId);
        if (responsable == null)
        {
            return Result<SolicitudDto>.Failure(MensajesSistema.Solicitud.RESPONSABLE_DEBE_SER_ANALISTA_O_ADMIN_ACTIVO);
        }

        try
        {
            // Domain method handles assignment and automatic transition to EnAnalisis if Registrada
            solicitud.AsignarResponsable(responsable, currentUserId, currentRol);
        }
        catch (InvalidOperationException ex)
        {
            return Result<SolicitudDto>.Failure(ex.Message);
        }

        _unitOfWork.Solicitudes.Update(solicitud);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
        return Result<SolicitudDto>.Success(MapToDto(updated!), string.Format(MensajesSistema.Solicitud.ASIGNACION_EXITOSA, responsable.Nombre));
    }

    public async Task<Result<ComentarioDto>> AgregarComentarioAsync(int solicitudId, CrearComentarioDto dto, int currentUserId, RolEnum currentRol)
    {
        var solicitud = await _unitOfWork.Solicitudes.GetByIdAsync(solicitudId);
        if (solicitud == null)
        {
            return Result<ComentarioDto>.Failure(MensajesSistema.Solicitud.NO_EXISTE);
        }

        if (currentRol == RolEnum.Solicitante && solicitud.SolicitanteId != currentUserId)
        {
            return Result<ComentarioDto>.Failure(MensajesSistema.Comentario.SIN_PERMISOS_COMENTAR);
        }

        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(currentUserId);

        var comentario = new Comentario
        {
            SolicitudId = solicitudId,
            UsuarioId = currentUserId,
            Texto = dto.Texto.Trim(),
            EsPublico = dto.EsPublico,
            Fecha = DateTime.UtcNow
        };

        comentario.AddDomainEvent(new Domain.Events.ComentarioAgregadoEvent(
            solicitud.Id,
            solicitud.Codigo,
            currentUserId,
            solicitud.SolicitanteId,
            solicitud.ResponsableId,
            comentario.Texto
        ));

        await _unitOfWork.Repository<Comentario>().AddAsync(comentario);
        await _unitOfWork.SaveChangesAsync();

        var comentarioDto = new ComentarioDto
        {
            Id = comentario.Id,
            SolicitudId = comentario.SolicitudId,
            UsuarioId = comentario.UsuarioId,
            UsuarioNombre = usuario?.Nombre ?? "Usuario",
            UsuarioRol = usuario?.Rol.ToString() ?? string.Empty,
            Texto = comentario.Texto,
            EsPublico = comentario.EsPublico,
            Fecha = comentario.Fecha
        };

        return Result<ComentarioDto>.Success(comentarioDto, MensajesSistema.Comentario.AGREGADO_EXITOSAMENTE);
    }

    private static SolicitudDto MapToDto(Solicitud s)
    {
        return new SolicitudDto
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
        };
    }

    private static SolicitudDetalleDto MapToDetalleDto(Solicitud s, RolEnum currentRol)
    {
        var dto = new SolicitudDetalleDto
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
            TipoSolicitudNombre = s.TipoSolicitud?.Nombre ?? string.Empty,
            HistorialEstados = s.HistorialesEstado.Select(h => new HistorialEstadoDto
            {
                Id = h.Id,
                SolicitudId = h.SolicitudId,
                EstadoAnterior = h.EstadoAnterior,
                EstadoNuevo = h.EstadoNuevo,
                UsuarioId = h.UsuarioId,
                UsuarioNombre = h.Usuario?.Nombre ?? string.Empty,
                Comentario = h.Comentario,
                Fecha = h.Fecha
            }).ToList(),
            Comentarios = s.Comentarios
                .Where(c => currentRol != RolEnum.Solicitante || c.EsPublico) // Solicitante only sees public comments
                .Select(c => new ComentarioDto
                {
                    Id = c.Id,
                    SolicitudId = c.SolicitudId,
                    UsuarioId = c.UsuarioId,
                    UsuarioNombre = c.Usuario?.Nombre ?? string.Empty,
                    UsuarioRol = c.Usuario?.Rol.ToString() ?? string.Empty,
                    Texto = c.Texto,
                    EsPublico = c.EsPublico,
                    Fecha = c.Fecha
                }).ToList(),
            Notificaciones = s.Notificaciones.Select(n => new NotificacionDto
            {
                Id = n.Id,
                SolicitudId = n.SolicitudId,
                UsuarioDestinoId = n.UsuarioDestinoId,
                UsuarioDestinoNombre = n.UsuarioDestino?.Nombre ?? string.Empty,
                Canal = n.Canal,
                Asunto = n.Asunto,
                Mensaje = n.Mensaje,
                Enviado = n.Enviado,
                Fecha = n.Fecha
            }).ToList()
        };

        return dto;
    }
}
