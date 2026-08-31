using MediatR;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Events;

public record EstadoSolicitudCambiadoEvent(
    int SolicitudId,
    string Codigo,
    EstadoSolicitudEnum? EstadoAnterior,
    EstadoSolicitudEnum EstadoNuevo,
    int UsuarioId,
    string Comentario
) : INotification;
