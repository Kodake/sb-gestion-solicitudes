using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record ComentarioAgregadoEvent(
    int SolicitudId,
    string Codigo,
    int AutorId,
    int? SolicitanteId,
    int? ResponsableId,
    string TextoComentario
) : INotification;
