using Microsoft.EntityFrameworkCore;
using SB.GestionSolicitudes.Domain.Entities;
using SB.GestionSolicitudes.Domain.Enums;

namespace SB.GestionSolicitudes.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Usuarios Iniciales
        if (!await context.Usuarios.AnyAsync())
        {
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var analistaPasswordHash = BCrypt.Net.BCrypt.HashPassword("Analista123!");
            var solicitantePasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!");

            var admin = new Usuario
            {
                Nombre = "Administrador del Sistema",
                Correo = "admin@sb.gob.do",
                PasswordHash = adminPasswordHash,
                Rol = RolEnum.Administrador,
                Activo = true
            };

            var analista = new Usuario
            {
                Nombre = "Carlos Mendoza (Analista IT)",
                Correo = "analista.tech@sb.gob.do",
                PasswordHash = analistaPasswordHash,
                Rol = RolEnum.Analista,
                Activo = true
            };

            var solicitante = new Usuario
            {
                Nombre = "Juan Pérez",
                Correo = "juan.perez@sb.gob.do",
                PasswordHash = solicitantePasswordHash,
                Rol = RolEnum.Solicitante,
                Activo = true
            };

            context.Usuarios.AddRange(admin, analista, solicitante);
            await context.SaveChangesAsync();
        }

        // 2. Áreas Institucionales
        if (!await context.Areas.AnyAsync())
        {
            var areas = new[]
            {
                new Area { Nombre = "Tecnología de la Información", Descripcion = "División de TI y Canales", Activa = true },
                new Area { Nombre = "Finanzas y Contabilidad", Descripcion = "Departamento Financiero", Activa = true },
                new Area { Nombre = "Recursos Humanos", Descripcion = "Gestión del Talento Humano", Activa = true },
                new Area { Nombre = "Operaciones Bancarias", Descripcion = "División Operativa", Activa = true },
                new Area { Nombre = "Legal y Cumplimiento", Descripcion = "Asesoría Jurídica", Activa = true }
            };

            context.Areas.AddRange(areas);
            await context.SaveChangesAsync();
        }

        // 3. Tipos de Solicitud
        if (!await context.TiposSolicitud.AnyAsync())
        {
            var tipos = new[]
            {
                new TipoSolicitud { Nombre = "Soporte Técnico", Descripcion = "Asistencia en Hardware/Software", Activo = true },
                new TipoSolicitud { Nombre = "Desarrollo de Software", Descripcion = "Nuevos requerimientos y ajustes", Activo = true },
                new TipoSolicitud { Nombre = "Acceso a Sistemas", Descripcion = "Permisos y credenciales", Activo = true },
                new TipoSolicitud { Nombre = "Mantenimiento de Hardware", Descripcion = "Reparaciones de equipos", Activo = true }
            };

            context.TiposSolicitud.AddRange(tipos);
            await context.SaveChangesAsync();
        }

        // 4. Entidades Gubernamentales Oficiales de RD (181 entidades según ListaEntidadesGubernamentales.xlsx)
        if (!await context.EntidadesGubernamentales.AnyAsync())
        {
            var entidades = GetEntidadesGubernamentalesSeed();
            context.EntidadesGubernamentales.AddRange(entidades);
            await context.SaveChangesAsync();
        }

        // 5. Solicitudes de Prueba Iniciales
        if (!await context.Solicitudes.AnyAsync())
        {
            var admin = await context.Usuarios.FirstAsync(u => u.Rol == RolEnum.Administrador);
            var analista = await context.Usuarios.FirstAsync(u => u.Rol == RolEnum.Analista);
            var solicitante = await context.Usuarios.FirstAsync(u => u.Rol == RolEnum.Solicitante);

            var areaIT = await context.Areas.FirstAsync(a => a.Nombre == "Tecnología de la Información");
            var areaFinanzas = await context.Areas.FirstAsync(a => a.Nombre == "Finanzas y Contabilidad");
            var areaOperaciones = await context.Areas.FirstAsync(a => a.Nombre == "Operaciones Bancarias");

            var tipoAccesos = await context.TiposSolicitud.FirstAsync(t => t.Nombre == "Acceso a Sistemas");
            var tipoSoporte = await context.TiposSolicitud.FirstAsync(t => t.Nombre == "Soporte Técnico");
            var tipoDesarrollo = await context.TiposSolicitud.FirstAsync(t => t.Nombre == "Desarrollo de Software");
            var tipoMantenimiento = await context.TiposSolicitud.FirstAsync(t => t.Nombre == "Mantenimiento de Hardware");

            var solicitud1 = new Solicitud
            {
                Codigo = "SOL-2026-0001",
                Titulo = "Acceso al módulo de reportes consolidados",
                Descripcion = "Se requiere asignar permisos de lectura en la plataforma de reportes bancarios para el nuevo analista.",
                Prioridad = PrioridadEnum.Alta,
                Estado = EstadoSolicitudEnum.EnProgreso,
                ReferenciaEvidencia = "https://intranet.sb.gob.do/ticket-ref/8821",
                FechaCreacion = DateTime.UtcNow.AddDays(-3),
                FechaCompromiso = DateTime.UtcNow.AddDays(2),
                SolicitanteId = solicitante.Id,
                ResponsableId = analista.Id,
                AreaId = areaIT.Id,
                TipoSolicitudId = tipoAccesos.Id
            };

            var solicitud2 = new Solicitud
            {
                Codigo = "SOL-2026-0002",
                Titulo = "Impresora de área no responde",
                Descripcion = "La impresora HP LaserJet del departamento de Finanzas no recibe trabajos de impresión.",
                Prioridad = PrioridadEnum.Media,
                Estado = EstadoSolicitudEnum.Registrada,
                ReferenciaEvidencia = "Piso 3 - Oficina 304",
                FechaCreacion = DateTime.UtcNow.AddHours(-5),
                FechaCompromiso = DateTime.UtcNow.AddDays(1),
                SolicitanteId = solicitante.Id,
                ResponsableId = null,
                AreaId = areaFinanzas.Id,
                TipoSolicitudId = tipoSoporte.Id
            };

            var solicitud3 = new Solicitud
            {
                Codigo = "SOL-2026-0003",
                Titulo = "Ajuste en cálculo de comisiones",
                Descripcion = "Se solicita modificar el algoritmo de cálculo de comisiones conforme a la nueva normativa SB-2026.",
                Prioridad = PrioridadEnum.Critica,
                Estado = EstadoSolicitudEnum.Resuelta,
                ReferenciaEvidencia = "Resolución SB-2026-04",
                FechaCreacion = DateTime.UtcNow.AddDays(-10),
                FechaCompromiso = DateTime.UtcNow.AddDays(-2),
                SolicitanteId = solicitante.Id,
                ResponsableId = analista.Id,
                AreaId = areaIT.Id,
                TipoSolicitudId = tipoDesarrollo.Id
            };

            var solicitud4 = new Solicitud
            {
                Codigo = "SOL-2026-0004",
                Titulo = "Reemplazo de batería de UPS",
                Descripcion = "UPS emitía sonido constante de alerta de batería agotada en la terminal 12.",
                Prioridad = PrioridadEnum.Baja,
                Estado = EstadoSolicitudEnum.Cerrada,
                ReferenciaEvidencia = "Inventario #9921",
                FechaCreacion = DateTime.UtcNow.AddDays(-15),
                FechaCompromiso = DateTime.UtcNow.AddDays(-12),
                SolicitanteId = solicitante.Id,
                ResponsableId = analista.Id,
                AreaId = areaOperaciones.Id,
                TipoSolicitudId = tipoMantenimiento.Id
            };

            context.Solicitudes.AddRange(solicitud1, solicitud2, solicitud3, solicitud4);
            await context.SaveChangesAsync();

            // 6. Historiales de Estado
            context.HistorialesEstado.AddRange(
                new HistorialEstado
                {
                    SolicitudId = solicitud1.Id,
                    EstadoAnterior = null,
                    EstadoNuevo = EstadoSolicitudEnum.Registrada,
                    UsuarioId = solicitante.Id,
                    Comentario = "Solicitud creada por el usuario.",
                    Fecha = DateTime.UtcNow.AddDays(-3)
                },
                new HistorialEstado
                {
                    SolicitudId = solicitud1.Id,
                    EstadoAnterior = EstadoSolicitudEnum.Registrada,
                    EstadoNuevo = EstadoSolicitudEnum.EnProgreso,
                    UsuarioId = analista.Id,
                    Comentario = "Solicitud tomada por el analista y puesta en progreso.",
                    Fecha = DateTime.UtcNow.AddDays(-2)
                },
                new HistorialEstado
                {
                    SolicitudId = solicitud4.Id,
                    EstadoAnterior = EstadoSolicitudEnum.Resuelta,
                    EstadoNuevo = EstadoSolicitudEnum.Cerrada,
                    UsuarioId = admin.Id,
                    Comentario = "Se verificó el reemplazo satisfactorio del UPS. Solicitud cerrada oficialmente.",
                    Fecha = DateTime.UtcNow.AddDays(-12)
                }
            );

            // 7. Comentarios
            context.Comentarios.AddRange(
                new Comentario
                {
                    SolicitudId = solicitud1.Id,
                    UsuarioId = analista.Id,
                    Texto = "Se han revisado las políticas de acceso. En proceso de asignación de perfil.",
                    EsPublico = true,
                    Fecha = DateTime.UtcNow.AddDays(-2)
                },
                new Comentario
                {
                    SolicitudId = solicitud4.Id,
                    UsuarioId = admin.Id,
                    Texto = "Batería reemplazada por el proveedor de soporte técnico.",
                    EsPublico = true,
                    Fecha = DateTime.UtcNow.AddDays(-12)
                }
            );

            // 8. Notificaciones
            context.Notificaciones.AddRange(
                new Notificacion
                {
                    SolicitudId = solicitud1.Id,
                    UsuarioDestinoId = solicitante.Id,
                    Canal = CanalNotificacionEnum.Database,
                    Asunto = "Solicitud Asignada: SOL-2026-0001",
                    Mensaje = "Su solicitud 'Acceso al módulo de reportes consolidados' ha sido asignada a Carlos Mendoza (Analista IT).",
                    Enviado = true,
                    Fecha = DateTime.UtcNow.AddDays(-2)
                }
            );

            await context.SaveChangesAsync();
        }
    }

    private static List<EntidadGubernamental> GetEntidadesGubernamentalesSeed()
    {
        return new List<EntidadGubernamental>
        {
            new() { Nombre = "Superintendencia de Bancos de la República Dominicana", Siglas = "SB", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Financiero", SitioWeb = "https://sb.gob.do", Activo = true },
            new() { Nombre = "Banco Central de la República Dominicana", Siglas = "BCRD", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Financiero", SitioWeb = "https://bancentral.gov.do", Activo = true },
            new() { Nombre = "Dirección General de Impuestos Internos", Siglas = "DGII", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://dgii.gov.do", Activo = true },
            new() { Nombre = "Dirección General de Aduanas", Siglas = "DGA", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://aduanas.gob.do", Activo = true },
            new() { Nombre = "Tesorería Nacional", Siglas = "TN", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://tesoreria.gob.do", Activo = true },
            new() { Nombre = "Superintendencia del Mercado de Valores", Siglas = "SIMV", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://simv.gob.do", Activo = true },
            new() { Nombre = "Superintendencia de Seguros", Siglas = "SIS", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://superseguros.gob.do", Activo = true },
            new() { Nombre = "Superintendencia de Salud y Riesgos Laborales", Siglas = "SISALRIL", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Trabajo", SitioWeb = "https://sisalril.gob.do", Activo = true },
            new() { Nombre = "Unidad de Análisis Financiero", Siglas = "UAF", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://uaf.gob.do", Activo = true },
            new() { Nombre = "Contraloría General de la República", Siglas = "CGR", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Control Interno", SitioWeb = "https://contraloria.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Hacienda y Economía", Siglas = "MH", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://hacienda.gob.do", Activo = true },
            new() { Nombre = "Ministerio de la Presidencia", Siglas = "MINPRE", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Presidencia", SitioWeb = "https://minpre.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Administración Pública", Siglas = "MAP", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Administración Pública", SitioWeb = "https://map.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Industria, Comercio y Mipymes", Siglas = "MICM", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Industria, Comercio y MIPYMES", SitioWeb = "https://micm.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Educación", Siglas = "MINERD", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Educación", SitioWeb = "https://ministeriodeeducacion.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Educación Superior, Ciencia y Tecnología", Siglas = "MESCyT", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Educación Superior, Ciencia y Tecnología", SitioWeb = "https://mescyt.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Salud Pública y Asistencia Social", Siglas = "MISPAS", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Salud", SitioWeb = "https://msp.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Obras Públicas y Comunicaciones", Siglas = "MOPC", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Obras Públicas y Comunicaciones", SitioWeb = "https://mopc.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Trabajo", Siglas = "MT", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Trabajo", SitioWeb = "https://mt.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Turismo", Siglas = "MITUR", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Turismo", SitioWeb = "https://mitur.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Medio Ambiente y Recursos Naturales", Siglas = "MIMARENA", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Medio Ambiente y Recursos Naturales", SitioWeb = "https://ambiente.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Interior y Policía", Siglas = "MIP", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Interior y Policía", SitioWeb = "https://mip.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Defensa", Siglas = "MIDE", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Defensa", SitioWeb = "https://mide.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Relaciones Exteriores", Siglas = "MIREX", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Relaciones Exteriores", SitioWeb = "https://mirex.gob.do", Activo = true },
            new() { Nombre = "Ministerio de la Mujer", Siglas = "MMujer", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Mujer", SitioWeb = "https://mujer.gob.do", Activo = true },
            new() { Nombre = "Ministerio de la Juventud", Siglas = "MJ", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Juventud", SitioWeb = "https://juventud.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Deportes y Recreación", Siglas = "MIDEREC", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Deportes y Recreación", SitioWeb = "https://miderec.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Cultura", Siglas = "MINC", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Cultura", SitioWeb = "https://cultura.gob.do", Activo = true },
            new() { Nombre = "Ministerio de Energía y Minas", Siglas = "MEM", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Energía y Minas", SitioWeb = "https://mem.gob.do", Activo = true },
            new() { Nombre = "Ministerio de la Vivienda, Hábitat y Edificaciones", Siglas = "MIVHED", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Vivienda, Hábitat y Edificaciones", SitioWeb = "https://mivhed.gob.do", Activo = true },
            new() { Nombre = "Ministerio Público", Siglas = "PGR", Categoria = "Ministerio", PoderEstado = "Poder Ejecutivo", Sector = "Justicia", SitioWeb = "https://pgr.gob.do", Activo = true },
            new() { Nombre = "Instituto Dominicano de las Telecomunicaciones", Siglas = "INDOTEL", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Presidencia", SitioWeb = "https://indotel.gob.do", Activo = true },
            new() { Nombre = "Dirección General de Ética e Integridad Gubernamental", Siglas = "DIGEIG", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Presidencia", SitioWeb = "https://digeig.gob.do", Activo = true },
            new() { Nombre = "Dirección General de Contrataciones Públicas", Siglas = "DGCP", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://dgcp.gob.do", Activo = true },
            new() { Nombre = "Dirección General de Presupuesto", Siglas = "DIGEPRES", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://digepres.gob.do", Activo = true },
            new() { Nombre = "Dirección General de Contabilidad Gubernamental", Siglas = "DIGECOG", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Hacienda", SitioWeb = "https://digecog.gob.do", Activo = true },
            new() { Nombre = "Oficina Gubernamental de Tecnologías de la Información y Comunicación", Siglas = "OGTIC", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Administración Pública", SitioWeb = "https://ogtic.gob.do", Activo = true },
            new() { Nombre = "Oficina Nacional de Estadística", Siglas = "ONE", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Economía, Planificación y Desarrollo", SitioWeb = "https://one.gob.do", Activo = true },
            new() { Nombre = "Oficina Nacional de la Propiedad Industrial", Siglas = "ONAPI", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Industria, Comercio y MIPYMES", SitioWeb = "https://onapi.gob.do", Activo = true },
            new() { Nombre = "Servicio Nacional de Salud", Siglas = "SNS", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Salud", SitioWeb = "https://sns.gob.do", Activo = true },
            new() { Nombre = "Instituto Nacional de Formación Técnico Profesional", Siglas = "INFOTEP", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Trabajo", SitioWeb = "https://infotep.gob.do", Activo = true },
            new() { Nombre = "Instituto Tecnológico de las Américas", Siglas = "ITLA", Categoria = "Organismo Desconcentrado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Educación Superior, Ciencia y Tecnología", SitioWeb = "https://itla.edu.do", Activo = true },
            new() { Nombre = "Universidad Autónoma de Santo Domingo", Siglas = "UASD", Categoria = "Organismo Descentralizado Funcionalmente", PoderEstado = "Poder Ejecutivo", Sector = "Educación Superior, Ciencia y Tecnología", SitioWeb = "https://uasd.edu.do", Activo = true }
        };
    }
}
