import { api } from './api';
import { ApiResponse, IUsuarioGestion, PaginatedList, RolEnum } from '@/types';

export interface FiltrosUsuarioParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  rol?: RolEnum;
  activo?: boolean;
}

export interface CrearUsuarioData {
  nombre: string;
  correo: string;
  password: string;
  rol: RolEnum;
  activo?: boolean;
}

export interface ActualizarUsuarioData {
  nombre: string;
  correo: string;
  rol: RolEnum;
  activo: boolean;
  nuevoPassword?: string;
}

export const usuariosService = {
  async getUsuarios(params?: FiltrosUsuarioParams): Promise<ApiResponse<PaginatedList<IUsuarioGestion>>> {
    const response = await api.get<ApiResponse<PaginatedList<IUsuarioGestion>>>('/usuarios', { params });
    return response.data;
  },

  async getUsuarioById(id: number): Promise<ApiResponse<IUsuarioGestion>> {
    const response = await api.get<ApiResponse<IUsuarioGestion>>(`/usuarios/${id}`);
    return response.data;
  },

  async crearUsuario(data: CrearUsuarioData): Promise<ApiResponse<IUsuarioGestion>> {
    const response = await api.post<ApiResponse<IUsuarioGestion>>('/usuarios', data);
    return response.data;
  },

  async actualizarUsuario(id: number, data: ActualizarUsuarioData): Promise<ApiResponse<IUsuarioGestion>> {
    const response = await api.put<ApiResponse<IUsuarioGestion>>(`/usuarios/${id}`, data);
    return response.data;
  },

  async toggleEstadoUsuario(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.patch<ApiResponse<boolean>>(`/usuarios/${id}/toggle-estado`);
    return response.data;
  },

  async eliminarUsuario(id: number): Promise<ApiResponse<boolean>> {
    const response = await api.delete<ApiResponse<boolean>>(`/usuarios/${id}`);
    return response.data;
  }
};
