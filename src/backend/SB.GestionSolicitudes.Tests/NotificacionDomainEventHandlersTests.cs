using Moq;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Events;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services.EventHandlers;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class NotificacionDomainEventHandlersTests
{
    private readonly Mock<INotificacionService> _notificacionServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly NotificacionDomainEventHandlers _handler;

    public NotificacionDomainEventHandlersTests()
    {
        _notificacionServiceMock = new Mock<INotificacionService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _usuarioRepoMock = new Mock<IUsuarioRepository>();

        _unitOfWorkMock.Setup(u => u.Usuarios).Returns(_usuarioRepoMock.Object);

        _handler = new NotificacionDomainEventHandlers(
            _notificacionServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_UsuarioCreadoEvent_EnviaBienvenidaAlUsuario()
    {
        // Arrange
        var evt = new UsuarioCreadoEvent(15, "Carlos Peña", "carlos@sb.gob.do", RolEnum.Solicitante, 1);
        _usuarioRepoMock.Setup(r => r.Query()).Returns(new List<Usuario>().AsQueryable());

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _notificacionServiceMock.Verify(n => n.EnviarNotificacionAsync(
            15,
            null,
            It.Is<string>(s => s.Contains("Carlos Peña")),
            It.Is<string>(m => m.Contains("Solicitante")),
            CanalNotificacionEnum.Database
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioEstadoCambiadoEvent_EnviaNotificacionAlUsuario()
    {
        // Arrange
        var evt = new UsuarioEstadoCambiadoEvent(25, "Laura Gómez", false, 1);

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _notificacionServiceMock.Verify(n => n.EnviarNotificacionAsync(
            25,
            null,
            MensajesSistema.Notificaciones.ASUNTO_ESTADO_USUARIO_CAMBIADO,
            It.Is<string>(m => m.Contains("Inactivo")),
            CanalNotificacionEnum.Database
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_UsuarioActualizadoEvent_EnviaNotificacionAlUsuario()
    {
        // Arrange
        var evt = new UsuarioActualizadoEvent(30, "Marcos Ruiz", RolEnum.Analista, 1);

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _notificacionServiceMock.Verify(n => n.EnviarNotificacionAsync(
            30,
            null,
            MensajesSistema.Notificaciones.ASUNTO_PERFIL_USUARIO_ACTUALIZADO,
            It.Is<string>(m => m.Contains("Analista")),
            CanalNotificacionEnum.Database
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ComentarioAgregado_PorSolicitante_NotificaAlResponsable()
    {
        // Arrange
        var evt = new ComentarioAgregadoEvent(
            SolicitudId: 100,
            Codigo: "SOL-2026-001",
            AutorId: 5,
            SolicitanteId: 5,
            ResponsableId: 9,
            TextoComentario: "Por favor revisar el archivo adjunto"
        );

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _notificacionServiceMock.Verify(n => n.EnviarNotificacionAsync(
            9,
            100,
            It.Is<string>(s => s.Contains("SOL-2026-001")),
            It.Is<string>(m => m.Contains("Por favor revisar")),
            CanalNotificacionEnum.Database
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ComentarioAgregado_PorAnalista_NotificaAlSolicitante()
    {
        // Arrange
        var evt = new ComentarioAgregadoEvent(
            SolicitudId: 100,
            Codigo: "SOL-2026-001",
            AutorId: 9,
            SolicitanteId: 5,
            ResponsableId: 9,
            TextoComentario: "Se ha validado la documentación correctamente"
        );

        // Act
        await _handler.Handle(evt, CancellationToken.None);

        // Assert
        _notificacionServiceMock.Verify(n => n.EnviarNotificacionAsync(
            5,
            100,
            It.Is<string>(s => s.Contains("SOL-2026-001")),
            It.Is<string>(m => m.Contains("validado")),
            CanalNotificacionEnum.Database
        ), Times.Once);
    }
}
