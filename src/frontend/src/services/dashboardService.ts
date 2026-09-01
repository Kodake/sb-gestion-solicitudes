import { api } from './api';
import { ApiResponse, DashboardResumen } from '@/types';

export const dashboardService = {
  async getResumen(): Promise<ApiResponse<DashboardResumen>> {
    const response = await api.get<ApiResponse<DashboardResumen>>('/dashboard/resumen');
    return response.data;
  }
};
