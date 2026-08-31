using SB.GestionSolicitudes.Domain.Entities;

namespace SB.GestionSolicitudes.Application.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime Expiracion) GenerateToken(Usuario usuario);
}
