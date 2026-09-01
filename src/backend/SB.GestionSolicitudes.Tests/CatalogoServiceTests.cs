using Moq;
using SB.GestionSolicitudes.Application.DTOs.Catalogos;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class CatalogoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<Area>> _areaRepoMock;
    private readonly Mock<IRepository<TipoSolicitud>> _tipoRepoMock;
    private readonly CatalogoService _catalogoService;

    public CatalogoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _areaRepoMock = new Mock<IRepository<Area>>();
        _tipoRepoMock = new Mock<IRepository<TipoSolicitud>>();

        _unitOfWorkMock.Setup(u => u.Repository<Area>()).Returns(_areaRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<TipoSolicitud>()).Returns(_tipoRepoMock.Object);

        _catalogoService = new CatalogoService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAreaById_AreaExistente_RetornaExitoso()
    {
        // Arrange
        var areaId = 1;
        var area = new Area
        {
            Id = areaId,
            Nombre = "Tecnología de la Información",
            Descripcion = "Área de soporte IT",
            Activa = true
        };

        _areaRepoMock.Setup(r => r.GetByIdAsync(areaId)).ReturnsAsync(area);

        // Act
        var result = await _catalogoService.GetAreaByIdAsync(areaId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Tecnología de la Información", result.Value!.Nombre);
        Assert.True(result.Value.Activa);
    }

    [Fact]
    public async Task GetAreaById_AreaInexistente_RetornaFallido()
    {
        // Arrange
        _areaRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Area?)null);

        // Act
        var result = await _catalogoService.GetAreaByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }

    [Fact]
    public async Task ToggleEstadoArea_AreaExistente_InvierteEstadoYGuarda()
    {
        // Arrange
        var areaId = 2;
        var area = new Area
        {
            Id = areaId,
            Nombre = "Recursos Humanos",
            Activa = true
        };

        _areaRepoMock.Setup(r => r.GetByIdAsync(areaId)).ReturnsAsync(area);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _catalogoService.ToggleEstadoAreaAsync(areaId, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(area.Activa); // Cambió a false
    }

    [Fact]
    public async Task GetTipoSolicitudById_TipoInexistente_RetornaFallido()
    {
        // Arrange
        _tipoRepoMock.Setup(r => r.GetByIdAsync(888)).ReturnsAsync((TipoSolicitud?)null);

        // Act
        var result = await _catalogoService.GetTipoSolicitudByIdAsync(888);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }

    [Fact]
    public async Task ToggleEstadoTipoSolicitud_TipoExistente_InvierteEstadoYGuarda()
    {
        // Arrange
        var tipoId = 5;
        var tipo = new TipoSolicitud
        {
            Id = tipoId,
            Nombre = "Acceso a Sistemas",
            Activo = false
        };

        _tipoRepoMock.Setup(r => r.GetByIdAsync(tipoId)).ReturnsAsync(tipo);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _catalogoService.ToggleEstadoTipoSolicitudAsync(tipoId, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(tipo.Activo); // Cambió a true
    }
}
