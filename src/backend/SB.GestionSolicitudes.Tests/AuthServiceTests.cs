using Moq;
using SB.GestionSolicitudes.Application.DTOs.Auth;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;
using SB.GestionSolicitudes.Domain.Interfaces;
using SB.GestionSolicitudes.Services;
using Xunit;

namespace SB.GestionSolicitudes.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly Mock<IJwtTokenGenerator> _jwtGeneratorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _jwtGeneratorMock = new Mock<IJwtTokenGenerator>();

        _unitOfWorkMock.Setup(u => u.Usuarios).Returns(_usuarioRepoMock.Object);

        _authService = new AuthService(_unitOfWorkMock.Object, _jwtGeneratorMock.Object);
    }

    [Fact]
    public async Task Login_UsuarioNoExiste_RetornaFallido()
    {
        // Arrange
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var request = new LoginRequestDto { Correo = "inexistente@sb.gob.do", Password = "Pass123!" };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("inválidas", result.Message.ToLower());
    }

    [Fact]
    public async Task Login_CredencialesCorrectas_RetornaToken()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        var usuario = new Usuario
        {
            Id = 1,
            Nombre = "Admin Test",
            Correo = "admin@sb.gob.do",
            PasswordHash = passwordHash,
            Rol = RolEnum.Administrador,
            Activo = true
        };

        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync(usuario.Correo))
            .ReturnsAsync(usuario);

        _jwtGeneratorMock.Setup(j => j.GenerateToken(usuario))
            .Returns(("mocked.jwt.token", DateTime.UtcNow.AddHours(1)));

        var request = new LoginRequestDto { Correo = usuario.Correo, Password = "Admin123!" };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("mocked.jwt.token", result.Value.Token);
    }
}
