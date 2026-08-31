using MediatR;
using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Events;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services.EventHandlers;

public class NotificacionDomainEventHandlers :
    INotificationHandler<SolicitudCreadaEvent>,
    INotificationHandler<SolicitudActualizadaEvent>,
    INotificationHandler<SolicitudAsignadaEvent>,
    INotificationHandler<EstadoSolicitudCambiadoEvent>,
    INotificationHandler<SolicitudCerradaEvent>,
    INotificationHandler<ComentarioAgregadoEvent>,
    INotificationHandler<UsuarioCreadoEvent>,
    INotificationHandler<UsuarioEstadoCambiadoEvent>,
    INotificationHandler<UsuarioActualizadoEvent>,
    INotificationHandler<EntidadGubernamentalCreadaEvent>,
    INotificationHandler<EntidadGubernamentalActualizadaEvent>,
    INotificationHandler<EntidadGubernamentalEstadoCambiadoEvent>,
    INotificationHandler<CatalogoItemCreadoEvent>,
    INotificationHandler<CatalogoItemActualizadoEvent>,
    INotificationHandler<CatalogoItemEstadoCambiadoEvent>
{
    private readonly INotificacionService _notificacionService;
    private readonly IUnitOfWork _unitOfWork;

    public NotificacionDomainEventHandlers(
        INotificacionService notificacionService,
        IUnitOfWork unitOfWork)
    {
        _notificacionService = notificacionService;
        _unitOfWork = unitOfWork;
    }

    // --- MÓDULO SOLICITUDES ---
    public async Task Handle(SolicitudCreadaEvent notification, CancellationToken cancellationToken)
    {
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.SolicitanteId,
            solicitudId: notification.SolicitudId,
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_SOLICITUD_CREADA, notification.Codigo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_SOLICITUD_CREADA, notification.Titulo, notification.Codigo)
        );
    }

    public async Task Handle(SolicitudActualizadaEvent notification, CancellationToken cancellationToken)
    {
        var asunto = string.Format(MensajesSistema.Notificaciones.ASUNTO_SOLICITUD_ACTUALIZADA, notification.Codigo);
        var mensaje = string.Format(MensajesSistema.Notificaciones.MENSAJE_SOLICITUD_ACTUALIZADA, notification.Codigo, notification.Titulo);

        // 1. Notificar al solicitante
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.SolicitanteId,
            solicitudId: notification.SolicitudId,
            asunto: asunto,
            mensaje: mensaje
        );

        // 2. Notificar al responsable si existe y es diferente al solicitante
        if (notification.ResponsableId.HasValue && notification.ResponsableId.Value != notification.SolicitanteId)
        {
            await _notificacionService.EnviarNotificacionAsync(
                usuarioDestinoId: notification.ResponsableId.Value,
                solicitudId: notification.SolicitudId,
                asunto: asunto,
                mensaje: mensaje
            );
        }

        // 3. Notificar a administradores
        await NotificarAdministradoresAsync(asunto, mensaje);
    }

    public async Task Handle(SolicitudAsignadaEvent notification, CancellationToken cancellationToken)
    {
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.ResponsableId,
            solicitudId: notification.SolicitudId,
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_SOLICITUD_ASIGNADA, notification.Codigo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_SOLICITUD_ASIGNADA, notification.Codigo)
        );
    }

    public async Task Handle(EstadoSolicitudCambiadoEvent notification, CancellationToken cancellationToken)
    {
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.UsuarioId,
            solicitudId: notification.SolicitudId,
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_ESTADO_CAMBIADO, notification.Codigo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ESTADO_CAMBIADO, notification.Codigo, notification.EstadoNuevo, notification.Comentario)
        );
    }

    public async Task Handle(SolicitudCerradaEvent notification, CancellationToken cancellationToken)
    {
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.UsuarioId,
            solicitudId: notification.SolicitudId,
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_SOLICITUD_CERRADA, notification.Codigo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_SOLICITUD_CERRADA, notification.Codigo, notification.ComentarioResolucion)
        );
    }

    public async Task Handle(ComentarioAgregadoEvent notification, CancellationToken cancellationToken)
    {
        var extracto = notification.TextoComentario.Length > 80
            ? notification.TextoComentario[..80] + "..."
            : notification.TextoComentario;

        // Si el autor es el solicitante y hay responsable, notificar al responsable
        if (notification.SolicitanteId.HasValue && notification.AutorId == notification.SolicitanteId.Value)
        {
            if (notification.ResponsableId.HasValue && notification.ResponsableId.Value != notification.AutorId)
            {
                await _notificacionService.EnviarNotificacionAsync(
                    usuarioDestinoId: notification.ResponsableId.Value,
                    solicitudId: notification.SolicitudId,
                    asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_NUEVO_COMENTARIO, notification.Codigo),
                    mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_NUEVO_COMENTARIO, notification.Codigo, extracto)
                );
            }
        }
        else if (notification.SolicitanteId.HasValue && notification.SolicitanteId.Value != notification.AutorId)
        {
            // Si el autor es otro usuario (analista/admin), notificar al solicitante
            await _notificacionService.EnviarNotificacionAsync(
                usuarioDestinoId: notification.SolicitanteId.Value,
                solicitudId: notification.SolicitudId,
                asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_NUEVO_COMENTARIO, notification.Codigo),
                mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_NUEVO_COMENTARIO, notification.Codigo, extracto)
            );
        }
    }

    // --- MÓDULO USUARIOS ---
    public async Task Handle(UsuarioCreadoEvent notification, CancellationToken cancellationToken)
    {
        // 1. Bienvenida al nuevo usuario
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.UsuarioId,
            solicitudId: null,
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_BIENVENIDA_USUARIO, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_BIENVENIDA_USUARIO, notification.Rol)
        );

        // 2. Alerta a todos los administradores
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_USUARIO_REGISTRADO_ADMIN, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_USUARIO_REGISTRADO_ADMIN, notification.Nombre, notification.Correo, notification.Rol)
        );
    }

    public async Task Handle(UsuarioEstadoCambiadoEvent notification, CancellationToken cancellationToken)
    {
        var estadoStr = notification.Activo ? "activado" : "desactivado";
        var estadoCap = notification.Activo ? "Activado" : "Desactivado";

        // 1. Alerta al usuario afectado
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.UsuarioId,
            solicitudId: null,
            asunto: MensajesSistema.Notificaciones.ASUNTO_ESTADO_USUARIO_CAMBIADO,
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ESTADO_USUARIO_CAMBIADO, notification.Activo ? "Activo" : "Inactivo")
        );

        // 2. Alerta a Administradores
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_ESTADO_USUARIO_ADMIN, estadoCap, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ESTADO_USUARIO_ADMIN, notification.Nombre, estadoStr)
        );
    }

    public async Task Handle(UsuarioActualizadoEvent notification, CancellationToken cancellationToken)
    {
        // 1. Alerta al usuario afectado
        await _notificacionService.EnviarNotificacionAsync(
            usuarioDestinoId: notification.UsuarioId,
            solicitudId: null,
            asunto: MensajesSistema.Notificaciones.ASUNTO_PERFIL_USUARIO_ACTUALIZADO,
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_PERFIL_USUARIO_ACTUALIZADO, notification.Rol)
        );

        // 2. Alerta a Administradores
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_USUARIO_ACTUALIZADO_ADMIN, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_USUARIO_ACTUALIZADO_ADMIN, notification.Nombre, notification.Rol)
        );
    }

    // --- MÓDULO ENTIDADES GUBERNAMENTALES ---
    public async Task Handle(EntidadGubernamentalCreadaEvent notification, CancellationToken cancellationToken)
    {
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_ENTIDAD_CREADA, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ENTIDAD_CREADA, notification.Nombre, notification.Sector)
        );
    }

    public async Task Handle(EntidadGubernamentalActualizadaEvent notification, CancellationToken cancellationToken)
    {
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_ENTIDAD_MODIFICADA, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ENTIDAD_MODIFICADA, notification.Nombre)
        );
    }

    public async Task Handle(EntidadGubernamentalEstadoCambiadoEvent notification, CancellationToken cancellationToken)
    {
        var estadoStr = notification.Activo ? "activada" : "desactivada";
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_ENTIDAD_ESTADO_CAMBIADO, notification.Nombre),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_ENTIDAD_ESTADO_CAMBIADO, notification.Nombre, estadoStr)
        );
    }

    // --- MÓDULO CATÁLOGOS ---
    public async Task Handle(CatalogoItemCreadoEvent notification, CancellationToken cancellationToken)
    {
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_CATALOGO_CREADO, notification.TipoCatalogo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_CATALOGO_CREADO, notification.TipoCatalogo, notification.Nombre)
        );
    }

    public async Task Handle(CatalogoItemActualizadoEvent notification, CancellationToken cancellationToken)
    {
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_CATALOGO_MODIFICADO, notification.TipoCatalogo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_CATALOGO_MODIFICADO, notification.Nombre, notification.TipoCatalogo)
        );
    }

    public async Task Handle(CatalogoItemEstadoCambiadoEvent notification, CancellationToken cancellationToken)
    {
        var estadoStr = notification.Activo ? "activado" : "desactivado";
        await NotificarAdministradoresAsync(
            asunto: string.Format(MensajesSistema.Notificaciones.ASUNTO_CATALOGO_ESTADO_CAMBIADO, notification.TipoCatalogo),
            mensaje: string.Format(MensajesSistema.Notificaciones.MENSAJE_CATALOGO_ESTADO_CAMBIADO, notification.Nombre, notification.TipoCatalogo, estadoStr)
        );
    }

    // --- HELPER PARA NOTIFICAR A ADMINISTRADORES ---
    private async Task NotificarAdministradoresAsync(string asunto, string mensaje, int? usuarioExcluidoId = null)
    {
        try
        {
            var adminIds = await _unitOfWork.Usuarios.Query()
                .Where(u => u.Activo && u.Rol == RolEnum.Administrador)
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var adminId in adminIds)
            {
                if (usuarioExcluidoId.HasValue && adminId == usuarioExcluidoId.Value) continue;

                await _notificacionService.EnviarNotificacionAsync(
                    usuarioDestinoId: adminId,
                    solicitudId: null,
                    asunto: asunto,
                    mensaje: mensaje
                );
            }
        }
        catch
        {
            // Ignorar fallas secundarias de consulta para evitar bloquear la transacción principal
        }
    }
}
