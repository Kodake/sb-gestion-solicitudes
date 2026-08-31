using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record SolicitudActualizadaEvent(
    int SolicitudId,
    string Codigo,
    string Titulo,
    int SolicitanteId,
    int? ResponsableId,
    int ModificadoPorId
) : INotification;
