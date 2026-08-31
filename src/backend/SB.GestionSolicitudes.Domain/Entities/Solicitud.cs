using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Events;
using SB.GestionSolicitudes.Domain.Services;

namespace SB.GestionSolicitudes.Domain.Entities;

public class Solicitud : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public PrioridadEnum Prioridad { get; set; } = PrioridadEnum.Media;
    public EstadoSolicitudEnum Estado { get; set; } = EstadoSolicitudEnum.Registrada;
    public string? ReferenciaEvidencia { get; set; }
    public DateTime FechaCompromiso { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    // Foreign Keys
    public int SolicitanteId { get; set; }
    public int? ResponsableId { get; set; }
    public int AreaId { get; set; }
    public int TipoSolicitudId { get; set; }

    // Navigation properties
    public Usuario Solicitante { get; set; } = null!;
    public Usuario? Responsable { get; set; }
    public Area Area { get; set; } = null!;
    public TipoSolicitud TipoSolicitud { get; set; } = null!;

    public ICollection<HistorialEstado> HistorialesEstado { get; set; } = new List<HistorialEstado>();
    public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    // Domain Methods - Encapsulando Reglas de Negocio (Eliminación de Anemia de Dominio)
    public void CambiarEstado(EstadoSolicitudEnum nuevoEstado, int currentUserId, RolEnum currentRol, string? comentario)
    {
        if (Estado == nuevoEstado) return;

        // 1. Validar Transición en la Matriz
        if (!SolicitudTransiciones.EsTransicionValida(Estado, nuevoEstado))
        {
            throw new InvalidOperationException(string.Format(MensajesDominio.SolicitudReglas.TRANSICION_INVALIDA, Estado, nuevoEstado));
        }

        // 2. Regla de reapertura de solicitud cerrada
        if (Estado == EstadoSolicitudEnum.Cerrada)
        {
            if (currentRol != RolEnum.Administrador && currentRol != RolEnum.Analista)
            {
                throw new InvalidOperationException(MensajesDominio.SolicitudReglas.REAPERTURA_PERMISOS);
            }
        }

        // 3. Regla de cierre: Requiere comentario obligatorio
        if (nuevoEstado == EstadoSolicitudEnum.Cerrada && string.IsNullOrWhiteSpace(comentario))
        {
            throw new InvalidOperationException(MensajesDominio.SolicitudReglas.CIERRE_REQUIERE_COMENTARIO);
        }

        var estadoAnterior = Estado;
        Estado = nuevoEstado;
        FechaActualizacion = DateTime.UtcNow;

        HistorialesEstado.Add(new HistorialEstado
        {
            SolicitudId = Id,
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = nuevoEstado,
            UsuarioId = currentUserId,
            Comentario = string.IsNullOrWhiteSpace(comentario) ? string.Format(MensajesDominio.SolicitudReglas.CAMBIO_ESTADO_HISTORIAL, nuevoEstado) : comentario.Trim(),
            Fecha = DateTime.UtcNow
        });

        if (nuevoEstado == EstadoSolicitudEnum.Cerrada)
        {
            AddDomainEvent(new SolicitudCerradaEvent(Id, Codigo, currentUserId, comentario!.Trim()));
        }
        else
        {
            AddDomainEvent(new EstadoSolicitudCambiadoEvent(Id, Codigo, estadoAnterior, nuevoEstado, currentUserId, comentario ?? string.Empty));
        }
    }

    public void AsignarResponsable(Usuario responsable, int asignadoPorUsuarioId, RolEnum asignadoPorRol)
    {
        if (asignadoPorRol != RolEnum.Administrador && asignadoPorRol != RolEnum.Analista)
        {
            throw new InvalidOperationException(MensajesDominio.SolicitudReglas.ASIGNAR_RESPONSABLE_PERMISOS);
        }

        if (!responsable.Activo || (responsable.Rol != RolEnum.Analista && responsable.Rol != RolEnum.Administrador))
        {
            throw new InvalidOperationException(MensajesDominio.SolicitudReglas.RESPONSABLE_DEBE_SER_ANALISTA_O_ADMIN);
        }

        ResponsableId = responsable.Id;
        Responsable = responsable;
        FechaActualizacion = DateTime.UtcNow;

        // Si estaba en Registrada, avanza automáticamente a EnAnalisis
        if (Estado == EstadoSolicitudEnum.Registrada)
        {
            var estadoAnt = Estado;
            Estado = EstadoSolicitudEnum.EnAnalisis;
            HistorialesEstado.Add(new HistorialEstado
            {
                SolicitudId = Id,
                EstadoAnterior = estadoAnt,
                EstadoNuevo = EstadoSolicitudEnum.EnAnalisis,
                UsuarioId = asignadoPorUsuarioId,
                Comentario = string.Format(MensajesDominio.SolicitudReglas.ASIGNACION_HISTORIAL, responsable.Nombre),
                Fecha = DateTime.UtcNow
            });
        }

        AddDomainEvent(new SolicitudAsignadaEvent(Id, Codigo, responsable.Id, asignadoPorUsuarioId));
    }

    public void ActualizarInformacion(string titulo, string descripcion, int areaId, int tipoSolicitudId, PrioridadEnum prioridad, DateTime? fechaCompromiso, string? referenciaEvidencia, int usuarioId, RolEnum rol)
    {
        // Regla: Solicitante solo puede editar si está en Registrada o EnEsperaDelSolicitante
        if (rol == RolEnum.Solicitante)
        {
            if (SolicitanteId != usuarioId)
            {
                throw new InvalidOperationException(MensajesDominio.SolicitudReglas.MODIFICAR_SOLICITUD_AJENA);
            }

            if (Estado != EstadoSolicitudEnum.Registrada && Estado != EstadoSolicitudEnum.EnEsperaDelSolicitante)
            {
                throw new InvalidOperationException(string.Format(MensajesDominio.SolicitudReglas.MODIFICAR_ESTADO_NO_PERMITIDO, Estado));
            }
        }
        else if (rol != RolEnum.Administrador && rol != RolEnum.Analista)
        {
            throw new InvalidOperationException(MensajesDominio.SolicitudReglas.MODIFICAR_PERMISOS_INSUFICIENTES);
        }

        Titulo = titulo.Trim();
        Descripcion = descripcion.Trim();
        AreaId = areaId;
        TipoSolicitudId = tipoSolicitudId;
        Prioridad = prioridad;
        if (fechaCompromiso.HasValue)
        {
            FechaCompromiso = fechaCompromiso.Value;
        }
        ReferenciaEvidencia = referenciaEvidencia?.Trim();
        FechaActualizacion = DateTime.UtcNow;

        AddDomainEvent(new SolicitudActualizadaEvent(Id, Codigo, Titulo, SolicitanteId, ResponsableId, usuarioId));
    }
}
