namespace SB.GestionSolicitudes.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
    public UserDto Usuario { get; set; } = null!;
}
