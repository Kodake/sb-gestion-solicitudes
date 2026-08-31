-- =============================================================================
-- RESETEO COMPLETO Y LIMPIEZA DE BASE DE DATOS - SB_GestionSolicitudes
-- =============================================================================

USE master;
GO

-- Cerrar conexiones activas y eliminar base de datos si existe
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SB_GestionSolicitudes')
BEGIN
    ALTER DATABASE SB_GestionSolicitudes SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SB_GestionSolicitudes;
END
GO

-- Crear base de datos limpia
CREATE DATABASE SB_GestionSolicitudes;
GO

USE SB_GestionSolicitudes;
GO

-- 1. Tabla Usuarios
CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Rol INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL
);
GO

-- 2. Tabla Areas
CREATE TABLE dbo.Areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    Activa BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL
);
GO

-- 3. Tabla TiposSolicitud
CREATE TABLE dbo.TiposSolicitud (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL
);
GO

-- 4. Tabla EntidadesGubernamentales
CREATE TABLE dbo.EntidadesGubernamentales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(250) NOT NULL,
    Categoria NVARCHAR(150) NOT NULL,
    PoderEstado NVARCHAR(100) NOT NULL,
    Sector NVARCHAR(150) NOT NULL,
    Siglas NVARCHAR(50) NULL,
    Direccion NVARCHAR(300) NULL,
    Telefono NVARCHAR(50) NULL,
    SitioWeb NVARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL
);

CREATE NONCLUSTERED INDEX IX_Entidades_Nombre ON dbo.EntidadesGubernamentales(Nombre);
CREATE NONCLUSTERED INDEX IX_Entidades_Sector ON dbo.EntidadesGubernamentales(Sector);
CREATE NONCLUSTERED INDEX IX_Entidades_PoderEstado ON dbo.EntidadesGubernamentales(PoderEstado);
GO

-- 5. Tabla Solicitudes
CREATE TABLE dbo.Solicitudes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(20) NOT NULL UNIQUE,
    Titulo NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(2000) NOT NULL,
    Prioridad INT NOT NULL,
    Estado INT NOT NULL,
    ReferenciaEvidencia NVARCHAR(500) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaCompromiso DATETIME2 NOT NULL,
    FechaActualizacion DATETIME2 NULL,
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL,
    SolicitanteId INT NOT NULL,
    ResponsableId INT NULL,
    AreaId INT NOT NULL,
    TipoSolicitudId INT NOT NULL,
    CONSTRAINT FK_Solicitudes_Solicitante FOREIGN KEY (SolicitanteId) REFERENCES dbo.Usuarios(Id),
    CONSTRAINT FK_Solicitudes_Responsable FOREIGN KEY (ResponsableId) REFERENCES dbo.Usuarios(Id),
    CONSTRAINT FK_Solicitudes_Area FOREIGN KEY (AreaId) REFERENCES dbo.Areas(Id),
    CONSTRAINT FK_Solicitudes_TipoSolicitud FOREIGN KEY (TipoSolicitudId) REFERENCES dbo.TiposSolicitud(Id)
);

CREATE NONCLUSTERED INDEX IX_Solicitudes_Estado ON dbo.Solicitudes(Estado);
CREATE NONCLUSTERED INDEX IX_Solicitudes_SolicitanteId ON dbo.Solicitudes(SolicitanteId);
CREATE NONCLUSTERED INDEX IX_Solicitudes_ResponsableId ON dbo.Solicitudes(ResponsableId);
GO

-- 6. Tabla HistorialesEstado
CREATE TABLE dbo.HistorialesEstado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SolicitudId INT NOT NULL,
    EstadoAnterior INT NULL,
    EstadoNuevo INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(1000) NOT NULL,
    Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL,
    CONSTRAINT FK_HistorialesEstado_Solicitud FOREIGN KEY (SolicitudId) REFERENCES dbo.Solicitudes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_HistorialesEstado_Usuario FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(Id)
);
GO

-- 7. Tabla Comentarios
CREATE TABLE dbo.Comentarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SolicitudId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Texto NVARCHAR(2000) NOT NULL,
    EsPublico BIT NOT NULL DEFAULT 1,
    Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL,
    CONSTRAINT FK_Comentarios_Solicitud FOREIGN KEY (SolicitudId) REFERENCES dbo.Solicitudes(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Comentarios_Usuario FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(Id)
);
GO

-- 8. Tabla Notificaciones
CREATE TABLE dbo.Notificaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SolicitudId INT NULL,
    UsuarioDestinoId INT NOT NULL,
    Canal INT NOT NULL,
    Asunto NVARCHAR(200) NOT NULL,
    Mensaje NVARCHAR(1000) NOT NULL,
    Enviado BIT NOT NULL DEFAULT 1,
    Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NULL,
    UsuarioCreacionId INT NULL,
    UsuarioModificacionId INT NULL,
    CONSTRAINT FK_Notificaciones_Solicitud FOREIGN KEY (SolicitudId) REFERENCES dbo.Solicitudes(Id) ON DELETE SET NULL,
    CONSTRAINT FK_Notificaciones_Usuario FOREIGN KEY (UsuarioDestinoId) REFERENCES dbo.Usuarios(Id)
);
GO
