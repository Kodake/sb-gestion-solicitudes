using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record SolicitudCerradaEvent(
    int SolicitudId,
    string Codigo,
    int UsuarioId,
    string ComentarioResolucion
) : INotification;
