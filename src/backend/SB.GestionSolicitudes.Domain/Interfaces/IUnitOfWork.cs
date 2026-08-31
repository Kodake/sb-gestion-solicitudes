namespace SB.GestionSolicitudes.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISolicitudRepository Solicitudes { get; }
    IUsuarioRepository Usuarios { get; }
    IRepository<T> Repository<T>() where T : Common.BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
