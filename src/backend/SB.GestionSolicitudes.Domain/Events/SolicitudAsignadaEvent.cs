using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record SolicitudAsignadaEvent(
    int SolicitudId,
    string Codigo,
    int ResponsableId,
    int AsignadoPorUsuarioId
) : INotification;
