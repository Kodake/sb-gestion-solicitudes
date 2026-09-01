import { api } from './api';
import { ApiResponse, Comentario, PaginatedList, Solicitud, SolicitudDetalle } from '@/types';

export interface FiltrosSolicitudParams {
  pageNumber?: number;
  pageSize?: number;
  estado?: number;
  prioridad?: number;
  areaId?: number;
  solicitanteId?: number;
  responsableId?: number;
  fechaInicio?: string;
  fechaFin?: string;
  searchTerm?: string;
}

export interface CrearSolicitudData {
  titulo: string;
  descripcion: string;
  prioridad: number;
  areaId: number;
  tipoSolicitudId: number;
  referenciaEvidencia?: string;
  fechaCompromiso?: string;
}

export interface ActualizarSolicitudData {
  titulo: string;
  descripcion: string;
  prioridad: number;
  areaId: number;
  tipoSolicitudId: number;
  referenciaEvidencia?: string;
  fechaCompromiso?: string;
}

export const solicitudService = {
  async getSolicitudes(params?: FiltrosSolicitudParams): Promise<ApiResponse<PaginatedList<Solicitud>>> {
    const response = await api.get<ApiResponse<PaginatedList<Solicitud>>>('/solicitudes', { params });
    return response.data;
  },

  async getSolicitudById(id: number): Promise<ApiResponse<SolicitudDetalle>> {
    const response = await api.get<ApiResponse<SolicitudDetalle>>(`/solicitudes/${id}`);
    return response.data;
  },

  async crearSolicitud(data: CrearSolicitudData): Promise<ApiResponse<Solicitud>> {
    const response = await api.post<ApiResponse<Solicitud>>('/solicitudes', data);
    return response.data;
  },

  async actualizarSolicitud(id: number, data: ActualizarSolicitudData): Promise<ApiResponse<Solicitud>> {
    const response = await api.put<ApiResponse<Solicitud>>(`/solicitudes/${id}`, data);
    return response.data;
  },

  async cambiarEstado(id: number, nuevoEstado: number, comentario: string): Promise<ApiResponse<Solicitud>> {
    const response = await api.patch<ApiResponse<Solicitud>>(`/solicitudes/${id}/estado`, {
      nuevoEstado,
      comentario
    });
    return response.data;
  },

  async asignarResponsable(id: number, responsableId: number, comentario?: string): Promise<ApiResponse<Solicitud>> {
    const response = await api.patch<ApiResponse<Solicitud>>(`/solicitudes/${id}/asignacion`, {
      responsableId,
      comentario
    });
    return response.data;
  },

  async agregarComentario(solicitudId: number, texto: string, esPublico: boolean = true): Promise<ApiResponse<Comentario>> {
    const response = await api.post<ApiResponse<Comentario>>(`/solicitudes/${solicitudId}/comentarios`, {
      texto,
      esPublico
    });
    return response.data;
  }
};
