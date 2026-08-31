-- =============================================================================
-- SEMILLA DE DATOS INICIALES - SISTEMA DE GESTIÓN DE SOLICITUDES INTERNAS (SB)
-- Contraseñas:
--   admin@sb.gob.do        => Admin123!
--   analista.tech@sb.gob.do=> Analista123!
--   juan.perez@sb.gob.do   => User123!
-- =============================================================================

USE SB_GestionSolicitudes;
GO

-- 1. Usuarios
SET IDENTITY_INSERT Usuarios ON;
INSERT INTO Usuarios (Id, Nombre, Correo, PasswordHash, Rol, Activo, FechaCreacion) VALUES
(1, N'Administrador del Sistema', N'admin@sb.gob.do', N'$2b$11$4UngfRomWiI5wdMvFKALDucqR2976a9YVOIonH/BDLFqdP4.6uJjy', 1, 1, CURRENT_TIMESTAMP),
(2, N'Carlos Mendoza (Analista IT)', N'analista.tech@sb.gob.do', N'$2b$11$DEARViae9mtEdKUiASwgmujvh.cigiyw2Ao4QBJsMkNstcGA1owNO', 2, 1, CURRENT_TIMESTAMP),
(3, N'Juan Pérez', N'juan.perez@sb.gob.do', N'$2b$11$bPrALQQeBIs1AZ1sr/qQjuycZVL2brtEoQTqrKOynEdlH1El9a4OO', 3, 1, CURRENT_TIMESTAMP);
SET IDENTITY_INSERT Usuarios OFF;
GO

-- 2. Áreas
SET IDENTITY_INSERT Areas ON;
INSERT INTO Areas (Id, Nombre, Descripcion, Activa, FechaCreacion) VALUES
(1, N'Tecnología de la Información', N'División de TI y Canales', 1, CURRENT_TIMESTAMP),
(2, N'Finanzas y Contabilidad', N'Departamento Financiero', 1, CURRENT_TIMESTAMP),
(3, N'Recursos Humanos', N'Gestión del Talento Humano', 1, CURRENT_TIMESTAMP),
(4, N'Operaciones Bancarias', N'División Operativa', 1, CURRENT_TIMESTAMP),
(5, N'Legal y Cumplimiento', N'Asesoría Jurídica', 1, CURRENT_TIMESTAMP);
SET IDENTITY_INSERT Areas OFF;
GO

-- 3. Tipos de Solicitud
SET IDENTITY_INSERT TiposSolicitud ON;
INSERT INTO TiposSolicitud (Id, Nombre, Descripcion, Activo, FechaCreacion) VALUES
(1, N'Soporte Técnico', N'Asistencia en Hardware/Software', 1, CURRENT_TIMESTAMP),
(2, N'Desarrollo de Software', N'Nuevos requerimientos y ajustes', 1, CURRENT_TIMESTAMP),
(3, N'Acceso a Sistemas', N'Permisos y credenciales', 1, CURRENT_TIMESTAMP),
(4, N'Mantenimiento de Hardware', N'Reparaciones de equipos', 1, CURRENT_TIMESTAMP);
SET IDENTITY_INSERT TiposSolicitud OFF;
GO

-- 4. Solicitudes de Ejemplo
SET IDENTITY_INSERT Solicitudes ON;
INSERT INTO Solicitudes (Id, Codigo, Titulo, Descripcion, Prioridad, Estado, ReferenciaEvidencia, FechaCreacion, FechaCompromiso, SolicitanteId, ResponsableId, AreaId, TipoSolicitudId) VALUES
(1, N'SOL-2026-0001', N'Acceso al módulo de reportes consolidados', N'Se requiere asignar permisos de lectura en la plataforma de reportes bancarios para el nuevo analista.', 3, 3, N'https://intranet.sb.gob.do/ticket-ref/8821', DATEADD(day, -3, GETUTCDATE()), DATEADD(day, 2, GETUTCDATE()), 3, 2, 1, 3),
(2, N'SOL-2026-0002', N'Impresora de área no responde', N'La impresora HP LaserJet del departamento de Finanzas no recibe trabajos de impresión.', 2, 1, N'Piso 3 - Oficina 304', DATEADD(hour, -5, GETUTCDATE()), DATEADD(day, 1, GETUTCDATE()), 3, NULL, 2, 1),
(3, N'SOL-2026-0003', N'Ajuste en cálculo de comisiones', N'Se solicita modificar el algoritmo de cálculo de comisiones conforme a la nueva normativa SB-2026.', 4, 5, N'Resolución SB-2026-04', DATEADD(day, -10, GETUTCDATE()), DATEADD(day, -2, GETUTCDATE()), 3, 2, 1, 2),
(4, N'SOL-2026-0004', N'Reemplazo de batería de UPS', N'UPS emitía sonido constante de alerta de batería agotada en la terminal 12.', 1, 6, N'Inventario #9921', DATEADD(day, -15, GETUTCDATE()), DATEADD(day, -12, GETUTCDATE()), 3, 2, 4, 4);
SET IDENTITY_INSERT Solicitudes OFF;
GO

-- 4.1 Historiales de Estado
SET IDENTITY_INSERT HistorialesEstado ON;
INSERT INTO HistorialesEstado (Id, SolicitudId, EstadoAnterior, EstadoNuevo, UsuarioId, Comentario, Fecha) VALUES
(1, 1, NULL, 1, 3, N'Solicitud creada por el usuario.', DATEADD(day, -3, GETUTCDATE())),
(2, 1, 1, 3, 2, N'Solicitud tomada por el analista y puesta en progreso.', DATEADD(day, -2, GETUTCDATE())),
(3, 4, 5, 6, 1, N'Se verificó el reemplazo satisfactorio del UPS. Solicitud cerrada oficialmente.', DATEADD(day, -12, GETUTCDATE()));
SET IDENTITY_INSERT HistorialesEstado OFF;
GO

-- 4.2 Comentarios
SET IDENTITY_INSERT Comentarios ON;
INSERT INTO Comentarios (Id, SolicitudId, UsuarioId, Texto, EsPublico, Fecha) VALUES
(1, 1, 2, N'Se han revisado las políticas de acceso. En proceso de asignación de perfil.', 1, DATEADD(day, -2, GETUTCDATE())),
(2, 4, 1, N'Batería reemplazada por el proveedor de soporte técnico.', 1, DATEADD(day, -12, GETUTCDATE()));
SET IDENTITY_INSERT Comentarios OFF;
GO

-- 4.3 Notificaciones
SET IDENTITY_INSERT Notificaciones ON;
INSERT INTO Notificaciones (Id, SolicitudId, UsuarioDestinoId, Canal, Asunto, Mensaje, Enviado, Fecha) VALUES
(1, 1, 3, 1, N'Solicitud Asignada: SOL-2026-0001', N'Su solicitud ''Acceso al módulo de reportes consolidados'' ha sido asignada a Carlos Mendoza (Analista IT).', 1, DATEADD(day, -2, GETUTCDATE()));
SET IDENTITY_INSERT Notificaciones OFF;
GO

-- 5. Entidades Gubernamentales Oficiales de la República Dominicana (181 entidades según ListaEntidadesGubernamentales.xlsx)
SET IDENTITY_INSERT EntidadesGubernamentales ON;
INSERT INTO EntidadesGubernamentales (Id, Nombre, Categoria, PoderEstado, Sector, Activo, FechaCreacion) VALUES
(1, N'Acuario Nacional', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(2, N'Administración General del Parque Nacional Mirador Norte', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(3, N'Archivo General de la Nación', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(4, N'Autoridad Nacional de Asuntos Marítimos', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(5, N'Autoridad Portuaria Dominicana', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(6, N'Biblioteca Nacional Pedro Henríquez Ureña', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(7, N'Centro de Atención Integral para la Discapacidad', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(8, N'Centro de Desarrollo y Competitividad Industrial', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(9, N'Centro de Operaciones de Emergencias', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(10, N'Comisión de Fomento a la Tecnificación del Sistema Nacional de Riego', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(11, N'Comisión Nacional de Defensa de la Competencia', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(12, N'Comisión Nacional de Energía', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Energía y Minas', 1, CURRENT_TIMESTAMP),
(13, N'Comisión Presidencial de Apoyo al Desarrollo Barrial', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(14, N'Comisión Reguladora de Prácticas Desleales en el Comercio y Medidas de Salvaguardas', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(15, N'Comité Ejecutor de Infraestructuras de Zonas Turísticas', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Turismo', 1, CURRENT_TIMESTAMP),
(16, N'Consejo de Coordinación de la Zona Especial de Desarrollo Fronterizo', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(17, N'Consejo Dominicano de Pesca y Acuicultura', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(18, N'Consejo Nacional de Competitividad', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(19, N'Consejo Nacional de Discapacidad', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(20, N'Consejo Nacional de Drogas', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(21, N'Consejo Nacional de Fronteras', N'Órgano Colegiado', N'Poder Ejecutivo', N'Relaciones Exteriores', 1, CURRENT_TIMESTAMP),
(22, N'Consejo Nacional de Investigaciones Agropecuarias y Forestales', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(23, N'Consejo Nacional de la Persona Envejeciente', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(24, N'Consejo Nacional de Producción Pecuaria', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(25, N'Consejo Nacional de Promoción y Apoyo a la Micro, Pequeña y Mediana Empresa', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(26, N'Consejo Nacional de Seguridad Social', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(27, N'Consejo Nacional de Zonas Francas de Exportación', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(28, N'Consejo Nacional para el Cambio Climático y Mercado de Carbono', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(29, N'Consejo Nacional para el VIH y el SIDA', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(30, N'Consejo Nacional para la Niñez y la Adolescencia', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(31, N'Consejo Nacional para la Reglamentación y Fomento de la Industria Lechera', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(32, N'Consejo Provincial para la Administración de los Fondos Mineros Sánchez Ramírez', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Economía, Planificación y Desarrollo', 1, CURRENT_TIMESTAMP),
(33, N'Consultoría Jurídica del Poder Ejecutivo', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(34, N'Contraloría General de la República', N'Ministerio', N'Poder Ejecutivo', N'Control Interno', 1, CURRENT_TIMESTAMP),
(35, N'Corporación de Fomento de la Industria Hotelera y Desarrollo del Turismo', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Turismo', 1, CURRENT_TIMESTAMP),
(36, N'Corporación del Acueducto y Alcantarillado de Boca Chica', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(37, N'Corporación del Acueducto y Alcantarillado de la Romana', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(38, N'Corporación del Acueducto y Alcantarillado de la Vega', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(39, N'Corporación del Acueducto y Alcantarillado de Moca', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(40, N'Corporación del Acueducto y Alcantarillado de Monseñor Nouel', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(41, N'Corporación del Acueducto y Alcantarillado de Puerto Plata', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(42, N'Corporación del Acueducto y Alcantarillado de Santiago', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(43, N'Corporación del Acueducto y Alcantarillado de Santo Domingo', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(44, N'Corporación Estatal de Radio y Televisión', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(45, N'Cuerpo Especializado de Control de Combustibles', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Defensa', 1, CURRENT_TIMESTAMP),
(46, N'Departamento Aeroportuario', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(47, N'Dirección de Asistencia Social y Alimentación Comunitaria', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(48, N'Dirección de Desarrollo Provincial', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(49, N'Dirección de Desarrollo Social Supérate', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(50, N'Dirección de Estrategia y Comunicación Gubernamental', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(51, N'Dirección de Infraestructura Escolar', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(52, N'Dirección de Prensa del Presidente de la República', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(53, N'Dirección del Comisionado Nacional de Beisbol', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Deportes y Recreación', 1, CURRENT_TIMESTAMP),
(54, N'Dirección Ejecutiva del Sistema Nacional de Atención a Emergencias y Seguridad 9-1-1', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(55, N'Dirección General de Aduanas', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(56, N'Dirección General de Alianzas Público Privadas', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(57, N'Dirección General de Bellas Artes', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(58, N'Dirección General de Bienes Nacionales', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(59, N'Dirección General de Catastro Nacional', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(60, N'Dirección General de Cine', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(61, N'Dirección General de Contabilidad Gubernamental', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(62, N'Dirección General de Contrataciones Públicas', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(63, N'Dirección General de Desarrollo de la Comunidad', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(64, N'Dirección General de Desarrollo Fronterizo', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(65, N'Dirección General de Embellecimiento de Carreteras y Avenidas de Circunvalación del País', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(66, N'Dirección General de Ética e Integridad Gubernamental', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(67, N'Dirección General de Ganadería', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(68, N'Dirección General de Impuestos Internos', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(69, N'Dirección General de Información y Defensa de los Afiliados a la Seguridad Social', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(70, N'Dirección General de Jubilaciones y Pensiones a Cargo del Estado', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(71, N'Dirección General de la Policía Nacional', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Interior y Policía', 1, CURRENT_TIMESTAMP),
(72, N'Dirección General de Mecenazgo', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(73, N'Dirección General de Medicamentos, Alimentos y Productos Sanitarios', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(74, N'Dirección General de Migración', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Interior y Policía', 1, CURRENT_TIMESTAMP),
(75, N'Dirección General de Minería', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Energía y Minas', 1, CURRENT_TIMESTAMP),
(76, N'Dirección General de Museos', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(77, N'Dirección General de Pasaportes', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Relaciones Exteriores', 1, CURRENT_TIMESTAMP),
(78, N'Dirección General de Persecución del Ministerio Público', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Justicia', 1, CURRENT_TIMESTAMP),
(79, N'Dirección General de Presupuesto', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(80, N'Dirección General de Proyectos Estratégicos y Especiales de la Presidencia', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(81, N'Dirección General de Riesgos Agropecuarios', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(82, N'Dirección General de Servicios Penitenciarios y Correccionales', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Justicia', 1, CURRENT_TIMESTAMP),
(83, N'Dirección Nacional de Control de Drogas', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(84, N'Empresa Metropolitana de Transporte, (EMT). S.A.', N'Empresa Pública', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(85, N'Fondo de Pensiones y Jubilaciones de los Trabajadores de la Construcción', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(86, N'Fondo Especial para el Desarrollo Agropecuario', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(87, N'Fondo Nacional para el Medio Ambiente y Recursos Naturales', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(88, N'Gabinete de Coordinación de Política Social', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(89, N'Industria Nacional de la Aguja', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(90, N'Instituto Agrario Dominicano', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(91, N'Instituto Azucarero Dominicano', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(92, N'Instituto de Auxilios', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(93, N'Instituto de Desarrollo y Crédito Cooperativo', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(94, N'Instituto de Educación Superior en Formación Diplomática y Consular “Dr. Eduardo Latorre Rodríguez”', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(95, N'Instituto de Estabilización de Precios', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(96, N'Instituto de Formación Turística del Caribe', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Turismo', 1, CURRENT_TIMESTAMP),
(97, N'Instituto de Innovación en Biotecnología e Industria', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(98, N'Instituto del Tabaco de la República Dominicana', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(99, N'Instituto Dominicano de Aviación Civil', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(100, N'Instituto Dominicano de Evaluación e Investigación de la Calidad Educativa', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(101, N'Instituto Dominicano de Investigaciones Agropecuarias y Forestales', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(102, N'Instituto Dominicano de las Telecomunicaciones', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(103, N'Instituto Dominicano de Meteorología', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(104, N'Instituto Dominicano de Prevención y Protección de Riesgos Laborales', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(105, N'Instituto Dominicano del Café', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(106, N'Instituto Dominicano para la Calidad', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(107, N'Instituto Duartiano', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(108, N'Instituto Geográfico Nacional José Joaquín Hungría Morell', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Economía, Planificación y Desarrollo', 1, CURRENT_TIMESTAMP),
(109, N'Instituto Nacional de Administración Pública', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Administración Pública', 1, CURRENT_TIMESTAMP),
(110, N'Instituto Nacional de Aguas Potables y Alcantarillados', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(111, N'Instituto Nacional de Atención Integral a la Primera Infancia', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(112, N'Instituto Nacional de Bienestar Estudiantil', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(113, N'Instituto Nacional de Bienestar Magisterial', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(114, N'Instituto Nacional de Ciencias Forenses', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Justicia', 1, CURRENT_TIMESTAMP),
(115, N'Instituto Nacional de Coordinación de Trasplante', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(116, N'Instituto Nacional de Custodia y Administración de Bienes Incautados, Decomisados y en Extinción de Dominio', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(117, N'Instituto Nacional de Educación Física', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(118, N'Instituto Nacional de Formación Técnico Profesional', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(119, N'Instituto Nacional de Formación y Capacitación del Magisterio', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(120, N'Instituto Nacional de la Uva', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(121, N'Instituto Nacional de Migración', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Interior y Policía', 1, CURRENT_TIMESTAMP),
(122, N'Instituto Nacional de Protección de los Derechos del Consumidor', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(123, N'Instituto Nacional de Recursos Hidráulicos', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(124, N'Instituto Nacional de Tránsito y Transporte Terrestre', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(125, N'Instituto para el Desarrollo del Suroeste', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Economía, Planificación y Desarrollo', 1, CURRENT_TIMESTAMP),
(126, N'Instituto Postal Dominicano', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(127, N'Instituto Superior de Formación Docente Salomé Ureña', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(128, N'Instituto Superior Especializado de Estudios Penitenciario y Correccionales', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Justicia', 1, CURRENT_TIMESTAMP),
(129, N'Instituto Técnico Superior Comunitario', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(130, N'Instituto Tecnológico de las Américas', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(131, N'Jardín Botánico Nacional "Dr. Rafael M. Moscoso"', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(132, N'Junta de Aviación Civil', N'Órgano Colegiado con Estructura Operativa', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(133, N'Liga Municipal Dominicana', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Interior y Policía', 1, CURRENT_TIMESTAMP),
(134, N'Lotería Nacional', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(135, N'Mercados Dominicanos de Abasto Agropecuario', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(136, N'Ministerio Administrativo de la Presidencia', N'Ministerio', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(137, N'Ministerio de Administración Pública', N'Ministerio', N'Poder Ejecutivo', N'Administración Pública', 1, CURRENT_TIMESTAMP),
(138, N'Ministerio de Agricultura', N'Ministerio', N'Poder Ejecutivo', N'Agricultura', 1, CURRENT_TIMESTAMP),
(139, N'Ministerio de Cultura', N'Ministerio', N'Poder Ejecutivo', N'Cultura', 1, CURRENT_TIMESTAMP),
(140, N'Ministerio de Defensa', N'Ministerio', N'Poder Ejecutivo', N'Defensa', 1, CURRENT_TIMESTAMP),
(141, N'Ministerio de Deportes y Recreación', N'Ministerio', N'Poder Ejecutivo', N'Deportes y Recreación', 1, CURRENT_TIMESTAMP),
(142, N'Ministerio de Educación', N'Ministerio', N'Poder Ejecutivo', N'Educación', 1, CURRENT_TIMESTAMP),
(143, N'Ministerio de Educación Superior, Ciencia y Tecnología', N'Ministerio', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(144, N'Ministerio de Energía y Minas', N'Ministerio', N'Poder Ejecutivo', N'Energía y Minas', 1, CURRENT_TIMESTAMP),
(145, N'Ministerio de Hacienda y Economía', N'Ministerio', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(146, N'Ministerio de Industria, Comercio y Mipymes', N'Ministerio', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(147, N'Ministerio de Interior y Policía', N'Ministerio', N'Poder Ejecutivo', N'Interior y Policía', 1, CURRENT_TIMESTAMP),
(148, N'Ministerio de la Juventud', N'Ministerio', N'Poder Ejecutivo', N'Juventud', 1, CURRENT_TIMESTAMP),
(149, N'Ministerio de la Mujer', N'Ministerio', N'Poder Ejecutivo', N'Mujer', 1, CURRENT_TIMESTAMP),
(150, N'Ministerio de la Presidencia', N'Ministerio', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(151, N'Ministerio de la Vivienda, Hábitat y Edificaciones', N'Ministerio', N'Poder Ejecutivo', N'Vivienda, Hábitat y Edificaciones', 1, CURRENT_TIMESTAMP),
(152, N'Ministerio de Medio Ambiente y Recursos Naturales', N'Ministerio', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(153, N'Ministerio de Obras Públicas y Comunicaciones', N'Ministerio', N'Poder Ejecutivo', N'Obras Públicas y Comunicaciones', 1, CURRENT_TIMESTAMP),
(154, N'Ministerio de Relaciones Exteriores', N'Ministerio', N'Poder Ejecutivo', N'Relaciones Exteriores', 1, CURRENT_TIMESTAMP),
(155, N'Ministerio de Salud Pública y Asistencia Social', N'Ministerio', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(156, N'Ministerio de Trabajo', N'Ministerio', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(157, N'Ministerio de Turismo', N'Ministerio', N'Poder Ejecutivo', N'Turismo', 1, CURRENT_TIMESTAMP),
(158, N'Ministerio Público', N'Ministerio', N'Poder Ejecutivo', N'Justicia', 1, CURRENT_TIMESTAMP),
(159, N'Museo Nacional de Historia Natural "Prof. Eugenio de Jesús Marcano"', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(160, N'Oficina de la Defensa Civil', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(161, N'Oficina Gubernamental de Tecnologías de la Información y Comunicación', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Administración Pública', 1, CURRENT_TIMESTAMP),
(162, N'Oficina Nacional de Derecho de Autor', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(163, N'Oficina Nacional de Estadística', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Economía, Planificación y Desarrollo', 1, CURRENT_TIMESTAMP),
(164, N'Oficina Nacional de Evaluación Sísmica y Vulnerabilidad de Infraestructura y Edificaciones', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(165, N'Oficina Nacional de la Propiedad Industrial', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(166, N'Organismo Dominicano de Acreditación', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Industria, Comercio y MIPYMES', 1, CURRENT_TIMESTAMP),
(167, N'Parque Zoológico Nacional Arq. Manuel Valverde Podesta', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Medio Ambiente y Recursos Naturales', 1, CURRENT_TIMESTAMP),
(168, N'Programa de Medicamentos Esenciales/Central de Apoyo Logístico', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(169, N'Servicio Geológico Nacional', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Energía y Minas', 1, CURRENT_TIMESTAMP),
(170, N'Servicio Nacional de Salud', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Salud', 1, CURRENT_TIMESTAMP),
(171, N'Sistema Único de Beneficiarios', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Social', 1, CURRENT_TIMESTAMP),
(172, N'Superintendencia de Salud y Riesgos Laborales', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(173, N'Superintendencia de Seguros', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(174, N'Superintendencia del Mercado de Valores', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(175, N'Tesorería de la Seguridad Social', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Trabajo', 1, CURRENT_TIMESTAMP),
(176, N'Tesorería Nacional', N'Organismo Desconcentrado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(177, N'Unidad de Análisis Financiero', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Hacienda', 1, CURRENT_TIMESTAMP),
(178, N'Unidad Ejecutora para la Readecuación de Barrios y Entornos', N'Unidad Ejecutora (Proyecto)', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(179, N'Unidad Técnica Ejecutora de Titulación de Terrenos del Estado', N'Unidad Ejecutora (Proyecto)', N'Poder Ejecutivo', N'Presidencia', 1, CURRENT_TIMESTAMP),
(180, N'Universidad Autónoma de Santo Domingo', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP),
(181, N'Universidad Tecnológica del Cibao Oriental', N'Organismo Descentralizado Funcionalmente', N'Poder Ejecutivo', N'Educación Superior, Ciencia y Tecnología', 1, CURRENT_TIMESTAMP)
;
SET IDENTITY_INSERT EntidadesGubernamentales OFF;
GO