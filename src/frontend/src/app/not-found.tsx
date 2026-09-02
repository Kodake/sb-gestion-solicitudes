'use client';

import React from 'react';
import Link from 'next/link';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import {
  Home,
  FileText,
  ArrowLeft,
  Compass,
  Building2,
  HelpCircle,
} from 'lucide-react';

export default function NotFound() {
  const router = useRouter();

  return (
    <div className="min-h-[80vh] flex flex-col items-center justify-center py-10 px-4 text-center animate-in fade-in duration-300">
      {/* Container Card */}
      <div className="w-full max-w-2xl bg-white rounded-3xl p-8 sm:p-12 shadow-xl border border-slate-200/90 relative overflow-hidden">
        {/* Decorative Top Accent Gradient */}
        <div className="absolute top-0 left-0 right-0 h-2 bg-gradient-to-r from-[#0D3048] via-[#00A86B] to-[#E64A19]" />

        {/* Institutional Logo */}
        <div className="flex justify-center mb-6">
          <Image
            src="/SUPERINTENDENCIA_DE_BANCOS.png"
            alt="Superintendencia de Bancos"
            width={240}
            height={60}
            priority
            className="h-12 w-auto object-contain"
          />
        </div>

        {/* 404 Large Visual Badge */}
        <div className="relative inline-flex items-center justify-center my-4">
          <div className="text-8xl sm:text-9xl font-black text-slate-100 tracking-tighter select-none font-mono">
            404
          </div>
          <div className="absolute inset-0 flex items-center justify-center">
            <div className="p-4 bg-[#0D3048] text-white rounded-2xl shadow-lg border border-[#1d4b6c] transform -rotate-6 hover:rotate-0 transition-transform duration-300">
              <Compass className="w-10 h-10 text-amber-300 animate-spin-slow" />
            </div>
          </div>
        </div>

        {/* Headings & Descriptions */}
        <div className="space-y-3 max-w-lg mx-auto">
          <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider bg-amber-50 text-amber-700 border border-amber-200">
            <HelpCircle className="w-3.5 h-3.5 text-amber-600" />
            <span>Recurso no disponible</span>
          </span>

          <h1 className="text-2xl sm:text-3xl font-black text-[#0D3048] tracking-tight">
            Página No Encontrada
          </h1>

          <p className="text-xs sm:text-sm text-slate-600 leading-relaxed">
            La ruta o solicitud a la que intentas acceder no existe, ha sido movida o no se encuentra disponible en el sistema institucional.
          </p>
        </div>

        {/* Action Buttons */}
        <div className="mt-8 flex flex-col sm:flex-row items-center justify-center gap-3">
          <button
            onClick={() => router.back()}
            className="w-full sm:w-auto inline-flex items-center justify-center gap-2 px-5 py-3 rounded-xl border border-slate-300 text-slate-700 hover:bg-slate-50 text-xs font-bold shadow-xs transition-all active:scale-95 cursor-pointer"
          >
            <ArrowLeft className="w-4 h-4 text-slate-500" />
            <span>Volver Atrás</span>
          </button>

          <Link
            href="/dashboard"
            className="w-full sm:w-auto inline-flex items-center justify-center gap-2 px-6 py-3 rounded-xl bg-[#0D3048] hover:bg-[#143B58] text-white text-xs font-bold shadow-md transition-all active:scale-95 cursor-pointer hover:shadow-lg"
          >
            <Home className="w-4 h-4 text-amber-300" />
            <span>Ir al Inicio / Dashboard</span>
          </Link>

          <Link
            href="/solicitudes"
            className="w-full sm:w-auto inline-flex items-center justify-center gap-2 px-5 py-3 rounded-xl bg-slate-100 hover:bg-slate-200 text-slate-800 text-xs font-bold transition-all active:scale-95 cursor-pointer"
          >
            <FileText className="w-4 h-4 text-[#0D3048]" />
            <span>Ver Solicitudes</span>
          </Link>
        </div>

        {/* Quick Institutional Access Links */}
        <div className="mt-10 pt-6 border-t border-slate-100 grid grid-cols-1 sm:grid-cols-3 gap-2 text-left">
          <Link
            href="/dashboard"
            className="p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-200 transition-colors group block"
          >
            <div className="flex items-center gap-2 font-bold text-xs text-[#0D3048] group-hover:text-blue-700">
              <Home className="w-3.5 h-3.5 text-slate-400 group-hover:text-blue-600" />
              <span>Dashboard</span>
            </div>
            <p className="text-[11px] text-slate-500 mt-0.5">Métricas y KPIs operativos</p>
          </Link>

          <Link
            href="/solicitudes/nueva"
            className="p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-200 transition-colors group block"
          >
            <div className="flex items-center gap-2 font-bold text-xs text-[#0D3048] group-hover:text-blue-700">
              <FileText className="w-3.5 h-3.5 text-slate-400 group-hover:text-blue-600" />
              <span>Nueva Solicitud</span>
            </div>
            <p className="text-[11px] text-slate-500 mt-0.5">Crear requerimiento de servicio</p>
          </Link>

          <Link
            href="/admin/entidades"
            className="p-3 rounded-xl hover:bg-slate-50 border border-transparent hover:border-slate-200 transition-colors group block"
          >
            <div className="flex items-center gap-2 font-bold text-xs text-[#0D3048] group-hover:text-blue-700">
              <Building2 className="w-3.5 h-3.5 text-slate-400 group-hover:text-blue-600" />
              <span>Entidades</span>
            </div>
            <p className="text-[11px] text-slate-500 mt-0.5">Catálogo público oficial</p>
          </Link>
        </div>
      </div>
    </div>
  );
}
