'use client';

import React from 'react';
import Link from 'next/link';
import { ArrowLeft, Save, AlertCircle } from 'lucide-react';
import { PrioridadEnum } from '@/types';
import { useNuevaSolicitud } from '@/hooks';

export default function NuevaSolicitudPage() {
  const {
    areas,
    tipos,
    loading,
    submitting,
    error,
    formData,
    updateFormField,
    handleSubmit,
  } = useNuevaSolicitud();

  if (loading) {
    return (
      <div className="flex items-center justify-center py-20">
        <div className="w-8 h-8 border-4 border-[#0D3048]/20 border-t-[#0D3048] rounded-full animate-spin"></div>
      </div>
    );
  }

  return (
    <div className="max-w-3xl mx-auto space-y-6">
      {/* Top Navigation Bar */}
      <div className="flex items-center justify-between pb-4 border-b border-slate-100">
        <div className="flex items-center gap-3">
          <Link
            href="/solicitudes"
            className="p-2 rounded-xl border border-slate-200 text-slate-500 hover:text-[#0D3048] hover:bg-slate-50 transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
          </Link>
          <div>
            <h2 className="text-xl font-bold text-[#0D3048]">Registrar Nueva Solicitud</h2>
            <p className="text-xs text-slate-500">Formulario de requerimiento de soporte o servicio técnico</p>
          </div>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-red-50 border border-red-200 rounded-xl flex items-start gap-3 text-red-700 text-xs font-medium">
          <AlertCircle className="w-4 h-4 shrink-0 mt-0.5" />
          <span>{error}</span>
        </div>
      )}

      {/* Main Form */}
      <form onSubmit={handleSubmit} className="bg-white p-6 md:p-8 rounded-2xl border border-slate-200/80 shadow-sm space-y-6">
        {/* Title */}
        <div>
          <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
            Título de la Solicitud <span className="text-red-500">*</span>
          </label>
          <input
            type="text"
            required
            maxLength={150}
            value={formData.titulo}
            onChange={(e) => updateFormField('titulo', e.target.value)}
            placeholder="Ejemplo: Acceso al módulo de reportes bancarios consolidados"
            className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] focus:ring-2 focus:ring-[#0D3048]/20 outline-none transition-all"
          />
        </div>

        {/* Grid Area & Request Type */}
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Área Solicitante <span className="text-red-500">*</span>
            </label>
            <select
              required
              value={formData.areaId}
              onChange={(e) => updateFormField('areaId', Number(e.target.value))}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none text-slate-700 bg-white"
            >
              {areas.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.nombre}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Tipo de Solicitud <span className="text-red-500">*</span>
            </label>
            <select
              required
              value={formData.tipoSolicitudId}
              onChange={(e) => updateFormField('tipoSolicitudId', Number(e.target.value))}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none text-slate-700 bg-white"
            >
              {tipos.map((t) => (
                <option key={t.id} value={t.id}>
                  {t.nombre}
                </option>
              ))}
            </select>
          </div>
        </div>

        {/* Priority & Commitment Date */}
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Prioridad Requerida <span className="text-red-500">*</span>
            </label>
            <select
              value={formData.prioridad}
              onChange={(e) => updateFormField('prioridad', Number(e.target.value))}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none text-slate-700 bg-white"
            >
              <option value={PrioridadEnum.Baja}>Baja</option>
              <option value={PrioridadEnum.Media}>Media</option>
              <option value={PrioridadEnum.Alta}>Alta</option>
              <option value={PrioridadEnum.Critica}>Crítica</option>
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Fecha Compromiso (Opcional)
            </label>
            <input
              type="date"
              value={formData.fechaCompromiso}
              onChange={(e) => updateFormField('fechaCompromiso', e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none text-slate-700"
            />
          </div>
        </div>

        {/* Description */}
        <div>
          <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
            Descripción Detallada <span className="text-red-500">*</span>
          </label>
          <textarea
            required
            rows={5}
            maxLength={2000}
            value={formData.descripcion}
            onChange={(e) => updateFormField('descripcion', e.target.value)}
            placeholder="Describa claramente la solicitud, justificación del requerimiento o detalles técnicos necesarios..."
            className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] focus:ring-2 focus:ring-[#0D3048]/20 outline-none transition-all resize-none"
          />
        </div>

        {/* Evidence Reference */}
        <div>
          <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
            Referencia de Evidencia (URL o Texto Libre)
          </label>
          <input
            type="text"
            maxLength={500}
            value={formData.referenciaEvidencia}
            onChange={(e) => updateFormField('referenciaEvidencia', e.target.value)}
            placeholder="Ejemplo: Ticket previo #8821 o URL de documentación interna"
            className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none transition-all"
          />
        </div>

        {/* Action Submit */}
        <div className="pt-4 border-t border-slate-100 flex items-center justify-end gap-3">
          <Link
            href="/solicitudes"
            className="px-5 py-2.5 rounded-xl border border-slate-200 text-slate-600 hover:bg-slate-50 text-xs font-semibold transition-colors"
          >
            Cancelar
          </Link>

          <button
            type="submit"
            disabled={submitting}
            className="inline-flex items-center gap-2 px-6 py-2.5 bg-[#E64A19] hover:bg-[#D84315] text-white font-semibold text-xs rounded-xl shadow-md transition-all disabled:opacity-50"
          >
            {submitting ? (
              <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin"></div>
            ) : (
              <>
                <Save className="w-4 h-4" />
                <span>Guardar Solicitud</span>
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}
