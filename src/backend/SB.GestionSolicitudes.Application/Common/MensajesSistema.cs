namespace SB.GestionSolicitudes.Application.Common;

public static class MensajesSistema
{
    public static class Errores
    {
        public const string ERROR_INTERNO_SERVIDOR = "Ocurrió un error interno en el servidor";
        public const string EXCEPCION_NO_CONTROLADA_LOG = "Excepción no controlada capturada en middleware global: {Message}";
    }

    public static class Auth
    {
        public const string CORREO_REQUERIDO = "El correo electrónico es obligatorio.";
        public const string CORREO_INVALIDO = "El formato del correo electrónico no es válido.";
        public const string PASSWORD_REQUERIDA = "La contraseña es obligatoria.";
        public const string CREDENCIALES_REQUERIDAS = "El correo y la contraseña son requeridos.";
        public const string CREDENCIALES_INVALIDAS = "Credenciales inválidas.";
        public const string CREDENCIALES_INCORRECTAS = "Credenciales inválidas.";
        public const string USUARIO_DESACTIVADO = "El usuario '{0}' se encuentra desactivado en el sistema. No puede acceder ni cambiar a este rol.";
        public const string LOGIN_EXITOSO = "Inicio de sesión exitoso.";
        public const string USUARIO_NO_ENCONTRADO = "Usuario no encontrado.";
    }

    public static class Solicitud
    {
        public const string TITULO_REQUERIDO = "El título es obligatorio.";
        public const string TITULO_EXCEDE_MAXIMO = "El título no puede exceder los 150 caracteres.";
        public const string DESCRIPCION_REQUERIDA = "La descripción es obligatoria.";
        public const string DESCRIPCION_EXCEDE_MAXIMO = "La descripción no puede exceder los 2000 caracteres.";
        public const string AREA_INVALIDA = "Debe seleccionar un área válida.";
        public const string AREA_NO_EXISTE_O_INACTIVA = "El área seleccionada no existe o está inactiva.";
        public const string TIPO_SOLICITUD_INVALIDO = "Debe seleccionar un tipo de solicitud válido.";
        public const string TIPO_SOLICITUD_NO_EXISTE_O_INACTIVO = "El tipo de solicitud seleccionado no existe o está inactivo.";
        public const string PRIORIDAD_INVALIDA = "La prioridad seleccionada no es válida.";
        public const string REFERENCIA_EVIDENCIA_EXCEDE_MAXIMO = "La referencia de evidencia no puede exceder los 500 caracteres.";
        public const string NO_EXISTE = "La solicitud especificada no existe.";
        public const string SIN_PERMISOS_CONSULTA = "No tiene permisos para consultar esta solicitud.";
        public const string SIN_PERMISOS_MODIFICACION = "No tiene permisos para modificar esta solicitud.";
        public const string REGISTRO_EXITOSO = "Solicitud registrada con éxito.";
        public const string ACTUALIZACION_EXITOSA = "Solicitud actualizada exitosamente.";
        public const string ESTADO_ACTUALIZADO = "Estado actualizado a '{0}' exitosamente.";
        public const string ASIGNACION_EXITOSA = "Solicitud asignada a {0} con éxito.";
        public const string HISTORIAL_INICIAL_COMENTARIO = "Solicitud creada en el sistema.";
        public const string ESTADO_INVALIDO = "El nuevo estado seleccionado no es válido.";
        public const string REAPERTURA_SOLO_ADMIN_ANALISTA = "Solo los usuarios con rol Administrador o Analista pueden reabrir una solicitud cerrada.";
        public const string COMENTARIO_RESOLUCION_REQUERIDO = "Es obligatorio ingresar un comentario de resolución para poder cerrar una solicitud.";
        public const string ASIGNAR_RESPONSABLE_SOLO_ADMIN_ANALISTA = "Solo administradores y analistas pueden asignar o reasignar responsables.";
        public const string RESPONSABLE_DEBE_SER_ANALISTA_O_ADMIN_ACTIVO = "El usuario seleccionado como responsable debe ser un analista o administrador activo.";
    }

    public static class Comentario
    {
        public const string TEXTO_REQUERIDO = "El texto del comentario es obligatorio.";
        public const string TEXTO_EXCEDE_MAXIMO = "El comentario no puede exceder los 1000 caracteres.";
        public const string SIN_PERMISOS_COMENTAR = "No tiene permisos para comentar en esta solicitud.";
        public const string AGREGADO_EXITOSAMENTE = "Comentario agregado exitosamente.";
    }

    public static class Notificaciones
    {
        public const string ASUNTO_SOLICITUD_CREADA = "Solicitud Creada Exitosamente: {0}";
        public const string MENSAJE_SOLICITUD_CREADA = "Su solicitud '{0}' ha sido registrada con el código {1}.";

        public const string ASUNTO_SOLICITUD_ASIGNADA = "Nueva Solicitud Asignada: {0}";
        public const string MENSAJE_SOLICITUD_ASIGNADA = "Le ha sido asignada la solicitud {0} para su atención técnica.";

        public const string ASUNTO_ESTADO_CAMBIADO = "Cambio de Estado en Solicitud: {0}";
        public const string MENSAJE_ESTADO_CAMBIADO = "La solicitud {0} cambió de estado a '{1}'. Comentario: {2}";

        public const string ASUNTO_SOLICITUD_CERRADA = "Solicitud Cerrada: {0}";
        public const string MENSAJE_SOLICITUD_CERRADA = "La solicitud {0} ha sido cerrada. Resolución: {1}";

        public const string ASUNTO_SOLICITUD_ACTUALIZADA = "Información de Solicitud Actualizada: {0}";
        public const string MENSAJE_SOLICITUD_ACTUALIZADA = "Se ha actualizado la información de la solicitud {0} ('{1}').";

        // Usuarios
        public const string ASUNTO_BIENVENIDA_USUARIO = "¡Bienvenido/a al Sistema, {0}!";
        public const string MENSAJE_BIENVENIDA_USUARIO = "Tu cuenta ha sido creada exitosamente con el rol {0}. Ya puedes acceder al sistema.";

        public const string ASUNTO_USUARIO_REGISTRADO_ADMIN = "Nuevo Usuario Registrado: {0}";
        public const string MENSAJE_USUARIO_REGISTRADO_ADMIN = "Se ha creado el usuario '{0}' ({1}) con rol {2}.";

        public const string ASUNTO_ESTADO_USUARIO_CAMBIADO = "Actualización de Estado de Cuenta";
        public const string MENSAJE_ESTADO_USUARIO_CAMBIADO = "El estado de tu cuenta de usuario ha cambiado a: {0}.";

        public const string ASUNTO_ESTADO_USUARIO_ADMIN = "Usuario {0}: {1}";
        public const string MENSAJE_ESTADO_USUARIO_ADMIN = "El usuario '{0}' ha sido {1} en el sistema.";

        public const string ASUNTO_PERFIL_USUARIO_ACTUALIZADO = "Actualización de Perfil de Usuario";
        public const string MENSAJE_PERFIL_USUARIO_ACTUALIZADO = "Se han actualizado los datos de tu usuario. Tu rol actual es: {0}.";

        public const string ASUNTO_USUARIO_ACTUALIZADO_ADMIN = "Usuario Modificado: {0}";
        public const string MENSAJE_USUARIO_ACTUALIZADO_ADMIN = "Se han actualizado los datos y rol del usuario '{0}' ({1}).";

        // Entidades Gubernamentales
        public const string ASUNTO_ENTIDAD_CREADA = "Nueva Entidad Registrada: {0}";
        public const string MENSAJE_ENTIDAD_CREADA = "Se ha registrado una nueva entidad gubernamental: {0} ({1}).";

        public const string ASUNTO_ENTIDAD_MODIFICADA = "Entidad Gubernamental Modificada: {0}";
        public const string MENSAJE_ENTIDAD_MODIFICADA = "Se han actualizado los datos de la entidad gubernamental '{0}'.";

        public const string ASUNTO_ENTIDAD_ESTADO_CAMBIADO = "Estado de Entidad Actualizado: {0}";
        public const string MENSAJE_ENTIDAD_ESTADO_CAMBIADO = "La entidad gubernamental '{0}' ha sido {1}.";

        // Catálogos
        public const string ASUNTO_CATALOGO_CREADO = "Nuevo Registro en Catálogo: {0}";
        public const string MENSAJE_CATALOGO_CREADO = "Se ha creado un nuevo elemento en {0}: '{1}'.";

        public const string ASUNTO_CATALOGO_MODIFICADO = "Catálogo Actualizado: {0}";
        public const string MENSAJE_CATALOGO_MODIFICADO = "Se ha modificado el elemento '{0}' en el catálogo de {1}.";

        public const string ASUNTO_CATALOGO_ESTADO_CAMBIADO = "Estado de Catálogo Modificado: {0}";
        public const string MENSAJE_CATALOGO_ESTADO_CAMBIADO = "El elemento '{0}' en {1} ha sido {2}.";

        // Comentarios
        public const string ASUNTO_NUEVO_COMENTARIO = "Nuevo Comentario en Solicitud: {0}";
        public const string MENSAJE_NUEVO_COMENTARIO = "Se ha añadido un nuevo comentario en la solicitud {0}: \"{1}\".";

        public const string LOG_NOTIFICACION_ENVIADA = "NOTIFICACIÓN [{Canal}] -> Usuario #{UsuarioId} | Asunto: '{Asunto}' | Mensaje: '{Mensaje}'";
    }

    public static class Catalogo
    {
        public const string NOMBRE_REQUERIDO = "El nombre es obligatorio.";
        public const string NOMBRE_EXCEDE_MAXIMO = "El nombre no puede exceder los 100 caracteres.";
        public const string DESCRIPCION_EXCEDE_MAXIMO = "La descripción no puede exceder los 250 caracteres.";
        public const string AREA_NO_ENCONTRADA = "El área especificada no existe.";
        public const string TIPO_NO_ENCONTRADO = "El tipo de solicitud especificado no existe.";
        public const string REGISTRO_DUPLICADO = "Ya existe un registro con este nombre.";
        public const string AREA_CREADA_EXITOSAMENTE = "Área creada exitosamente.";
        public const string AREA_ACTUALIZADA_EXITOSAMENTE = "Área actualizada exitosamente.";
        public const string AREA_ESTADO_CAMBIADO = "Área {0} exitosamente.";
        public const string TIPO_CREADO_EXITOSAMENTE = "Tipo de solicitud creado exitosamente.";
        public const string TIPO_ACTUALIZADO_EXITOSAMENTE = "Tipo de solicitud actualizado exitosamente.";
        public const string TIPO_ESTADO_CAMBIADO = "Tipo de solicitud {0} exitosamente.";
    }

    public static class UsuarioMensajes
    {
        public const string NOMBRE_REQUERIDO = "El nombre del usuario es obligatorio.";
        public const string CORREO_REQUERIDO = "El correo electrónico es obligatorio.";
        public const string CORREO_DUPLICADO = "Ya existe un usuario registrado con este correo electrónico.";
        public const string PASSWORD_REQUERIDA = "La contraseña es requerida.";
        public const string PASSWORD_MINIMO = "La contraseña debe tener al menos 6 caracteres.";
        public const string USUARIO_NO_EXISTE = "El usuario especificado no existe.";
        public const string CREACION_EXITOSA = "Usuario creado exitosamente.";
        public const string ACTUALIZACION_EXITOSA = "Usuario actualizado exitosamente.";
        public const string ESTADO_CAMBIADO = "Usuario {0} exitosamente.";
        public const string DESACTIVACION_EXITOSA = "Usuario desactivado correctamente.";
    }

    public static class EntidadGubernamentalMensajes
    {
        public const string NOMBRE_REQUERIDO = "El nombre de la entidad es obligatorio.";
        public const string CATEGORIA_REQUERIDA = "La categoría de la entidad es obligatoria.";
        public const string PODER_ESTADO_REQUERIDO = "El poder del estado es obligatorio.";
        public const string SECTOR_REQUERIDO = "El sector es obligatorio.";
        public const string ENTIDAD_NO_EXISTE = "La entidad gubernamental especificada no existe.";
        public const string REGISTRO_DUPLICADO = "Ya existe una entidad gubernamental con este nombre.";
        public const string CREACION_EXITOSA = "Entidad gubernamental creada exitosamente.";
        public const string ACTUALIZACION_EXITOSA = "Entidad gubernamental actualizada exitosamente.";
        public const string ESTADO_CAMBIADO = "Entidad gubernamental {0} exitosamente.";
        public const string DESACTIVACION_EXITOSA = "Entidad gubernamental desactivada correctamente.";
    }
}
