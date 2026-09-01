'use client';

import React, { createContext, useContext, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { User } from '@/types';
import { authService } from '@/services/authService';
import toast from 'react-hot-toast';

interface IAuthContext {
  user: User | null;
  token: string | null;
  loading: boolean;
  login: (correo: string, pass: string) => Promise<{ success: boolean; message: string }>;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<IAuthContext | undefined>(undefined);

const LOGOUT_DELAY_MS = 3500;
const TOAST_SUCCESS_DURATION_MS = 3000;

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const router = useRouter();

  useEffect(() => {
    const savedToken = localStorage.getItem('sb_token');
    const savedUser = localStorage.getItem('sb_user');

    if (savedToken && savedUser) {
      setToken(savedToken);
      try {
        setUser(JSON.parse(savedUser));
      } catch {
        localStorage.removeItem('sb_user');
      }
    }
    setLoading(false);
  }, []);

  const login = async (correo: string, pass: string) => {
    try {
      const res = await authService.login(correo, pass);
      if (res.success && res.data) {
        setToken(res.data.token);
        setUser(res.data.usuario);
        localStorage.setItem('sb_token', res.data.token);
        localStorage.setItem('sb_user', JSON.stringify(res.data.usuario));
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        return { success: true, message: 'Inicio de sesión exitoso' };
      }
      return { success: false, message: res.message || 'Credenciales inválidas' };
    } catch (err: any) {
      return { success: false, message: err?.response?.data?.message || 'Error al conectar con el servidor' };
    }
  };

  const logout = () => {
    toast.loading('Cerrando sesión de forma segura...', {
      id: 'logout-toast',
      style: {
        background: '#0D3048',
        color: '#ffffff',
        fontSize: '13px',
        fontWeight: '600',
        borderRadius: '12px',
        border: '1px solid rgba(255, 255, 255, 0.2)',
      },
    });

    setTimeout(() => {
      toast.success('¡Hasta pronto! Sesión cerrada.', {
        id: 'logout-toast',
        icon: '👋',
        duration: TOAST_SUCCESS_DURATION_MS,
        style: {
          background: '#0D3048',
          color: '#ffffff',
          fontSize: '13px',
          fontWeight: '600',
          borderRadius: '12px',
          border: '1px solid rgba(255, 255, 255, 0.2)',
        },
      });

      setToken(null);
      setUser(null);
      localStorage.removeItem('sb_token');
      localStorage.removeItem('sb_user');
      router.push('/login');
    }, LOGOUT_DELAY_MS);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        loading,
        login,
        logout,
        isAuthenticated: !!token && !!user,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth debe ser utilizado dentro de un AuthProvider');
  }
  return context;
};
