using System.Collections.Concurrent;
using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Interfaces;

namespace SB.GestionSolicitudes.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public ISolicitudRepository Solicitudes { get; }
    public IUsuarioRepository Usuarios { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Solicitudes = new SolicitudRepository(_context);
        Usuarios = new UsuarioRepository(_context);
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        return (IRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new Repository<T>(_context));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
