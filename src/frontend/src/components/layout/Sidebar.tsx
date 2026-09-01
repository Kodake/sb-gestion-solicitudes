'use client';

import React from 'react';
import Link from 'next/link';
import Image from 'next/image';
import { usePathname } from 'next/navigation';
import {
  FileSearch,
  FilePlus,
  LogOut,
  X,
  AlertTriangle,
  User as UserIcon,
  Shield,
  CheckCircle2,
  Key,
  Building2,
  Layers,
  Users,
} from 'lucide-react';
import { useAuth, useLogout, useDisclosure } from '@/hooks';
import { Modal } from '@/components/ui/Modal';
import { RolEnum } from '@/types';

interface SidebarProps {
  isMobileOpen?: boolean;
  onCloseMobile?: () => void;
}

export const Sidebar: React.FC<SidebarProps> = ({
  isMobileOpen = false,
  onCloseMobile = () => {},
}) => {
  const pathname = usePathname();
  const { user } = useAuth();
  const { isLogoutModalOpen, openLogoutModal, closeLogoutModal, confirmLogout } = useLogout();
  const { isOpen: isProfileModalOpen, open: openProfileModal, close: closeProfileModal } = useDisclosure(false);

  const isAdmin = user?.rol === RolEnum.Administrador || user?.rolNombre === 'Administrador';

  const navItems = [
    {
      name: 'Inicio',
      href: '/dashboard',
      icon: '/home.svg',
      isSvgIcon: true,
    },
    {
      name: 'Consulta',
      href: '/solicitudes',
      icon: FileSearch,
      isSvgIcon: false,
    },
    {
      name: 'Crear registro',
      href: '/solicitudes/nueva',
      icon: FilePlus,
      isSvgIcon: false,
    },
  ];

  const adminNavItems = [
    {
      name: 'Entidades Públicas',
      href: '/admin/entidades',
      icon: Building2,
    },
    {
      name: 'Catálogos (Áreas/Tipos)',
      href: '/admin/catalogos',
      icon: Layers,
    },
    {
      name: 'Gestión Usuarios',
      href: '/admin/usuarios',
      icon: Users,
    },
  ];

  const checkIsActive = (href: string) => {
    const current = (pathname || '').replace(/\/+$/, '') || '/';
    const target = href.replace(/\/+$/, '') || '/';

    if (target === '/dashboard') {
      return current === '/dashboard';
    }
    if (target === '/solicitudes/nueva') {
      return current === '/solicitudes/nueva';
    }
    if (target === '/solicitudes') {
      return current === '/solicitudes' || (current.startsWith('/solicitudes/') && current !== '/solicitudes/nueva');
    }
    return current.startsWith(target);
  };

  const getRolePermissions = () => {
    switch (user?.rol) {
      case 1:
        return [
          'Acceso total a catálogos, áreas y usuarios',
          'Mantenimiento de entidades gubernamentales',
          'Gestión y asignación de analistas responsables',
          'Cambio de estados y reapertura de solicitudes',
          'Visualización completa de métricas e indicadores',
        ];
      case 2:
        return [
          'Gestión técnica de solicitudes asignadas',
          'Cambio de estado con justificación técnica',
          'Publicación de notas internas y comentarios',
          'Reapertura de solicitudes en canal de soporte',
        ];
      default:
        return [
          'Creación de solicitudes de soporte técnico',
          'Edición de solicitudes en estado inicial',
          'Seguimiento exclusivo a requerimientos propios',
          'Publicación de comentarios al analista',
        ];
    }
  };

  return (
    <>
      {/* Mobile Backdrop Overlay */}
      {isMobileOpen && (
        <div
          onClick={onCloseMobile}
          className="fixed inset-0 bg-slate-950/70 backdrop-blur-sm z-40 lg:hidden transition-opacity duration-300"
          aria-hidden="true"
        />
      )}

      {/* Sidebar Container */}
      <aside
        className={`w-64 bg-[#0D3048] text-white flex flex-col min-h-screen shrink-0 border-r border-[#194260] select-none
          fixed lg:static inset-y-0 left-0 z-50 transform transition-transform duration-300 ease-in-out
          ${isMobileOpen ? 'translate-x-0 shadow-2xl' : '-translate-x-full lg:translate-x-0'}`}
      >
        {/* Brand Header & Mobile Close */}
        <div className="p-4 pb-5 border-b border-[#194260]/60 flex items-center justify-between">
          <Link
            href="/dashboard"
            onClick={onCloseMobile}
            className="w-full bg-white p-3 rounded-2xl shadow-sm border border-slate-100 block transition-all hover:scale-[1.02] hover:shadow-md duration-200"
          >
            <Image
              src="/SUPERINTENDENCIA_DE_BANCOS.png"
              alt="Superintendencia de Bancos"
              width={200}
              height={55}
              className="w-full h-auto object-contain"
              priority
            />
          </Link>

          {/* Close Button for Mobile Drawer */}
          <button
            onClick={onCloseMobile}
            className="p-2 rounded-xl text-slate-300 hover:text-white hover:bg-white/10 lg:hidden transition-colors"
            aria-label="Cerrar menú"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Navigation Menu */}
        <nav className="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
          <span className="text-[10px] font-extrabold uppercase tracking-wider text-slate-400 px-3">Principal</span>
          {navItems.map((item) => {
            const isActive = checkIsActive(item.href);
            return (
              <Link
                key={item.name}
                href={item.href}
                onClick={onCloseMobile}
                className={`group relative flex items-center gap-3.5 px-4 py-3 rounded-xl font-medium text-sm transition-all duration-200 active:scale-[0.98] ${
                  isActive
                    ? 'bg-[#184668] text-white shadow-lg border border-[#275980]'
                    : 'text-slate-300 hover:bg-[#133A57]/80 hover:text-white'
                }`}
              >
                {/* Active Indicator Bar */}
                {isActive && (
                  <span className="absolute left-0 top-1/2 -translate-y-1/2 w-1.5 h-6 bg-[#E64A19] rounded-r-full shadow-sm"></span>
                )}

                {item.isSvgIcon ? (
                  <Image
                    src={item.icon as string}
                    alt={item.name}
                    width={20}
                    height={20}
                    className="w-5 h-5 object-contain transition-transform duration-200 group-hover:scale-110"
                  />
                ) : (
                  (() => {
                    const IconComp = item.icon as any;
                    return (
                      <IconComp className="w-5 h-5 text-slate-300 transition-transform duration-200 group-hover:scale-110 group-hover:text-white" />
                    );
                  })()
                )}
                <span className="font-semibold tracking-wide text-xs sm:text-sm">{item.name}</span>
              </Link>
            );
          })}

          {/* Admin Section */}
          {isAdmin && (
            <div className="pt-4 space-y-2 border-t border-[#194260]/60 mt-4">
              <span className="text-[10px] font-extrabold uppercase tracking-wider text-amber-400 px-3 flex items-center gap-1.5">
                <Shield className="w-3 h-3" />
                <span>Administración</span>
              </span>
              {adminNavItems.map((item) => {
                const isActive = checkIsActive(item.href);
                const IconComp = item.icon;
                return (
                  <Link
                    key={item.name}
                    href={item.href}
                    onClick={onCloseMobile}
                    className={`group relative flex items-center gap-3.5 px-4 py-3 rounded-xl font-medium text-xs sm:text-sm transition-all duration-200 active:scale-[0.98] ${
                      isActive
                        ? 'bg-[#184668] text-white shadow-lg border border-[#275980]'
                        : 'text-slate-300 hover:bg-[#133A57]/80 hover:text-white'
                    }`}
                  >
                    {isActive && (
                      <span className="absolute left-0 top-1/2 -translate-y-1/2 w-1.5 h-6 bg-amber-400 rounded-r-full shadow-sm"></span>
                    )}
                    <IconComp className="w-5 h-5 text-amber-300/80 transition-transform duration-200 group-hover:scale-110 group-hover:text-amber-300" />
                    <span className="font-semibold tracking-wide">{item.name}</span>
                  </Link>
                );
              })}
            </div>
          )}
        </nav>

        {/* User Profile Card Footer */}
        <div className="p-4 m-4 bg-[#133852]/90 backdrop-blur-md rounded-2xl border border-[#1d4b6c] hover:border-amber-300/50 hover:ring-2 hover:ring-amber-300/40 text-xs shadow-lg space-y-3 transition-all duration-200 hover:scale-[1.02] active:scale-95 group cursor-pointer">
          <div onClick={openProfileModal} className="space-y-3">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 rounded-full bg-[#E64A19] text-white flex items-center justify-center font-extrabold text-xs shadow-md ring-2 ring-white/20 group-hover:ring-amber-300/60 group-hover:scale-105 transition-all shrink-0">
                {user?.nombre ? user.nombre.charAt(0).toUpperCase() : 'U'}
              </div>
              <div className="min-w-0 flex-1">
                <p className="font-bold text-white text-xs truncate leading-tight group-hover:text-amber-200 transition-colors">
                  {user?.nombre || 'Usuario'}
                </p>
                <p className="text-slate-300 text-[10px] font-mono truncate mt-0.5">{user?.correo}</p>
              </div>
            </div>

            <div className="flex items-center justify-between pt-1 border-t border-[#1d4b6c]/60">
              <span className="inline-block px-2.5 py-0.5 rounded-full text-[10px] uppercase tracking-wider font-extrabold bg-[#E64A19] text-white shadow-sm">
                {user?.rolNombre || 'Solicitante'}
              </span>
              <span className="text-[10px] font-semibold text-amber-300 group-hover:underline flex items-center gap-1">
                Ver perfil →
              </span>
            </div>
          </div>

          <button
            onClick={(e) => {
              e.stopPropagation();
              openLogoutModal();
            }}
            className="w-full flex items-center justify-center gap-2 px-3 py-2 rounded-xl bg-red-500/15 hover:bg-red-600 text-red-300 hover:text-white border border-red-500/30 hover:border-red-600 text-xs font-semibold shadow-sm transition-all duration-200 active:scale-95 cursor-pointer"
          >
            <LogOut className="w-4 h-4" />
            <span>Cerrar Sesión</span>
          </button>
        </div>
      </aside>

      {/* Modal: Perfil del Usuario Autenticado */}
      <Modal
        isOpen={isProfileModalOpen}
        onClose={closeProfileModal}
        title="Perfil del Usuario Autenticado"
      >
        <div className="space-y-6 pt-1">
          <div className="p-5 rounded-2xl bg-gradient-to-br from-[#0D3048] to-[#143B58] text-white flex items-center gap-4 border border-[#194260] shadow-md">
            <div className="w-14 h-14 rounded-2xl bg-[#E64A19] text-white flex items-center justify-center font-black text-xl shadow-lg ring-4 ring-white/10 shrink-0">
              {user?.nombre ? user.nombre.charAt(0).toUpperCase() : 'U'}
            </div>
            <div className="space-y-1 min-w-0">
              <h3 className="text-base font-extrabold text-white leading-tight truncate">{user?.nombre}</h3>
              <p className="text-xs text-slate-300 font-mono truncate">{user?.correo}</p>
              <div className="pt-1 flex items-center gap-2">
                <span className="inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] uppercase font-extrabold bg-[#E64A19] text-white shadow-xs">
                  <Shield className="w-3 h-3" />
                  <span>{user?.rolNombre || 'Usuario'}</span>
                </span>
                <span className="text-[10px] font-bold text-emerald-400 bg-emerald-950/60 px-2 py-0.5 rounded-full border border-emerald-500/30 flex items-center gap-1">
                  <CheckCircle2 className="w-3 h-3" /> JWT Activo
                </span>
              </div>
            </div>
          </div>

          <div className="p-4 rounded-xl bg-slate-50 border border-slate-200/80 space-y-3">
            <h4 className="text-xs font-bold text-[#0D3048] uppercase tracking-wider flex items-center gap-2">
              <Key className="w-4 h-4 text-blue-600" />
              <span>Capacidades de tu Rol en el Sistema</span>
            </h4>
            <ul className="space-y-2 text-xs text-slate-700 font-medium">
              {getRolePermissions().map((perm, idx) => (
                <li key={idx} className="flex items-start gap-2">
                  <CheckCircle2 className="w-3.5 h-3.5 text-emerald-600 shrink-0 mt-0.5" />
                  <span>{perm}</span>
                </li>
              ))}
            </ul>
          </div>

          <div className="pt-3 border-t border-slate-100 flex items-center justify-between">
            <button
              onClick={() => {
                closeProfileModal();
                openLogoutModal();
              }}
              className="inline-flex items-center gap-1.5 px-4 py-2 rounded-xl bg-red-50 text-red-700 hover:bg-red-600 hover:text-white text-xs font-semibold border border-red-200 transition-all active:scale-95 cursor-pointer"
            >
              <LogOut className="w-3.5 h-3.5" />
              <span>Cerrar Sesión</span>
            </button>

            <button
              type="button"
              onClick={closeProfileModal}
              className="px-5 py-2 rounded-xl bg-[#0D3048] hover:bg-[#133A57] text-white text-xs font-semibold shadow transition-all active:scale-95"
            >
              Cerrar Ventana
            </button>
          </div>
        </div>
      </Modal>

      {/* Modal: Confirmar Cierre de Sesión */}
      <Modal
        isOpen={isLogoutModalOpen}
        onClose={closeLogoutModal}
        title="Confirmar Cierre de Sesión"
      >
        <div className="space-y-4 pt-1">
          <div className="flex items-start gap-3.5 p-4 rounded-xl bg-amber-50 border border-amber-200 text-amber-900">
            <AlertTriangle className="w-5 h-5 text-amber-600 shrink-0 mt-0.5" />
            <div className="text-xs space-y-1">
              <p className="font-bold text-slate-900">¿Está seguro que desea cerrar su sesión?</p>
              <p className="text-slate-600 leading-normal">
                Se finalizará su sesión de trabajo actual en el Sistema de Gestión de Solicitudes de la Superintendencia de Bancos.
              </p>
            </div>
          </div>

          <div className="pt-3 border-t border-slate-100 flex items-center justify-end gap-2">
            <button
              type="button"
              onClick={closeLogoutModal}
              className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold hover:bg-slate-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="button"
              onClick={() => confirmLogout(onCloseMobile)}
              className="inline-flex items-center gap-1.5 px-5 py-2 rounded-xl bg-red-600 hover:bg-red-700 text-white text-xs font-semibold shadow transition-all active:scale-95"
            >
              <LogOut className="w-3.5 h-3.5" />
              <span>Confirmar Salida</span>
            </button>
          </div>
        </div>
      </Modal>
    </>
  );
};
