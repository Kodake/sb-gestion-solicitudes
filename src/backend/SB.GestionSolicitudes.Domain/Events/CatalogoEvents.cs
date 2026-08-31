using MediatR;

namespace SB.GestionSolicitudes.Domain.Events;

public record CatalogoItemCreadoEvent(string TipoCatalogo, int ItemId, string Nombre, int CreadoPorId) : INotification;

public record CatalogoItemActualizadoEvent(string TipoCatalogo, int ItemId, string Nombre, int ModificadoPorId) : INotification;

public record CatalogoItemEstadoCambiadoEvent(string TipoCatalogo, int ItemId, string Nombre, bool Activo, int ModificadoPorId) : INotification;
