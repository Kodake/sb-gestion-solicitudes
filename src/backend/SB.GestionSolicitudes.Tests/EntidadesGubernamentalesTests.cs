using Moq;
using SB.GestionSolicitudes.Application.DTOs.EntidadesGubernamentales;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class EntidadesGubernamentalesTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<EntidadGubernamental>> _repoMock;
    private readonly EntidadGubernamentalService _service;

    public EntidadesGubernamentalesTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repoMock = new Mock<IRepository<EntidadGubernamental>>();
        _unitOfWorkMock.Setup(u => u.Repository<EntidadGubernamental>()).Returns(_repoMock.Object);

        _service = new EntidadGubernamentalService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetEntidadById_Existe_RetornaExitoso()
    {
        // Arrange
        var entidad = new EntidadGubernamental
        {
            Id = 1,
            Nombre = "Superintendencia de Bancos",
            Siglas = "SB",
            Categoria = "Organismo Descentralizado Funcionalmente",
            PoderEstado = "Poder Ejecutivo",
            Sector = "Finanzas",
            Activo = true
        };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entidad);

        // Act
        var result = await _service.GetEntidadByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Superintendencia de Bancos", result.Value!.Nombre);
        Assert.Equal("SB", result.Value.Siglas);
    }

    [Fact]
    public async Task GetEntidadById_NoExiste_RetornaFallido()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EntidadGubernamental?)null);

        // Act
        var result = await _service.GetEntidadByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }

    [Fact]
    public async Task ToggleEstadoEntidad_InvierteEstadoActivo()
    {
        // Arrange
        var entidad = new EntidadGubernamental
        {
            Id = 1,
            Nombre = "Banco Central",
            Categoria = "Organismo Descentralizado",
            PoderEstado = "Poder Ejecutivo",
            Sector = "Finanzas",
            Activo = true
        };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entidad);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _service.ToggleEstadoEntidadAsync(1, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(entidad.Activo);
    }

    [Fact]
    public async Task ActualizarEntidad_NoExiste_RetornaFallido()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EntidadGubernamental?)null);

        var dto = new ActualizarEntidadGubernamentalDto
        {
            Nombre = "Entidad Inexistente",
            Categoria = "Categoria",
            PoderEstado = "Poder",
            Sector = "Sector"
        };

        // Act
        var result = await _service.ActualizarEntidadAsync(999, dto, currentUserId: 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }

    [Fact]
    public async Task ToggleEstadoEntidad_NoExiste_RetornaFallido()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((EntidadGubernamental?)null);

        // Act
        var result = await _service.ToggleEstadoEntidadAsync(999, currentUserId: 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }
}

