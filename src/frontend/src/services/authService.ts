import { api } from './api';
import { ApiResponse, LoginResponse, User } from '@/types';

export const authService = {
  async login(correo: string, password: string): Promise<ApiResponse<LoginResponse>> {
    const response = await api.post<ApiResponse<LoginResponse>>('/auth/login', { correo, password });
    return response.data;
  },

  async getMe(): Promise<ApiResponse<User>> {
    const response = await api.get<ApiResponse<User>>('/auth/me');
    return response.data;
  }
};
