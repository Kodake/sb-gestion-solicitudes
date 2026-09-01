'use client';

import React from 'react';
import Link from 'next/link';
import { EstadoSolicitudEnum, PrioridadEnum } from '@/types';
import { StateBadge, PriorityBadge } from '@/components/ui/Badge';
import { Search, Filter, RefreshCw, PlusCircle, Calendar, AlertTriangle, Eye } from 'lucide-react';
import { useSolicitudesList } from '@/hooks';

export default function SolicitudesConsultaPage() {
  const {
    data,
    areas,
    loading,
    filtros,
    updateFiltro,
    handleSearchSubmit,
    handleResetFilters,
    fetchSolicitudes,
  } = useSolicitudesList();

  return (
    <div className="space-y-6">
      {/* Top Bar */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 pb-4 border-b border-slate-100">
        <div>
          <h2 className="text-xl font-bold text-[#0D3048]">Consulta General de Solicitudes</h2>
          <p className="text-xs text-slate-500 mt-0.5">Búsqueda, filtrado y seguimiento del historial</p>
        </div>
        <Link
          href="/solicitudes/nueva"
          className="inline-flex items-center justify-center gap-2 px-5 py-2.5 bg-[#E64A19] hover:bg-[#D84315] text-white font-semibold text-xs rounded-xl shadow-sm transition-all"
        >
          <PlusCircle className="w-4 h-4" />
          <span>Crear Registro</span>
        </Link>
      </div>

      {/* Filter Card */}
      <div className="bg-[#EDF0F7]/40 p-5 rounded-2xl border border-slate-200/80 space-y-4">
        <form onSubmit={handleSearchSubmit} className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-3">
          {/* Search Term */}
          <div className="relative col-span-1 sm:col-span-2 md:col-span-1">
            <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3.5" />
            <input
              type="text"
              value={filtros.searchTerm || ''}
              onChange={(e) => updateFiltro('searchTerm', e.target.value)}
              placeholder="Código o palabras clave..."
              className="w-full pl-9 pr-4 py-2.5 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none bg-white"
            />
          </div>

          {/* Estado */}
          <div>
            <select
              value={filtros.estado ?? ''}
              onChange={(e) => updateFiltro('estado', e.target.value ? Number(e.target.value) : undefined)}
              className="w-full px-3 py-2.5 rounded-xl border border-slate-200 text-xs font-medium text-slate-700 bg-white outline-none focus:border-[#0D3048]"
            >
              <option value="">Todos los estados</option>
              <option value={EstadoSolicitudEnum.Registrada}>Registrada</option>
              <option value={EstadoSolicitudEnum.EnAnalisis}>En Análisis</option>
              <option value={EstadoSolicitudEnum.EnProgreso}>En Progreso</option>
              <option value={EstadoSolicitudEnum.EnEsperaDelSolicitante}>En Espera del Solicitante</option>
              <option value={EstadoSolicitudEnum.Resuelta}>Resuelta</option>
              <option value={EstadoSolicitudEnum.Cerrada}>Cerrada</option>
            </select>
          </div>

          {/* Prioridad */}
          <div>
            <select
              value={filtros.prioridad ?? ''}
              onChange={(e) => updateFiltro('prioridad', e.target.value ? Number(e.target.value) : undefined)}
              className="w-full px-3 py-2.5 rounded-xl border border-slate-200 text-xs font-medium text-slate-700 bg-white outline-none focus:border-[#0D3048]"
            >
              <option value="">Todas las prioridades</option>
              <option value={PrioridadEnum.Baja}>Baja</option>
              <option value={PrioridadEnum.Media}>Media</option>
              <option value={PrioridadEnum.Alta}>Alta</option>
              <option value={PrioridadEnum.Critica}>Crítica</option>
            </select>
          </div>

          {/* Área */}
          <div>
            <select
              value={filtros.areaId ?? ''}
              onChange={(e) => updateFiltro('areaId', e.target.value ? Number(e.target.value) : undefined)}
              className="w-full px-3 py-2.5 rounded-xl border border-slate-200 text-xs font-medium text-slate-700 bg-white outline-none focus:border-[#0D3048]"
            >
              <option value="">Todas las áreas</option>
              {areas.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.nombre}
                </option>
              ))}
            </select>
          </div>
        </form>

        <div className="flex items-center justify-between pt-2 border-t border-slate-200/60 text-xs">
          <button
            type="button"
            onClick={handleResetFilters}
            className="flex items-center gap-1.5 text-slate-500 hover:text-[#0D3048] font-semibold transition-colors"
          >
            <RefreshCw className="w-3.5 h-3.5" />
            <span>Limpiar Filtros</span>
          </button>

          <p className="text-slate-400 font-medium">
            Mostrando <span className="font-bold text-slate-700">{data?.items.length || 0}</span> de{' '}
            <span className="font-bold text-slate-700">{data?.totalCount || 0}</span> registros
          </p>
        </div>
      </div>

      {/* Requests Table */}
      {loading ? (
        <div className="flex items-center justify-center py-20">
          <div className="w-8 h-8 border-4 border-[#0D3048]/20 border-t-[#0D3048] rounded-full animate-spin"></div>
        </div>
      ) : !data || data.items.length === 0 ? (
        <div
          onClick={handleResetFilters}
          className="p-12 text-center bg-white rounded-3xl border border-slate-200/80 shadow-sm hover:shadow-2xl hover:border-amber-400/60 hover:ring-4 hover:ring-amber-300/20 transition-all duration-300 transform hover:-translate-y-1.5 group cursor-pointer space-y-4 max-w-md mx-auto my-8 select-none"
        >
          <div className="w-16 h-16 rounded-2xl bg-slate-100 group-hover:bg-[#0D3048] text-slate-400 group-hover:text-amber-400 flex items-center justify-center mx-auto shadow-xs group-hover:shadow-xl group-hover:scale-110 transition-all duration-300 group-hover:rotate-6 border border-slate-200/60 group-hover:border-[#18486B]">
            <Filter className="w-8 h-8 transition-transform group-hover:scale-110" />
          </div>

          <div className="space-y-1">
            <h3 className="text-base font-extrabold text-slate-800 group-hover:text-[#0D3048] transition-colors">
              No se encontraron solicitudes
            </h3>
            <p className="text-xs text-slate-500 group-hover:text-slate-700 transition-colors max-w-xs mx-auto leading-relaxed">
              Pruebe ajustando los filtros de búsqueda o haga clic aquí para restablecer los criterios.
            </p>
          </div>

          <div className="pt-2">
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                handleResetFilters();
              }}
              className="inline-flex items-center gap-2 px-4 py-2 rounded-xl bg-slate-100 group-hover:bg-[#E64A19] text-slate-700 group-hover:text-white text-xs font-bold transition-all shadow-xs group-hover:shadow-md active:scale-95 cursor-pointer"
            >
              <RefreshCw className="w-3.5 h-3.5 group-hover:rotate-180 transition-transform duration-500" />
              <span>Restablecer Filtros</span>
            </button>
          </div>
        </div>
      ) : (
        <div className="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left text-xs min-w-[1000px]">
              <thead className="bg-[#0D3048] text-white uppercase text-[10px] font-bold tracking-wider">
                <tr>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[140px]">Código</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[220px]">Título / Requerimiento</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[160px]">Área</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[120px]">Prioridad</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[150px]">Estado</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[130px]">Solicitante</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 min-w-[140px]">Responsable</th>
                  <th style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 text-right">Acción</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100 font-medium text-slate-700">
                {data.items.map((item) => (
                  <tr key={item.id} className="hover:bg-slate-50/80 transition-colors">
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 font-mono font-bold text-[#0D3048]">
                      {item.codigo}
                      {item.estaVencida && (
                        <span style={{ whiteSpace: 'nowrap' }} className="text-[9px] font-extrabold text-red-500 uppercase tracking-tight flex items-center gap-1 mt-0.5">
                          <AlertTriangle className="w-3 h-3" /> SLA Vencido
                        </span>
                      )}
                    </td>
                    <td className="py-3.5 px-4 max-w-xs">
                      <p className="font-bold text-slate-900 truncate">{item.titulo}</p>
                      <p className="text-[10px] text-slate-400 truncate">{item.tipoSolicitudNombre}</p>
                    </td>
                    <td className="py-3.5 px-4 text-slate-600 truncate">{item.areaNombre}</td>
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4">
                      <PriorityBadge prioridad={item.prioridad} />
                    </td>
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4">
                      <StateBadge estado={item.estado} />
                    </td>
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 truncate">{item.solicitanteNombre}</td>
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 truncate">
                      {item.responsableNombre ? (
                        <span className="text-slate-800 font-semibold">{item.responsableNombre}</span>
                      ) : (
                        <span className="text-slate-400 italic">Sin Asignar</span>
                      )}
                    </td>
                    <td style={{ whiteSpace: 'nowrap' }} className="py-3.5 px-4 text-right">
                      <Link
                        href={`/solicitudes/${item.id}`}
                        className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg bg-slate-100 hover:bg-[#0D3048] text-slate-700 hover:text-white font-semibold transition-all shadow-sm"
                      >
                        <Eye className="w-3.5 h-3.5" />
                        <span>Ver</span>
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Pagination Footer */}
          <div className="p-4 border-t border-slate-100 bg-slate-50/50 flex flex-col sm:flex-row items-center justify-between gap-4 text-xs">
            <div className="flex items-center gap-3">
              <span className="text-slate-500 font-medium">Mostrar:</span>
              <select
                value={filtros.pageSize || 5}
                onChange={(e) => updateFiltro('pageSize', Number(e.target.value))}
                className="px-2.5 py-1 rounded-lg border border-slate-200 bg-white font-semibold text-slate-700 outline-none focus:border-[#0D3048]"
              >
                <option value={5}>5 por página</option>
                <option value={10}>10 por página</option>
                <option value={20}>20 por página</option>
              </select>
              <span className="text-slate-400 font-medium hidden md:inline">
                Mostrando <span className="font-bold text-slate-700">{((data.pageNumber - 1) * (filtros.pageSize || 5)) + 1}</span> a{' '}
                <span className="font-bold text-slate-700">{Math.min(data.pageNumber * (filtros.pageSize || 5), data.totalCount)}</span> de{' '}
                <span className="font-bold text-slate-700">{data.totalCount}</span> solicitudes
              </span>
            </div>

            <div className="flex items-center gap-2">
              <button
                disabled={!data.hasPreviousPage}
                onClick={() => updateFiltro('pageNumber', data.pageNumber - 1)}
                className="px-3.5 py-1.5 rounded-lg border border-slate-200 bg-white text-slate-700 font-semibold disabled:opacity-40 hover:bg-slate-100 transition-all active:scale-95 shadow-xs cursor-pointer disabled:cursor-not-allowed"
              >
                Anterior
              </button>

              {/* Numbered Page Buttons */}
              <div className="flex items-center gap-1">
                {Array.from({ length: data.totalPages || 1 }, (_, idx) => idx + 1).map((pageNum) => (
                  <button
                    key={pageNum}
                    onClick={() => updateFiltro('pageNumber', pageNum)}
                    className={`w-8 h-8 rounded-lg font-bold text-xs transition-all cursor-pointer ${
                      data.pageNumber === pageNum
                        ? 'bg-[#0D3048] text-white shadow-sm'
                        : 'bg-white border border-slate-200 text-slate-700 hover:bg-slate-100'
                    }`}
                  >
                    {pageNum}
                  </button>
                ))}
              </div>

              <button
                disabled={!data.hasNextPage}
                onClick={() => updateFiltro('pageNumber', data.pageNumber + 1)}
                className="px-3.5 py-1.5 rounded-lg border border-slate-200 bg-white text-slate-700 font-semibold disabled:opacity-40 hover:bg-slate-100 transition-all active:scale-95 shadow-xs cursor-pointer disabled:cursor-not-allowed"
              >
                Siguiente
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
