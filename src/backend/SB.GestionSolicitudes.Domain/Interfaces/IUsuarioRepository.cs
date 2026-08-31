using SB.GestionSolicitudes.Domain.Entities;

namespace SB.GestionSolicitudes.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByCorreoAsync(string correo);
}
