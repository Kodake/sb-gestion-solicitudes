using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record EntidadGubernamentalCreadaEvent(int EntidadId, string Nombre, string Sector, int CreadoPorId) : INotification;

public record EntidadGubernamentalActualizadaEvent(int EntidadId, string Nombre, int ModificadoPorId) : INotification;

public record EntidadGubernamentalEstadoCambiadoEvent(int EntidadId, string Nombre, bool Activo, int ModificadoPorId) : INotification;
