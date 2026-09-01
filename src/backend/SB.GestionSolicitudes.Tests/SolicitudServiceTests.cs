using Moq;
using SB.GestionSolicitudes.Application.DTOs.Solicitudes;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class SolicitudServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISolicitudRepository> _solicitudRepoMock;
    private readonly SolicitudService _solicitudService;

    public SolicitudServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _solicitudRepoMock = new Mock<ISolicitudRepository>();
        _unitOfWorkMock.Setup(u => u.Solicitudes).Returns(_solicitudRepoMock.Object);

        _solicitudService = new SolicitudService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CambiarEstado_ACerradaSinComentario_RetornaFallido()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.EnProgreso,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId))
            .ReturnsAsync(solicitud);

        var dto = new CambiarEstadoDto
        {
            NuevoEstado = EstadoSolicitudEnum.Cerrada,
            Comentario = "" // Sin comentario
        };

        // Act
        var result = await _solicitudService.CambiarEstadoAsync(solicitudId, dto, currentUserId: 1, currentRol: RolEnum.Administrador);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("comentario", result.Message.ToLower());
    }

    [Fact]
    public async Task CambiarEstado_TransicionInvalida_RetornaFallido()
    {
        // Arrange: Intentar pasar de Resuelta a Registrada (transición ilegal)
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.Resuelta,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId))
            .ReturnsAsync(solicitud);

        var dto = new CambiarEstadoDto
        {
            NuevoEstado = EstadoSolicitudEnum.Registrada,
            Comentario = "Intento ilegal de retroceder a registrada"
        };

        // Act
        var result = await _solicitudService.CambiarEstadoAsync(solicitudId, dto, currentUserId: 1, currentRol: RolEnum.Administrador);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("transición inválida", result.Message.ToLower());
    }

    [Fact]
    public async Task CambiarEstado_ACerradaConComentario_RetornaExitoso()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.Resuelta,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId))
            .ReturnsAsync(solicitud);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var dto = new CambiarEstadoDto
        {
            NuevoEstado = EstadoSolicitudEnum.Cerrada,
            Comentario = "Se verificó la solución correctamente."
        };

        // Act
        var result = await _solicitudService.CambiarEstadoAsync(solicitudId, dto, currentUserId: 1, currentRol: RolEnum.Administrador);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EstadoSolicitudEnum.Cerrada, solicitud.Estado);
    }

    [Fact]
    public async Task ReabrirSolicitudCerrada_PorSolicitante_RetornaFallido()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.Cerrada,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId))
            .ReturnsAsync(solicitud);

        var dto = new CambiarEstadoDto
        {
            NuevoEstado = EstadoSolicitudEnum.EnProgreso,
            Comentario = "Reabrir solicitud por inconveniente"
        };

        // Act (Rol Solicitante intenta reabrir)
        var result = await _solicitudService.CambiarEstadoAsync(solicitudId, dto, currentUserId: 3, currentRol: RolEnum.Solicitante);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("solo un administrador o analista", result.Message.ToLower());
    }

    [Fact]
    public async Task ActualizarSolicitud_PorSolicitanteEnEstadoRegistrada_RetornaExitoso()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Titulo = "Título Original",
            Descripcion = "Descripción Original",
            Estado = EstadoSolicitudEnum.Registrada,
            SolicitanteId = 3,
            AreaId = 1,
            TipoSolicitudId = 1,
            Prioridad = PrioridadEnum.Media
        };

        var area = new Area { Id = 1, Nombre = "TI", Activa = true };
        var tipo = new TipoSolicitud { Id = 1, Nombre = "Soporte", Activo = true };

        var areaRepoMock = new Mock<IRepository<Area>>();
        areaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(area);

        var tipoRepoMock = new Mock<IRepository<TipoSolicitud>>();
        tipoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tipo);

        _unitOfWorkMock.Setup(u => u.Repository<Area>()).Returns(areaRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<TipoSolicitud>()).Returns(tipoRepoMock.Object);
        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId)).ReturnsAsync(solicitud);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new ActualizarSolicitudDto
        {
            Titulo = "Título Modificado",
            Descripcion = "Descripción Modificada",
            AreaId = 1,
            TipoSolicitudId = 1,
            Prioridad = PrioridadEnum.Alta,
            ReferenciaEvidencia = "Evidencia 123"
        };

        // Act
        var result = await _solicitudService.ActualizarSolicitudAsync(solicitudId, dto, currentUserId: 3, currentRol: RolEnum.Solicitante);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Título Modificado", solicitud.Titulo);
        Assert.Equal(PrioridadEnum.Alta, solicitud.Prioridad);
    }

    [Fact]
    public async Task AsignarResponsable_PorAnalista_AsignaCorrectamente()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.EnAnalisis,
            SolicitanteId = 3,
            ResponsableId = null
        };

        var analista = new Usuario
        {
            Id = 2,
            Nombre = "Laura Analista",
            Rol = RolEnum.Analista,
            Activo = true
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId)).ReturnsAsync(solicitud);
        _unitOfWorkMock.Setup(u => u.Usuarios.GetByIdAsync(2)).ReturnsAsync(analista);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new AsignarResponsableDto
        {
            ResponsableId = 2,
            Comentario = "Asignación para análisis de seguridad"
        };

        // Act
        var result = await _solicitudService.AsignarResponsableAsync(solicitudId, dto, currentUserId: 1, currentRol: RolEnum.Administrador);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, solicitud.ResponsableId);
    }

    [Fact]
    public async Task ActualizarSolicitud_EnEstadoResuelta_PorSolicitante_RetornaFallido()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.Resuelta,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId)).ReturnsAsync(solicitud);

        var area = new Area { Id = 1, Nombre = "TI", Activa = true };
        var tipo = new TipoSolicitud { Id = 1, Nombre = "Soporte", Activo = true };

        var areaRepoMock = new Mock<IRepository<Area>>();
        areaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(area);

        var tipoRepoMock = new Mock<IRepository<TipoSolicitud>>();
        tipoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tipo);

        _unitOfWorkMock.Setup(u => u.Repository<Area>()).Returns(areaRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<TipoSolicitud>()).Returns(tipoRepoMock.Object);

        var dto = new ActualizarSolicitudDto
        {
            Titulo = "Intento de edición cuando ya está resuelta",
            Descripcion = "Descripción",
            AreaId = 1,
            TipoSolicitudId = 1,
            Prioridad = PrioridadEnum.Baja
        };

        // Act
        var result = await _solicitudService.ActualizarSolicitudAsync(solicitudId, dto, currentUserId: 3, currentRol: RolEnum.Solicitante);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no se puede modificar", result.Message.ToLower());
    }

    [Fact]
    public async Task CambiarEstado_DeRegistradaAEnAnalisis_RetornaExitoso()
    {
        // Arrange
        var solicitudId = 1;
        var solicitud = new Solicitud
        {
            Id = solicitudId,
            Codigo = "SOL-2026-0001",
            Estado = EstadoSolicitudEnum.Registrada,
            SolicitanteId = 3
        };

        _solicitudRepoMock.Setup(r => r.GetByIdWithDetailsAsync(solicitudId)).ReturnsAsync(solicitud);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new CambiarEstadoDto
        {
            NuevoEstado = EstadoSolicitudEnum.EnAnalisis,
            Comentario = "Inicio de revisión técnica"
        };

        // Act
        var result = await _solicitudService.CambiarEstadoAsync(solicitudId, dto, currentUserId: 2, currentRol: RolEnum.Analista);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EstadoSolicitudEnum.EnAnalisis, solicitud.Estado);
    }
}

