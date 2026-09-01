import { api } from './api';
import { ApiResponse, INotificacion } from '@/types';

export const notificacionesService = {
  async getNotificaciones(): Promise<ApiResponse<INotificacion[]>> {
    try {
      const response = await api.get<ApiResponse<INotificacion[]>>('/notificaciones');
      return response.data;
    } catch (err: any) {
      return {
        success: false,
        message: err?.response?.data?.message || 'Error al obtener notificaciones',
        data: [],
        errors: [err?.message || 'Error de conexión'],
        timestamp: new Date().toISOString()
      };
    }
  },

  async eliminarNotificacion(id: number): Promise<ApiResponse<boolean>> {
    try {
      const response = await api.delete<ApiResponse<boolean>>(`/notificaciones/${id}`);
      return response.data;
    } catch (err: any) {
      return {
        success: false,
        message: err?.response?.data?.message || 'Notificación eliminada localmente.',
        data: false,
        errors: [err?.message || 'Error de conexión'],
        timestamp: new Date().toISOString()
      };
    }
  },

  async limpiarTodas(): Promise<ApiResponse<boolean>> {
    try {
      const response = await api.delete<ApiResponse<boolean>>('/notificaciones');
      return response.data;
    } catch (err: any) {
      return {
        success: false,
        message: err?.response?.data?.message || 'Notificaciones limpiadas localmente.',
        data: false,
        errors: [err?.message || 'Error de conexión'],
        timestamp: new Date().toISOString()
      };
    }
  }
};
