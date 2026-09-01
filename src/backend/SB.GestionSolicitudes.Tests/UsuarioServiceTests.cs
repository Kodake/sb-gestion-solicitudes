using Moq;
using SB.GestionSolicitudes.Application.DTOs.Usuarios;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class UsuarioServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _unitOfWorkMock.Setup(u => u.Usuarios).Returns(_usuarioRepoMock.Object);

        _service = new UsuarioService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CrearUsuario_CorreoDuplicado_RetornaFallido()
    {
        // Arrange
        var dto = new CrearUsuarioDto
        {
            Nombre = "Nuevo Usuario",
            Correo = "admin@sb.gob.do",
            Password = "Password123!",
            Rol = RolEnum.Analista
        };

        var usuarioExistente = new Usuario
        {
            Id = 1,
            Correo = "admin@sb.gob.do",
            Nombre = "Admin"
        };

        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@sb.gob.do"))
            .ReturnsAsync(usuarioExistente);

        // Act
        var result = await _service.CrearUsuarioAsync(dto, currentUserId: 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("ya existe un usuario", result.Message.ToLower());
    }

    [Fact]
    public async Task CrearUsuario_DatosValidos_RetornaExitoso()
    {
        // Arrange
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Torres",
            Correo = "ana.torres@sb.gob.do",
            Password = "Password123!",
            Rol = RolEnum.Analista,
            Activo = true
        };

        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("ana.torres@sb.gob.do"))
            .ReturnsAsync((Usuario?)null);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _service.CrearUsuarioAsync(dto, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Ana Torres", result.Value!.Nombre);
        Assert.Equal("ana.torres@sb.gob.do", result.Value.Correo);
        Assert.Equal(RolEnum.Analista, result.Value.Rol);
    }

    [Fact]
    public async Task ToggleEstadoUsuario_UsuarioExistente_AlternaEstadoYGuarda()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 5,
            Nombre = "Carlos Diaz",
            Correo = "carlos.diaz@sb.gob.do",
            Activo = true
        };

        _usuarioRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(usuario);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _service.ToggleEstadoUsuarioAsync(5, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(usuario.Activo); // Cambiado a inactivo
    }

    [Fact]
    public async Task ActualizarUsuario_UsuarioExistente_ActualizaCamposYGuarda()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 3,
            Nombre = "Pedro Santos",
            Correo = "pedro.santos@sb.gob.do",
            Rol = RolEnum.Solicitante,
            Activo = true
        };

        _usuarioRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(usuario);
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("pedro.actualizado@sb.gob.do")).ReturnsAsync((Usuario?)null);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new ActualizarUsuarioDto
        {
            Nombre = "Pedro Santos Modificado",
            Correo = "pedro.actualizado@sb.gob.do",
            Rol = RolEnum.Analista,
            Activo = true
        };

        // Act
        var result = await _service.ActualizarUsuarioAsync(3, dto, currentUserId: 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Pedro Santos Modificado", usuario.Nombre);
        Assert.Equal(RolEnum.Analista, usuario.Rol);
    }

    [Fact]
    public async Task GetUsuarioById_UsuarioInexistente_RetornaFallido()
    {
        // Arrange
        _usuarioRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usuario?)null);

        // Act
        var result = await _service.GetUsuarioByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("no existe", result.Message.ToLower());
    }
}

