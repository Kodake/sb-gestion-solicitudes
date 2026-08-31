using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record SolicitudCreadaEvent(
    int SolicitudId,
    string Codigo,
    string Titulo,
    int SolicitanteId
) : INotification;
