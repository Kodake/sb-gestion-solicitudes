'use client';

import React from 'react';
import Link from 'next/link';
import { StatCard } from '@/components/ui/StatCard';
import { StateBadge, PriorityBadge } from '@/components/ui/Badge';
import { EstadoPieChart, PrioridadPieChart } from '@/components/ui/Charts';
import { Inbox, CheckCircle2, Clock, AlertTriangle, ArrowRight, PlusCircle, Sparkles, PieChart } from 'lucide-react';
import { useDashboard } from '@/hooks';

export default function DashboardPage() {
  const { data, loading, totalReqs } = useDashboard();

  if (loading) {
    return (
      <div className="flex items-center justify-center py-24">
        <div className="flex flex-col items-center gap-3">
          <div className="w-9 h-9 border-4 border-[#0D3048]/20 border-t-[#0D3048] rounded-full animate-spin"></div>
          <p className="text-xs font-semibold text-slate-400 uppercase tracking-wider">Cargando tablero...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-fade-in">
      {/* Action Header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pb-4 border-b border-slate-100">
        <div>
          <h2 className="text-xl font-extrabold text-[#0D3048] flex items-center gap-2">
            <span>Resumen Operativo de Solicitudes</span>
          </h2>
          <p className="text-xs text-slate-500 font-medium mt-0.5">Indicadores generales y métricas de desempeño</p>
        </div>
        <Link
          href="/solicitudes/nueva"
          className="btn-interactive inline-flex items-center justify-center gap-2 px-5 py-2.5 bg-[#E64A19] hover:bg-[#D84315] text-white font-semibold text-xs rounded-xl shadow-md transition-all active:scale-95"
        >
          <PlusCircle className="w-4 h-4" />
          <span>Nueva Solicitud</span>
        </Link>
      </div>

      {/* Main Metric Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-5">
        <StatCard
          title="Total Solicitudes"
          value={data?.totalSolicitudes || 0}
          icon={Inbox}
          bgClass="bg-blue-50"
          colorClass="text-blue-700"
        />
        <StatCard
          title="Solicitudes Abiertas"
          value={data?.solicitudesAbiertas || 0}
          icon={Clock}
          bgClass="bg-amber-50"
          colorClass="text-amber-700"
        />
        <StatCard
          title="Solicitudes Cerradas"
          value={data?.solicitudesCerradas || 0}
          icon={CheckCircle2}
          bgClass="bg-emerald-50"
          colorClass="text-emerald-700"
        />
        <StatCard
          title="Solicitudes Vencidas"
          value={data?.solicitudesVencidas || 0}
          icon={AlertTriangle}
          bgClass="bg-red-50"
          colorClass="text-red-700"
          subtitle="Superaron fecha compromiso"
        />
      </div>

      {/* Interactive Charts Section */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Donut / Pie Chart: Distribution by Status */}
        <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <div className="flex items-center justify-between border-b border-slate-100 pb-3">
            <h3 className="text-sm font-bold text-[#0D3048] flex items-center gap-2">
              <PieChart className="w-4 h-4 text-blue-600" />
              <span>Distribución por Estado</span>
            </h3>
            <span className="text-[11px] font-semibold text-slate-400 font-mono">Total: {data?.totalSolicitudes}</span>
          </div>

          <EstadoPieChart data={data?.porEstado || []} total={data?.totalSolicitudes || 0} />
        </div>

        {/* Donut / Pie Chart: Distribution by Priority */}
        <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <div className="flex items-center justify-between border-b border-slate-100 pb-3">
            <h3 className="text-sm font-bold text-[#0D3048] flex items-center gap-2">
              <PieChart className="w-4 h-4 text-amber-500" />
              <span>Distribución por Prioridad</span>
            </h3>
            <span className="text-[11px] font-semibold text-slate-400 font-mono">Total: {data?.totalSolicitudes}</span>
          </div>

          <PrioridadPieChart data={data?.porPrioridad || []} total={data?.totalSolicitudes || 0} />
        </div>
      </div>

      {/* Recent Requests Table */}
      <div className="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden space-y-4">
        <div className="p-6 pb-2 flex items-center justify-between border-b border-slate-100">
          <div>
            <h3 className="text-sm font-bold text-[#0D3048]">Últimas Solicitudes Registradas</h3>
            <p className="text-xs text-slate-400">Transacciones e incidencias recientes en la plataforma</p>
          </div>
          <Link
            href="/solicitudes"
            className="inline-flex items-center gap-1.5 text-xs font-bold text-[#0D3048] hover:text-[#E64A19] transition-colors"
          >
            <span>Ver Todas</span>
            <ArrowRight className="w-4 h-4" />
          </Link>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs min-w-[700px]">
            <thead className="bg-[#0D3048] text-white uppercase text-[10px] font-bold tracking-wider">
              <tr>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">Código</th>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">Título</th>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">Área</th>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">Estado</th>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">Prioridad</th>
                <th style={{ whiteSpace: 'nowrap' }} className="py-3 px-4 text-right">Detalle</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100 font-medium text-slate-700">
              {data?.ultimasSolicitudes.map((item) => (
                <tr key={item.id} className="hover:bg-slate-50/80 transition-colors">
                  <td style={{ whiteSpace: 'nowrap' }} className="py-3 px-4 font-mono font-bold text-[#0D3048]">{item.codigo}</td>
                  <td className="py-3 px-4 max-w-xs truncate font-semibold text-slate-900">{item.titulo}</td>
                  <td className="py-3 px-4 text-slate-600 truncate">{item.areaNombre}</td>
                  <td style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">
                    <StateBadge estado={item.estado} />
                  </td>
                  <td style={{ whiteSpace: 'nowrap' }} className="py-3 px-4">
                    <PriorityBadge prioridad={item.prioridad} />
                  </td>
                  <td style={{ whiteSpace: 'nowrap' }} className="py-3 px-4 text-right">
                    <Link
                      href={`/solicitudes/${item.id}`}
                      className="inline-flex items-center gap-1 text-[#0D3048] hover:text-[#E64A19] font-bold hover:underline"
                    >
                      <span>Abrir</span>
                      <ArrowRight className="w-3 h-3" />
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
