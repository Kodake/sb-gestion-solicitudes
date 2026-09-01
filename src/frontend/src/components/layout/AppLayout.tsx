'use client';

import React, { useState } from 'react';
import { usePathname } from 'next/navigation';
import { Sidebar } from './Sidebar';
import { Header } from './Header';
import { useAuth } from '@/context/AuthContext';
import { Toaster } from 'react-hot-toast';
import { ToastCenter } from '@/components/ui/ToastCenter';

export const AppLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const pathname = usePathname();
  const { isAuthenticated, loading } = useAuth();
  const [isMobileSidebarOpen, setIsMobileSidebarOpen] = useState(false);

  const isLoginPage = !pathname || pathname === '/login' || pathname === '/login/' || pathname.startsWith('/login');

  React.useEffect(() => {
    if (!loading && !isAuthenticated && !isLoginPage) {
      window.location.href = '/login';
    }
  }, [loading, isAuthenticated, isLoginPage]);

  if (loading) {
    return (
      <div className="min-h-screen bg-[#0D3048] flex items-center justify-center text-white p-4">
        <div className="flex flex-col items-center gap-3 animate-pulse">
          <div className="w-10 h-10 border-4 border-white/20 border-t-white rounded-full animate-spin"></div>
          <p className="text-xs font-semibold text-slate-300 tracking-wider uppercase">Cargando plataforma...</p>
        </div>
      </div>
    );
  }

  if (isLoginPage) {
    return (
      <main className="min-h-screen bg-[#0D3048] relative">
        {children}
        <ToastCenter />
        <Toaster position="top-right" containerStyle={{ display: 'none' }} />
      </main>
    );
  }

  if (!isAuthenticated) {
    return null;
  }

  return (
    <div className="flex min-h-screen bg-[#0D3048] relative overflow-x-hidden">
      {/* Sidebar matching Maqueta.jpeg with Mobile First drawer */}
      <Sidebar
        isMobileOpen={isMobileSidebarOpen}
        onCloseMobile={() => setIsMobileSidebarOpen(false)}
      />

      {/* Main Wrapper */}
      <div className="flex-1 flex flex-col min-w-0 bg-[#EDF0F7]">
        {/* Header */}
        <Header onToggleMobileSidebar={() => setIsMobileSidebarOpen(true)} />

        {/* Content Area with Mobile First padding */}
        <main className="flex-1 p-3 sm:p-6 md:p-8 overflow-y-auto">
          <div className="bg-white rounded-2xl sm:rounded-[24px] p-4 sm:p-6 md:p-8 shadow-sm border border-slate-200/80 min-h-[calc(100vh-100px)] sm:min-h-[calc(100vh-140px)] animate-fade-in">
            {children}
          </div>
        </main>
      </div>

      {/* Custom HD Toast Center Component */}
      <ToastCenter />

      {/* React Hot Toast Store Listener */}
      <Toaster position="top-right" containerStyle={{ display: 'none' }} />
    </div>
  );
};
