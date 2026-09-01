import { api } from './api';
import { ApiResponse, Area, TipoSolicitud, User } from '@/types';

export interface GuardarAreaDto {
  nombre: string;
  descripcion?: string;
  activa?: boolean;
}

export interface GuardarTipoSolicitudDto {
  nombre: string;
  descripcion?: string;
  activo?: boolean;
}

export const catalogosService = {
  // Áreas
  async getAreas(soloActivas?: boolean): Promise<ApiResponse<Area[]>> {
    const response = await api.get<ApiResponse<Area[]>>('/catalogos/areas', {
      params: { soloActivas }
    });
    return response.data;
  },

  async crearArea(data: GuardarAreaDto): Promise<ApiResponse<Area>> {
    const response = await api.post<ApiResponse<Area>>('/catalogos/areas', data);
    return response.data;
  },

  async actualizarArea(id: number, data: GuardarAreaDto): Promise<ApiResponse<Area>> {
    const response = await api.put<ApiResponse<Area>>(`/catalogos/areas/${id}`, data);
    return response.data;
  },

  async toggleEstadoArea(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.patch<ApiResponse<boolean>>(`/catalogos/areas/${id}/toggle-activo`);
    return response.data;
  },

  // Tipos de Solicitud
  async getTiposSolicitud(soloActivos?: boolean): Promise<ApiResponse<TipoSolicitud[]>> {
    const response = await api.get<ApiResponse<TipoSolicitud[]>>('/catalogos/tipos-solicitud', {
      params: { soloActivos }
    });
    return response.data;
  },

  async crearTipoSolicitud(data: GuardarTipoSolicitudDto): Promise<ApiResponse<TipoSolicitud>> {
    const response = await api.post<ApiResponse<TipoSolicitud>>('/catalogos/tipos-solicitud', data);
    return response.data;
  },

  async actualizarTipoSolicitud(id: number, data: GuardarTipoSolicitudDto): Promise<ApiResponse<TipoSolicitud>> {
    const response = await api.put<ApiResponse<TipoSolicitud>>(`/catalogos/tipos-solicitud/${id}`, data);
    return response.data;
  },

  async toggleEstadoTipoSolicitud(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.patch<ApiResponse<boolean>>(`/catalogos/tipos-solicitud/${id}/toggle-activo`);
    return response.data;
  },

  // Usuarios para selector
  async getUsuarios(rol?: number): Promise<ApiResponse<User[]>> {
    const response = await api.get<ApiResponse<User[]>>('/catalogos/usuarios', {
      params: { rol }
    });
    return response.data;
  }
};
