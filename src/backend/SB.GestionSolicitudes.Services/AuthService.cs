using BCrypt.Net;
using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Auth;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Result<LoginResponseDto>.Failure(MensajesSistema.Auth.CREDENCIALES_REQUERIDAS);
        }

        var usuario = await _unitOfWork.Usuarios.GetByCorreoAsync(request.Correo.Trim());

        if (usuario == null)
        {
            return Result<LoginResponseDto>.Failure(MensajesSistema.Auth.CREDENCIALES_INVALIDAS);
        }

        if (!usuario.Activo)
        {
            return Result<LoginResponseDto>.Failure(string.Format(MensajesSistema.Auth.USUARIO_DESACTIVADO, usuario.Nombre));
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);
        if (!isPasswordValid)
        {
            return Result<LoginResponseDto>.Failure(MensajesSistema.Auth.CREDENCIALES_INCORRECTAS);
        }

        var (token, expiracion) = _jwtTokenGenerator.GenerateToken(usuario);

        var response = new LoginResponseDto
        {
            Token = token,
            Expiracion = expiracion,
            Usuario = new UserDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                Activo = usuario.Activo
            }
        };

        return Result<LoginResponseDto>.Success(response, MensajesSistema.Auth.LOGIN_EXITOSO);
    }

    public async Task<Result<UserDto>> GetCurrentUserAsync(int userId)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(userId);
        if (usuario == null || !usuario.Activo)
        {
            return Result<UserDto>.Failure(MensajesSistema.Auth.USUARIO_NO_ENCONTRADO);
        }

        var dto = new UserDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol,
            Activo = usuario.Activo
        };

        return Result<UserDto>.Success(dto);
    }
}
