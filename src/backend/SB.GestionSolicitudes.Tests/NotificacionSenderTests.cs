using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Infrastructure.Notifications;
using SB.GestionSolicitudes.Infrastructure.Persistence;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class NotificacionSenderTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<NotificacionSender>> _loggerMock;
    private readonly NotificacionSender _service;

    public NotificacionSenderTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();

        _loggerMock = new Mock<ILogger<NotificacionSender>>();
        _service = new NotificacionSender(_context, _loggerMock.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task EnviarNotificacion_GuardaNotificacionEnBaseDeDatos()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 1,
            Nombre = "Administrador General",
            Correo = "admin@sb.gob.do",
            PasswordHash = "hash",
            Rol = RolEnum.Administrador,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Act
        await _service.EnviarNotificacionAsync(
            usuarioDestinoId: 1,
            solicitudId: null,
            asunto: "Nueva Solicitud",
            mensaje: "Se ha registrado una solicitud de prueba",
            canal: CanalNotificacionEnum.Database
        );

        // Assert
        var notificaciones = await _context.Notificaciones.ToListAsync();
        Assert.Single(notificaciones);
        Assert.Equal("Nueva Solicitud", notificaciones[0].Asunto);
        Assert.Equal(1, notificaciones[0].UsuarioDestinoId);
        Assert.True(notificaciones[0].Enviado);
    }

    [Fact]
    public async Task GetNotificacionesUsuario_RetornaNotificacionesDelUsuario()
    {
        // Arrange
        var usuario1 = new Usuario
        {
            Id = 10,
            Nombre = "Usuario 10",
            Correo = "user10@sb.gob.do",
            PasswordHash = "hash",
            Rol = RolEnum.Solicitante,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
        var usuario2 = new Usuario
        {
            Id = 20,
            Nombre = "Usuario 20",
            Correo = "user20@sb.gob.do",
            PasswordHash = "hash",
            Rol = RolEnum.Solicitante,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
        _context.Usuarios.AddRange(usuario1, usuario2);

        _context.Notificaciones.AddRange(
            new Notificacion
            {
                UsuarioDestinoId = 10,
                Asunto = "Alerta 1",
                Mensaje = "Mensaje 1",
                Canal = CanalNotificacionEnum.Database,
                Enviado = true,
                Fecha = DateTime.UtcNow
            },
            new Notificacion
            {
                UsuarioDestinoId = 20,
                Asunto = "Alerta de otro usuario",
                Mensaje = "Mensaje 2",
                Canal = CanalNotificacionEnum.Database,
                Enviado = true,
                Fecha = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetNotificacionesUsuarioAsync(10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!);
        Assert.Equal("Alerta 1", result.Value![0].Asunto);
        Assert.Equal("Usuario 10", result.Value[0].UsuarioDestinoNombre);
    }
}
