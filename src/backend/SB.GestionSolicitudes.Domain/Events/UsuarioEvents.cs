using MediatR;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Domain.Events;

public record UsuarioCreadoEvent(int UsuarioId, string Nombre, string Correo, RolEnum Rol, int CreadoPorId) : INotification;

public record UsuarioEstadoCambiadoEvent(int UsuarioId, string Nombre, bool Activo, int ModificadoPorId) : INotification;

public record UsuarioActualizadoEvent(int UsuarioId, string Nombre, RolEnum Rol, int ModificadoPorId) : INotification;
