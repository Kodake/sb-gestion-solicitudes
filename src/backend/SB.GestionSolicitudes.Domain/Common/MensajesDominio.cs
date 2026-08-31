namespace SB.GestionSolicitudes.Domain.Common;

public static class MensajesDominio
{
    public static class SolicitudReglas
    {
        public const string TRANSICION_INVALIDA = "Transición inválida: No está permitido cambiar de '{0}' a '{1}'.";
        public const string REAPERTURA_PERMISOS = "Solo un Administrador o Analista puede reabrir una solicitud cerrada.";
        public const string CIERRE_REQUIERE_COMENTARIO = "Para cerrar una solicitud es obligatorio proporcionar un comentario de resolución o justificación.";
        public const string CAMBIO_ESTADO_HISTORIAL = "Cambio de estado a {0}";
        public const string ASIGNAR_RESPONSABLE_PERMISOS = "Solo un Administrador o Analista puede asignar un responsable.";
        public const string RESPONSABLE_DEBE_SER_ANALISTA_O_ADMIN = "El responsable asignado debe ser un Analista o Administrador activo.";
        public const string ASIGNACION_HISTORIAL = "Asignado a {0}. Estado actualizado a En Análisis.";
        public const string MODIFICAR_SOLICITUD_AJENA = "No tiene permisos para modificar una solicitud ajena.";
        public const string MODIFICAR_ESTADO_NO_PERMITIDO = "No se puede modificar la solicitud en estado '{0}'. Solo en 'Registrada' o 'En Espera del Solicitante'.";
        public const string MODIFICAR_PERMISOS_INSUFICIENTES = "No tiene los privilegios necesarios para modificar esta solicitud.";
    }
}
