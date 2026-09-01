'use client';

import React from 'react';
import { usePathname } from 'next/navigation';
import { useAuth, useLogout, useDisclosure } from '@/hooks';
import { Shield, Menu, Mail, CheckCircle2, Key, LogOut } from 'lucide-react';
import { Modal } from '@/components/ui/Modal';
import { NotificationBell } from '@/components/ui/NotificationBell';
import { RolEnum } from '@/types';

interface HeaderProps {
  onToggleMobileSidebar?: () => void;
}

export const Header: React.FC<HeaderProps> = ({ onToggleMobileSidebar }) => {
  const pathname = usePathname();
  const { user } = useAuth();
  const { isOpen: isProfileModalOpen, open: openProfileModal, close: closeProfileModal } = useDisclosure(false);
  const { isLogoutModalOpen, openLogoutModal, closeLogoutModal, confirmLogout } = useLogout();

  const isAdmin = user?.rol === RolEnum.Administrador || user?.rolNombre === 'Administrador';
  const isAnalyst = user?.rol === RolEnum.Analista || user?.rolNombre === 'Analista';

  const getPageTitle = () => {
    const current = (pathname || '').replace(/\/+$/, '') || '/';
    if (current === '/dashboard') return 'Tablero de Control';
    if (current === '/solicitudes') return 'Consulta de Solicitudes';
    if (current === '/solicitudes/nueva') return 'Registrar Nueva Solicitud';
    if (current.startsWith('/solicitudes/')) return 'Detalle de Solicitud';
    if (current === '/admin/entidades') return 'Mantenimiento de Entidades Públicas';
    if (current === '/admin/catalogos') return 'Mantenimiento de Catálogos';
    if (current === '/admin/usuarios') return 'Gestión de Usuarios y Accesos';
    return 'Sistema de Gestión de Solicitudes';
  };

  const getRolePermissions = () => {
    if (isAdmin) {
      return [
        'Acceso total a catálogos, áreas y usuarios',
        'Gestión y asignación de analistas responsables',
        'Cambio de estados y reapertura de solicitudes cerradas',
        'Publicación de comentarios públicos e internos',
        'Visualización completa de métricas e indicadores SLA'
      ];
    }
    if (isAnalyst) {
      return [
        'Gestión técnica y atención de solicitudes asignadas',
        'Cambio de estado y actualización del ciclo de vida',
        'Asignación y reasignación de analistas',
        'Publicación de comentarios públicos y notas internas',
        'Consulta de trazabilidad e historial de auditoría'
      ];
    }
    return [
      'Registro de nuevas solicitudes de soporte tecnológico',
      'Consulta exclusiva de requerimientos propios',
      'Seguimiento a la línea de tiempo de estados',
      'Publicación de comentarios públicos al analista'
    ];
  };

  return (
    <>
      <header className="min-h-16 lg:h-20 bg-[#0D3048] text-white flex items-center justify-between px-4 sm:px-6 lg:px-8 border-b border-[#194260] py-3 lg:py-0 select-none">
        {/* Left: Mobile Menu Toggle & Title */}
        <div className="flex items-center gap-3 min-w-0">
          <button
            onClick={onToggleMobileSidebar}
            className="p-2 rounded-xl bg-[#143B58] text-white hover:bg-[#1b4b6f] lg:hidden transition-colors shrink-0 shadow-sm active:scale-95"
            aria-label="Abrir menú de navegación"
          >
            <Menu className="w-5 h-5" />
          </button>

          <div className="min-w-0">
            <h1 className="text-base sm:text-xl lg:text-2xl font-bold tracking-tight text-white truncate transition-all duration-200">
              {getPageTitle()}
            </h1>
            <p className="text-[11px] sm:text-xs text-slate-300 font-medium truncate hidden sm:block">
              Superintendencia de Bancos - República Dominicana
            </p>
          </div>
        </div>

        {/* Right: Role Chip & Interactive User Profile Badge */}
        <div className="flex items-center gap-2 sm:gap-4 shrink-0">
          {/* Active Role Chip */}
          <div className="hidden md:flex items-center gap-1.5 bg-[#143B58] px-3 py-1.5 rounded-xl border border-[#225075] text-xs shadow-inner">
            <Shield className="w-3.5 h-3.5 text-amber-400" />
            <span className="text-[11px] font-semibold text-slate-300">Rol Activo:</span>
            <span className={`px-2 py-0.5 rounded-md text-[11px] font-bold ${
              isAdmin
                ? 'bg-amber-500 text-slate-950'
                : isAnalyst
                ? 'bg-blue-500 text-white'
                : 'bg-emerald-500 text-white'
            }`}>
              {user?.rolNombre || 'Usuario'}
            </span>
          </div>

          {/* Notification Bell Inbox */}
          <NotificationBell />

          {/* Interactive User Profile Badge with Hover Scale & Click Modal */}
          <button
            onClick={openProfileModal}
            title="Haz clic para ver la información de tu perfil"
            className="group flex items-center gap-2.5 bg-[#133852] hover:bg-[#184668] px-3 sm:px-4 py-1.5 sm:py-2 rounded-xl border border-[#1d4b6c] hover:border-slate-300/60 shadow-sm hover:shadow-md transition-all duration-200 cursor-pointer active:scale-95"
          >
            <div className="w-7 h-7 sm:w-8 sm:h-8 rounded-full bg-[#E64A19] flex items-center justify-center font-bold text-white text-xs shadow-sm ring-2 ring-white/20 shrink-0 group-hover:scale-110 transition-transform duration-200">
              {user?.nombre ? user.nombre.charAt(0).toUpperCase() : 'U'}
            </div>
            <div className="text-left hidden md:block leading-tight">
              <p className="text-xs font-semibold text-white tracking-tight group-hover:text-slate-100 transition-colors">
                {user?.nombre || 'Usuario'}
              </p>
              <p className="text-[10px] text-slate-300 font-mono">
                {user?.rolNombre || 'Autenticado'}
              </p>
            </div>
          </button>
        </div>
      </header>

      {/* MODAL DE PERFIL DE USUARIO */}
      <Modal
        isOpen={isProfileModalOpen}
        onClose={closeProfileModal}
        title="Perfil de Usuario Institucional"
        size="md"
      >
        <div className="space-y-6">
          {/* Header Card */}
          <div className="flex items-center gap-4 bg-gradient-to-br from-slate-50 to-slate-100 p-4 rounded-2xl border border-slate-200">
            <div className="w-16 h-16 rounded-2xl bg-[#0D3048] text-white flex items-center justify-center text-2xl font-black shadow-md border-2 border-[#194260] ring-4 ring-slate-100 shrink-0">
              {user?.nombre ? user.nombre.charAt(0).toUpperCase() : 'U'}
            </div>
            <div className="min-w-0">
              <h4 className="text-base font-bold text-slate-900 truncate">
                {user?.nombre}
              </h4>
              <div className="flex items-center gap-1.5 text-xs text-slate-500 mt-0.5">
                <Mail className="w-3.5 h-3.5 text-slate-400 shrink-0" />
                <span className="truncate">{user?.correo}</span>
              </div>
              <div className="mt-2 inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[11px] font-bold bg-[#0D3048]/10 text-[#0D3048] border border-[#0D3048]/20">
                <Shield className="w-3 h-3 text-[#0D3048]" />
                {user?.rolNombre}
              </div>
            </div>
          </div>

          {/* Details List */}
          <div className="space-y-3 bg-white rounded-xl border border-slate-200 p-4">
            <h5 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">
              Detalles de la Cuenta
            </h5>
            <div className="flex items-center justify-between text-xs py-1 border-b border-slate-100">
              <span className="text-slate-500">ID de Usuario:</span>
              <span className="font-mono font-semibold text-slate-800">#{user?.id}</span>
            </div>
            <div className="flex items-center justify-between text-xs py-1 border-b border-slate-100">
              <span className="text-slate-500">Estado de Cuenta:</span>
              <span className="inline-flex items-center gap-1 text-emerald-600 font-semibold">
                <CheckCircle2 className="w-3.5 h-3.5" /> Activo
              </span>
            </div>
            <div className="flex items-center justify-between text-xs py-1">
              <span className="text-slate-500">Institución:</span>
              <span className="font-semibold text-slate-800">Superintendencia de Bancos</span>
            </div>
          </div>

          {/* Permissions Scope */}
          <div className="space-y-2 bg-slate-50 rounded-xl border border-slate-200 p-4">
            <div className="flex items-center gap-1.5 text-xs font-bold text-slate-700">
              <Key className="w-3.5 h-3.5 text-[#0D3048]" />
              <span>Alcance y Permisos del Rol</span>
            </div>
            <ul className="text-xs text-slate-600 space-y-1.5 pl-5 list-disc marker:text-[#0D3048]">
              {getRolePermissions().map((perm, index) => (
                <li key={index} className="leading-relaxed">{perm}</li>
              ))}
            </ul>
          </div>

          {/* Action Buttons */}
          <div className="flex items-center justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={closeProfileModal}
              className="px-4 py-2 text-xs font-semibold text-slate-600 hover:text-slate-800 hover:bg-slate-100 rounded-xl transition-colors cursor-pointer"
            >
              Cerrar
            </button>
            <button
              type="button"
              onClick={() => {
                closeProfileModal();
                openLogoutModal();
              }}
              className="flex items-center gap-1.5 px-4 py-2 text-xs font-bold text-white bg-red-600 hover:bg-red-700 rounded-xl shadow-sm transition-all duration-150 cursor-pointer active:scale-95"
            >
              <LogOut className="w-3.5 h-3.5" />
              Cerrar Sesión
            </button>
          </div>
        </div>
      </Modal>

      {/* MODAL DE CONFIRMACIÓN DE CIERRE DE SESIÓN */}
      <Modal
        isOpen={isLogoutModalOpen}
        onClose={closeLogoutModal}
        title="Confirmar Cierre de Sesión"
        size="sm"
      >
        <div className="space-y-4">
          <p className="text-xs text-slate-600 leading-relaxed">
            ¿Está seguro de que desea salir del <strong className="text-slate-800">Sistema de Gestión de Solicitudes</strong>? Deberá ingresar sus credenciales nuevamente para acceder.
          </p>

          <div className="flex items-center justify-end gap-2 pt-2 border-t border-slate-100">
            <button
              type="button"
              onClick={closeLogoutModal}
              className="px-3.5 py-2 text-xs font-semibold text-slate-600 hover:text-slate-800 hover:bg-slate-100 rounded-xl transition-colors cursor-pointer"
            >
              Cancelar
            </button>
            <button
              type="button"
              onClick={() => {
                closeLogoutModal();
                confirmLogout();
              }}
              className="flex items-center gap-1.5 px-4 py-2 text-xs font-bold text-white bg-[#0D3048] hover:bg-red-600 rounded-xl shadow-sm transition-all duration-150 cursor-pointer active:scale-95"
            >
              <LogOut className="w-3.5 h-3.5" />
              Sí, Salir
            </button>
          </div>
        </div>
      </Modal>
    </>
  );
};
