export enum RolEnum {
  Administrador = 1,
  Analista = 2,
  Solicitante = 3
}

export enum PrioridadEnum {
  Baja = 1,
  Media = 2,
  Alta = 3,
  Critica = 4
}

export enum EstadoSolicitudEnum {
  Registrada = 1,
  EnAnalisis = 2,
  EnProgreso = 3,
  EnEsperaDelSolicitante = 4,
  Resuelta = 5,
  Cerrada = 6
}

export enum CanalNotificacionEnum {
  Database = 1,
  Console = 2,
  Email = 3
}

// Interfaces con prefijo 'I' según Regla 8.0 del documento de especificaciones técnicas
export interface IUser {
  id: number;
  nombre: string;
  correo: string;
  rol: RolEnum;
  rolNombre: string;
  activo: boolean;
}

export interface IUsuarioGestion {
  id: number;
  nombre: string;
  correo: string;
  rol: RolEnum;
  rolNombre: string;
  activo: boolean;
  fechaCreacion: string;
  fechaModificacion?: string;
}

export interface ILoginResponse {
  token: string;
  expiracion: string;
  usuario: IUser;
}

export interface IArea {
  id: number;
  nombre: string;
  descripcion?: string;
  activa: boolean;
}

export interface ITipoSolicitud {
  id: number;
  nombre: string;
  descripcion?: string;
  activo: boolean;
}

export interface IEntidadGubernamental {
  id: number;
  nombre: string;
  categoria: string;
  poderEstado: string;
  sector: string;
  siglas?: string;
  direccion?: string;
  telefono?: string;
  sitioWeb?: string;
  activo: boolean;
  fechaCreacion: string;
  fechaModificacion?: string;
}

export interface ISolicitud {
  id: number;
  codigo: string;
  titulo: string;
  descripcion: string;
  prioridad: PrioridadEnum;
  prioridadNombre: string;
  estado: EstadoSolicitudEnum;
  estadoNombre: string;
  referenciaEvidencia?: string;
  fechaCreacion: string;
  fechaCompromiso: string;
  fechaActualizacion?: string;
  solicitanteId: number;
  solicitanteNombre: string;
  responsableId?: number;
  responsableNombre?: string;
  areaId: number;
  areaNombre: string;
  tipoSolicitudId: number;
  tipoSolicitudNombre: string;
  estaVencida: boolean;
}

export interface IHistorialEstado {
  id: number;
  solicitudId: number;
  estadoAnterior?: EstadoSolicitudEnum;
  estadoAnteriorNombre?: string;
  estadoNuevo: EstadoSolicitudEnum;
  estadoNuevoNombre: string;
  usuarioId: number;
  usuarioNombre: string;
  comentario: string;
  fecha: string;
}

export interface IComentario {
  id: number;
  solicitudId: number;
  usuarioId: number;
  usuarioNombre: string;
  usuarioRol: string;
  texto: string;
  esPublico: boolean;
  fecha: string;
}

export interface INotificacion {
  id: number;
  solicitudId?: number;
  usuarioDestinoId: number;
  usuarioDestinoNombre: string;
  canal: CanalNotificacionEnum;
  canalNombre: string;
  asunto: string;
  mensaje: string;
  enviado: boolean;
  fecha: string;
}

export interface ISolicitudDetalle extends ISolicitud {
  historialEstados: IHistorialEstado[];
  comentarios: IComentario[];
  notificaciones: INotificacion[];
}

export interface IMetricaEstado {
  estado: EstadoSolicitudEnum;
  estadoNombre: string;
  cantidad: number;
}

export interface IMetricaPrioridad {
  prioridad: PrioridadEnum;
  prioridadNombre: string;
  cantidad: number;
}

export interface IDashboardResumen {
  totalSolicitudes: number;
  solicitudesAbiertas: number;
  solicitudesCerradas: number;
  solicitudesVencidas: number;
  porEstado: IMetricaEstado[];
  porPrioridad: IMetricaPrioridad[];
  ultimasSolicitudes: ISolicitud[];
}

export interface IPaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface IApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
  timestamp: string;
}

// Aliases para retrocompatibilidad
export type User = IUser;
export type LoginResponse = ILoginResponse;
export type Area = IArea;
export type TipoSolicitud = ITipoSolicitud;
export type EntidadGubernamental = IEntidadGubernamental;
export type Solicitud = ISolicitud;
export type HistorialEstado = IHistorialEstado;
export type Comentario = IComentario;
export type Notificacion = INotificacion;
export type SolicitudDetalle = ISolicitudDetalle;
export type MetricaEstado = IMetricaEstado;
export type MetricaPrioridad = IMetricaPrioridad;
export type DashboardResumen = IDashboardResumen;
export type PaginatedList<T> = IPaginatedList<T>;
export type ApiResponse<T> = IApiResponse<T>;
