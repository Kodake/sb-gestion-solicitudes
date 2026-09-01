'use client';

import React, { useState, useEffect, useRef, useCallback } from 'react';
import { Bell, CheckCircle2, Clock, Trash2, X, ExternalLink, RefreshCw } from 'lucide-react';
import { notificacionesService } from '@/services/notificacionesService';
import { INotificacion } from '@/types';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks';

export const NotificationBell: React.FC = () => {
  const router = useRouter();
  const { user } = useAuth();
  const [notificaciones, setNotificaciones] = useState<INotificacion[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Clean up any legacy dismissed_ids keys from browser localStorage
  useEffect(() => {
    if (typeof window !== 'undefined') {
      try {
        Object.keys(localStorage).forEach((key) => {
          if (key.startsWith('sb_dismissed_ids')) {
            localStorage.removeItem(key);
          }
        });
      } catch {
        // Ignore
      }
    }
  }, []);

  const fetchNotificaciones = useCallback(async () => {
    if (!user) return;
    try {
      const res = await notificacionesService.getNotificaciones();
      if (res.success && Array.isArray(res.data)) {
        setNotificaciones(res.data);
      }
    } catch (err) {
      console.error('Error fetching notifications:', err);
    }
  }, [user]);

  // Fetch notifications on mount and fast polling every 3 seconds for real-time responsiveness
  useEffect(() => {
    fetchNotificaciones();
    const interval = setInterval(fetchNotificaciones, 3000);
    return () => clearInterval(interval);
  }, [fetchNotificaciones]);

  // Listen to window custom events to refresh notifications immediately after any action
  useEffect(() => {
    const handleNotificationRefresh = () => {
      fetchNotificaciones();
    };

    window.addEventListener('notification-refresh', handleNotificationRefresh);
    window.addEventListener('focus', handleNotificationRefresh);
    return () => {
      window.removeEventListener('notification-refresh', handleNotificationRefresh);
      window.removeEventListener('focus', handleNotificationRefresh);
    };
  }, [fetchNotificaciones]);

  // Close dropdown on click outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const totalCount = notificaciones.length;

  const handleSelectNotificacion = (n: INotificacion) => {
    setIsOpen(false);
    if (n.solicitudId) {
      router.push(`/solicitudes/${n.solicitudId}`);
    }
  };

  const handleEliminar = async (e: React.MouseEvent, id: number) => {
    e.stopPropagation();
    try {
      setDeletingId(id);
      // Optimistic update
      setNotificaciones((prev) => prev.filter((n) => n.id !== id));
      await notificacionesService.eliminarNotificacion(id);
    } catch (err) {
      console.error('Error eliminando notificación:', err);
      // Rollback on error
      fetchNotificaciones();
    } finally {
      setDeletingId(null);
    }
  };

  const handleLimpiarTodas = async (e: React.MouseEvent) => {
    e.stopPropagation();
    if (notificaciones.length === 0) return;
    try {
      setLoading(true);
      setNotificaciones([]);
      await notificacionesService.limpiarTodas();
    } catch (err) {
      console.error('Error limpiando notificaciones:', err);
      fetchNotificaciones();
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="relative" ref={dropdownRef}>
      {/* Bell Trigger Button */}
      <button
        onClick={() => {
          const nextState = !isOpen;
          setIsOpen(nextState);
          if (nextState) {
            setLoading(true);
            fetchNotificaciones().finally(() => setLoading(false));
          }
        }}
        title={`Bandeja de Notificaciones (${totalCount} activas)`}
        className="relative p-2 sm:p-2.5 rounded-xl bg-[#133852] hover:bg-[#184668] text-white border border-[#1d4b6c] hover:border-slate-300/60 shadow-sm transition-all duration-200 active:scale-95 cursor-pointer flex items-center justify-center group"
        aria-label="Notificaciones"
      >
        <Bell className="w-4 h-4 sm:w-5 sm:h-5 text-amber-300 group-hover:scale-110 transition-transform" />

        {/* Real-time Badge Indicator */}
        {totalCount > 0 ? (
          <span className="absolute -top-1.5 -right-1.5 bg-[#E64A19] text-white text-[10px] font-extrabold px-1.5 py-0.2 rounded-full ring-2 ring-[#0D3048] shadow-md animate-bounce">
            {totalCount > 9 ? '9+' : totalCount}
          </span>
        ) : (
          <span className="absolute top-1.5 right-1.5 w-2 h-2 rounded-full bg-slate-500/50"></span>
        )}
      </button>

      {/* Dropdown Panel */}
      {isOpen && (
        <div className="absolute right-0 mt-2 w-80 sm:w-96 bg-white rounded-2xl shadow-2xl border border-slate-200 text-slate-800 z-50 overflow-hidden animate-in fade-in slide-from-top-2 duration-200">
          {/* Header */}
          <div className="bg-[#0D3048] text-white p-4 flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Bell className="w-4 h-4 text-amber-300" />
              <h3 className="text-xs sm:text-sm font-bold">Bandeja de Notificaciones</h3>
            </div>

            <div className="flex items-center gap-2">
              <span className="bg-[#194260] text-amber-300 text-[11px] font-semibold px-2 py-0.5 rounded-full">
                {totalCount} {totalCount === 1 ? 'notificación' : 'notificaciones'}
              </span>

              {totalCount > 0 && (
                <button
                  onClick={handleLimpiarTodas}
                  title="Eliminar todas las notificaciones"
                  className="p-1 rounded-lg bg-red-500/20 hover:bg-red-500/40 text-red-300 hover:text-white transition-colors cursor-pointer"
                >
                  <Trash2 className="w-3.5 h-3.5" />
                </button>
              )}
            </div>
          </div>

          {/* Body */}
          <div className="max-h-80 overflow-y-auto divide-y divide-slate-100 custom-scrollbar">
            {loading && notificaciones.length === 0 ? (
              <div className="p-8 text-center space-y-2">
                <RefreshCw className="w-6 h-6 text-slate-400 animate-spin mx-auto" />
                <p className="text-xs text-slate-500">Cargando notificaciones...</p>
              </div>
            ) : notificaciones.length === 0 ? (
              <div className="p-8 text-center space-y-2">
                <CheckCircle2 className="w-8 h-8 text-slate-300 mx-auto" />
                <p className="text-xs font-semibold text-slate-600">No tienes notificaciones pendientes</p>
                <p className="text-[11px] text-slate-400">
                  Todas tus alertas y cambios de estado generados aparecerán aquí.
                </p>
              </div>
            ) : (
              notificaciones.map((n) => (
                <div
                  key={n.id}
                  onClick={() => handleSelectNotificacion(n)}
                  className="relative p-3.5 hover:bg-slate-50 transition-colors cursor-pointer group space-y-1 pr-9"
                >
                  {/* Delete Single Notification Button */}
                  <button
                    onClick={(e) => handleEliminar(e, n.id)}
                    disabled={deletingId === n.id}
                    title="Eliminar notificación"
                    className="absolute top-3 right-2.5 p-1 rounded-full text-slate-400 hover:text-red-600 hover:bg-red-50 transition-all opacity-80 group-hover:opacity-100 cursor-pointer"
                  >
                    <X className="w-3.5 h-3.5" />
                  </button>

                  <div className="flex items-start justify-between gap-2">
                    <span className="text-xs font-bold text-[#0D3048] group-hover:text-blue-700 transition-colors line-clamp-1 pr-2">
                      {n.asunto}
                    </span>
                  </div>

                  <p className="text-xs text-slate-600 line-clamp-2 leading-relaxed">{n.mensaje}</p>

                  <div className="pt-1 flex items-center justify-between text-[10px] text-slate-400 font-mono">
                    <span className="flex items-center gap-1">
                      <Clock className="w-3 h-3 text-slate-400" />
                      {new Date(n.fecha).toLocaleDateString('es-DO', {
                        month: 'short',
                        day: 'numeric',
                        hour: '2-digit',
                        minute: '2-digit',
                      })}
                    </span>

                    {n.solicitudId && (
                      <span className="flex items-center gap-1 font-semibold text-[#E64A19] group-hover:underline">
                        <span>Ver solicitud</span>
                        <ExternalLink className="w-2.5 h-2.5" />
                      </span>
                    )}
                  </div>
                </div>
              ))
            )}
          </div>

          {/* Footer */}
          <div className="bg-slate-50 p-2.5 flex items-center justify-between border-t border-slate-100 text-xs px-3">
            <button
              onClick={() => {
                setLoading(true);
                fetchNotificaciones().finally(() => setLoading(false));
              }}
              className="inline-flex items-center gap-1 text-[11px] text-slate-500 hover:text-[#0D3048] font-medium transition-colors cursor-pointer"
            >
              <RefreshCw className={`w-3 h-3 ${loading ? 'animate-spin' : ''}`} />
              <span>Actualizar</span>
            </button>

            <button
              onClick={() => setIsOpen(false)}
              className="text-xs text-slate-500 hover:text-slate-800 font-medium cursor-pointer"
            >
              Cerrar panel
            </button>
          </div>
        </div>
      )}
    </div>
  );
};
