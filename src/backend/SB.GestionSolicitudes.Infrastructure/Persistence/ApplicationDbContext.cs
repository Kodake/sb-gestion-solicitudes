using MediatR;
using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Application.Interfaces;
using SB.GestionSolicitudes.Domain.Common;
using SB.GestionSolicitudes.Domain.Entities;

namespace SB.GestionSolicitudes.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator? _mediator;
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator? mediator = null,
        ICurrentUserService? currentUserService = null)
        : base(options)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<TipoSolicitud> TiposSolicitud => Set<TipoSolicitud>();
    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
    public DbSet<HistorialEstado> HistorialesEstado => Set<HistorialEstado>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<EntidadGubernamental> EntidadesGubernamentales => Set<EntidadGubernamental>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario
        modelBuilder.Entity<Usuario>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            b.Property(u => u.Correo).IsRequired().HasMaxLength(100);
            b.HasIndex(u => u.Correo).IsUnique();
            b.Property(u => u.PasswordHash).IsRequired();
            b.Property(u => u.Rol).HasConversion<int>();
        });

        // Area
        modelBuilder.Entity<Area>(b =>
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Nombre).IsRequired().HasMaxLength(100);
            b.Property(a => a.Descripcion).HasMaxLength(250);
        });

        // TipoSolicitud
        modelBuilder.Entity<TipoSolicitud>(b =>
        {
            b.HasKey(t => t.Id);
            b.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            b.Property(t => t.Descripcion).HasMaxLength(250);
        });

        // Solicitud
        modelBuilder.Entity<Solicitud>(b =>
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.Codigo).IsRequired().HasMaxLength(20);
            b.HasIndex(s => s.Codigo).IsUnique();

            b.Property(s => s.Titulo).IsRequired().HasMaxLength(150);
            b.Property(s => s.Descripcion).IsRequired().HasMaxLength(2000);
            b.Property(s => s.Prioridad).HasConversion<int>();
            b.Property(s => s.Estado).HasConversion<int>();
            b.Property(s => s.ReferenciaEvidencia).HasMaxLength(500);

            b.HasOne(s => s.Solicitante)
                .WithMany(u => u.SolicitudesCreadas)
                .HasForeignKey(s => s.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(s => s.Responsable)
                .WithMany(u => u.SolicitudesAsignadas)
                .HasForeignKey(s => s.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(s => s.Area)
                .WithMany(a => a.Solicitudes)
                .HasForeignKey(s => s.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(s => s.TipoSolicitud)
                .WithMany(t => t.Solicitudes)
                .HasForeignKey(s => s.TipoSolicitudId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // HistorialEstado
        modelBuilder.Entity<HistorialEstado>(b =>
        {
            b.HasKey(h => h.Id);
            b.Property(h => h.Comentario).HasMaxLength(1000);
            b.Property(h => h.EstadoAnterior).HasConversion<int>();
            b.Property(h => h.EstadoNuevo).HasConversion<int>();

            b.HasOne(h => h.Solicitud)
                .WithMany(s => s.HistorialesEstado)
                .HasForeignKey(h => h.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(h => h.Usuario)
                .WithMany(u => u.HistorialesEstado)
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Comentario
        modelBuilder.Entity<Comentario>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Texto).IsRequired().HasMaxLength(1000);

            b.HasOne(c => c.Solicitud)
                .WithMany(s => s.Comentarios)
                .HasForeignKey(c => c.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(c => c.Usuario)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Notificacion
        modelBuilder.Entity<Notificacion>(b =>
        {
            b.HasKey(n => n.Id);
            b.Property(n => n.Asunto).IsRequired().HasMaxLength(150);
            b.Property(n => n.Mensaje).IsRequired().HasMaxLength(1000);
            b.Property(n => n.Canal).HasConversion<int>();

            b.HasOne(n => n.Solicitud)
                .WithMany(s => s.Notificaciones)
                .HasForeignKey(n => n.SolicitudId)
                .OnDelete(DeleteBehavior.SetNull);

            b.HasOne(n => n.UsuarioDestino)
                .WithMany()
                .HasForeignKey(n => n.UsuarioDestinoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // EntidadGubernamental
        modelBuilder.Entity<EntidadGubernamental>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Nombre).IsRequired().HasMaxLength(250);
            b.Property(e => e.Categoria).IsRequired().HasMaxLength(150);
            b.Property(e => e.PoderEstado).IsRequired().HasMaxLength(100);
            b.Property(e => e.Sector).IsRequired().HasMaxLength(150);
            b.Property(e => e.Siglas).HasMaxLength(50);
            b.Property(e => e.Direccion).HasMaxLength(300);
            b.Property(e => e.Telefono).HasMaxLength(50);
            b.Property(e => e.SitioWeb).HasMaxLength(200);
            b.HasIndex(e => e.Nombre);
            b.HasIndex(e => e.Sector);
            b.HasIndex(e => e.PoderEstado);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUserService?.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = DateTime.UtcNow;
                if (currentUserId.HasValue && !entry.Entity.UsuarioCreacionId.HasValue)
                {
                    entry.Entity.UsuarioCreacionId = currentUserId.Value;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaModificacion = DateTime.UtcNow;
                if (currentUserId.HasValue)
                {
                    entry.Entity.UsuarioModificacionId = currentUserId.Value;
                }
            }
        }

        // Despachar eventos de dominio antes de guardar para que las notificaciones
        // y efectos colaterales queden registrados dentro de la misma transacción atómica
        if (_mediator != null)
        {
            await DispatchDomainEventsAsync();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator!.Publish(domainEvent);
        }
    }
}
