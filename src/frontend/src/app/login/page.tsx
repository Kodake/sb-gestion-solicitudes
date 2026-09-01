'use client';

import React, { useState } from 'react';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/context/AuthContext';
import { LogIn, Shield, User, UserCheck, AlertCircle } from 'lucide-react';

export default function LoginPage() {
  const [correo, setCorreo] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const { login } = useAuth();
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSubmitting(true);

    const res = await login(correo, password);
    setSubmitting(false);

    if (res.success) {
      router.push('/dashboard');
    } else {
      setError(res.message);
    }
  };

  const handleSelectRole = (email: string) => {
    setError(null);
    setCorreo(email);
    setPassword('');
  };

  return (
    <div className="min-h-screen bg-[#0D3048] flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        {/* Card Principal */}
        <div className="bg-white rounded-2xl shadow-2xl overflow-hidden border border-slate-100">
          {/* Header Institucional */}
          <div className="bg-white p-8 text-center border-b border-slate-100">
            <div className="flex justify-center mb-4">
              <Image
                src="/SUPERINTENDENCIA_DE_BANCOS.png"
                alt="Superintendencia de Bancos"
                width={260}
                height={65}
                priority
                className="h-14 w-auto object-contain"
              />
            </div>
            <h1 className="text-xl font-bold text-slate-800 tracking-tight">
              Gestión de Solicitudes Internas
            </h1>
            <p className="text-xs text-slate-500 mt-1">
              Ingrese sus credenciales de acceso institucional
            </p>
          </div>

          {/* Formulario */}
          <form onSubmit={handleSubmit} className="p-8 space-y-5">
            {error && (
              <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-xl text-xs flex items-center gap-2.5">
                <AlertCircle className="w-4 h-4 text-red-500 shrink-0" />
                <span className="font-medium">{error}</span>
              </div>
            )}

            <div>
              <label className="block text-xs font-semibold text-slate-700 mb-1.5 uppercase tracking-wider">
                Correo Institucional
              </label>
              <input
                type="email"
                required
                value={correo}
                onChange={(e) => setCorreo(e.target.value)}
                placeholder="usuario@sb.gob.do"
                className="w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:border-[#0D3048] focus:ring-2 focus:ring-[#0D3048]/10 text-sm outline-none transition-all"
              />
            </div>

            <div>
              <label className="block text-xs font-semibold text-slate-700 mb-1.5 uppercase tracking-wider">
                Contraseña
              </label>
              <input
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••••••"
                className="w-full px-4 py-2.5 rounded-xl border border-slate-200 focus:border-[#0D3048] focus:ring-2 focus:ring-[#0D3048]/10 text-sm outline-none transition-all"
              />
            </div>

            <button
              type="submit"
              disabled={submitting}
              className="w-full bg-[#0D3048] hover:bg-[#0D3048]/90 text-white font-medium py-2.5 rounded-xl text-sm transition-all duration-200 shadow-md flex items-center justify-center gap-2 disabled:opacity-60"
            >
              {submitting ? (
                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
              ) : (
                <>
                  <LogIn className="w-4 h-4" />
                  Iniciar Sesión
                </>
              )}
            </button>
          </form>

          {/* Selector Rápido de Cuenta */}
          <div className="bg-slate-50 p-6 border-t border-slate-100">
            <p className="text-[11px] font-semibold text-slate-500 uppercase tracking-wider text-center mb-3">
              Seleccionar Cuenta Institucional
            </p>
            <div className="grid grid-cols-3 gap-2">
              <button
                type="button"
                onClick={() => handleSelectRole('admin@sb.gob.do')}
                className="flex flex-col items-center justify-center p-2.5 rounded-xl border border-slate-200 bg-white hover:border-[#0D3048] hover:bg-slate-50 transition-all text-center group"
              >
                <Shield className="w-4 h-4 text-[#0D3048] mb-1 group-hover:scale-110 transition-transform" />
                <span className="text-[11px] font-semibold text-slate-700">Admin</span>
                <span className="text-[9px] text-slate-400">admin@sb</span>
              </button>

              <button
                type="button"
                onClick={() => handleSelectRole('analista.tech@sb.gob.do')}
                className="flex flex-col items-center justify-center p-2.5 rounded-xl border border-slate-200 bg-white hover:border-[#0D3048] hover:bg-slate-50 transition-all text-center group"
              >
                <UserCheck className="w-4 h-4 text-emerald-600 mb-1 group-hover:scale-110 transition-transform" />
                <span className="text-[11px] font-semibold text-slate-700">Analista</span>
                <span className="text-[9px] text-slate-400">analista@sb</span>
              </button>

              <button
                type="button"
                onClick={() => handleSelectRole('juan.perez@sb.gob.do')}
                className="flex flex-col items-center justify-center p-2.5 rounded-xl border border-slate-200 bg-white hover:border-[#0D3048] hover:bg-slate-50 transition-all text-center group"
              >
                <User className="w-4 h-4 text-blue-600 mb-1 group-hover:scale-110 transition-transform" />
                <span className="text-[11px] font-semibold text-slate-700">Solicitante</span>
                <span className="text-[9px] text-slate-400">juan@sb</span>
              </button>
            </div>
          </div>
        </div>

        {/* Footer */}
        <p className="text-center text-white/50 text-[11px] mt-6">
          © 2026 Superintendencia de Bancos de la República Dominicana. Todos los derechos reservados.
        </p>
      </div>
    </div>
  );
}
