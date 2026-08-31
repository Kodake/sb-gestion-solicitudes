using SB.GestionSolicitudes.Application.Common;
using SB.GestionSolicitudes.Application.DTOs.Auth;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<UserDto>> GetCurrentUserAsync(int userId);
}
