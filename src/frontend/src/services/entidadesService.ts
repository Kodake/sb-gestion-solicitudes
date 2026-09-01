import { api } from './api';
import { ApiResponse, IEntidadGubernamental, PaginatedList } from '@/types';

export interface FiltrosEntidadParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  categoria?: string;
  poderEstado?: string;
  sector?: string;
  activo?: boolean;
}

export interface GuardarEntidadData {
  nombre: string;
  categoria: string;
  poderEstado: string;
  sector: string;
  activo?: boolean;
}

export const entidadesService = {
  async getEntidades(params?: FiltrosEntidadParams): Promise<ApiResponse<PaginatedList<IEntidadGubernamental>>> {
    const response = await api.get<ApiResponse<PaginatedList<IEntidadGubernamental>>>('/entidades-gubernamentales', { params });
    return response.data;
  },

  async getEntidadById(id: number): Promise<ApiResponse<IEntidadGubernamental>> {
    const response = await api.get<ApiResponse<IEntidadGubernamental>>(`/entidades-gubernamentales/${id}`);
    return response.data;
  },

  async crearEntidad(data: GuardarEntidadData): Promise<ApiResponse<IEntidadGubernamental>> {
    const response = await api.post<ApiResponse<IEntidadGubernamental>>('/entidades-gubernamentales', data);
    return response.data;
  },

  async actualizarEntidad(id: number, data: GuardarEntidadData): Promise<ApiResponse<IEntidadGubernamental>> {
    const response = await api.put<ApiResponse<IEntidadGubernamental>>(`/entidades-gubernamentales/${id}`, data);
    return response.data;
  },

  async toggleEstadoEntidad(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.patch<ApiResponse<boolean>>(`/entidades-gubernamentales/${id}/toggle-activo`);
    return response.data;
  },

  async eliminarEntidad(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.delete<ApiResponse<boolean>>(`/entidades-gubernamentales/${id}`);
    return response.data;
  },

  async getSectores(): Promise<ApiResponse<string[]>> {
    const response = await api.get<ApiResponse<string[]>>('/entidades-gubernamentales/sectores');
    return response.data;
  },

  async getPoderes(): Promise<ApiResponse<string[]>> {
    const response = await api.get<ApiResponse<string[]>>('/entidades-gubernamentales/poderes');
    return response.data;
  },

  async getCategorias(): Promise<ApiResponse<string[]>> {
    const response = await api.get<ApiResponse<string[]>>('/entidades-gubernamentales/categorias');
    return response.data;
  }
};
