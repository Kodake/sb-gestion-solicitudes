using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Correo.ToLower() == correo.ToLower());
    }
}
