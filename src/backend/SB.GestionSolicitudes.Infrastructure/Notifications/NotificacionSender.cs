using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Notificaciones;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Infrastructure.Persistence;

namespace SB.GestionSolicitudes.Infrastructure.Notifications;

public class NotificacionSender : INotificacionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificacionSender> _logger;

    public NotificacionSender(ApplicationDbContext context, ILogger<NotificacionSender> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task EnviarNotificacionAsync(int usuarioDestinoId, int? solicitudId, string asunto, string mensaje, CanalNotificacionEnum canal = CanalNotificacionEnum.Database)
    {
        var targetSolicitudId = (solicitudId.HasValue && solicitudId.Value > 0) ? solicitudId.Value : (int?)null;

        var notificacion = new Notificacion
        {
            UsuarioDestinoId = usuarioDestinoId,
            SolicitudId = targetSolicitudId,
            Asunto = asunto,
            Mensaje = mensaje,
            Canal = canal,
            Enviado = true,
            Fecha = DateTime.UtcNow
        };

        _context.Notificaciones.Add(notificacion);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            // If already within an active SaveChanges cycle, it will be committed with the main entity
        }

        _logger.LogInformation(
            MensajesSistema.Notificaciones.LOG_NOTIFICACION_ENVIADA,
            canal, usuarioDestinoId, asunto, mensaje
        );
    }

    public async Task<Result<IReadOnlyList<NotificacionDto>>> GetNotificacionesUsuarioAsync(int usuarioId)
    {
        var notificaciones = await _context.Notificaciones
            .Include(n => n.UsuarioDestino)
            .Where(n => n.UsuarioDestinoId == usuarioId)
            .OrderByDescending(n => n.Fecha)
            .Select(n => new NotificacionDto
            {
                Id = n.Id,
                SolicitudId = n.SolicitudId,
                UsuarioDestinoId = n.UsuarioDestinoId,
                UsuarioDestinoNombre = n.UsuarioDestino.Nombre,
                Canal = n.Canal,
                Asunto = n.Asunto,
                Mensaje = n.Mensaje,
                Enviado = n.Enviado,
                Fecha = n.Fecha
            })
            .ToListAsync();

        return Result<IReadOnlyList<NotificacionDto>>.Success(notificaciones);
    }

    public async Task<Result<bool>> EliminarNotificacionAsync(int notificacionId, int usuarioId)
    {
        var notificacion = await _context.Notificaciones
            .FirstOrDefaultAsync(n => n.Id == notificacionId && n.UsuarioDestinoId == usuarioId);

        if (notificacion == null)
        {
            return Result<bool>.Failure("La notificación no existe o no pertenece al usuario.");
        }

        _context.Notificaciones.Remove(notificacion);
        await _context.SaveChangesAsync();

        return Result<bool>.Success(true, "Notificación eliminada correctamente.");
    }

    public async Task<Result<bool>> LimpiarNotificacionesUsuarioAsync(int usuarioId)
    {
        var notificaciones = await _context.Notificaciones
            .Where(n => n.UsuarioDestinoId == usuarioId)
            .ToListAsync();

        if (notificaciones.Any())
        {
            _context.Notificaciones.RemoveRange(notificaciones);
            await _context.SaveChangesAsync();
        }

        return Result<bool>.Success(true, "Todas las notificaciones han sido eliminadas.");
    }
}
