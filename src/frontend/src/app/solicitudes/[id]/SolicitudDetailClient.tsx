'use client';

import React from 'react';
import Link from 'next/link';
import { EstadoSolicitudEnum, RolEnum } from '@/types';
import { StateBadge, PriorityBadge } from '@/components/ui/Badge';
import { Modal } from '@/components/ui/Modal';
import { useAuth, useSolicitudDetalle } from '@/hooks';
import {
  ArrowLeft,
  UserCheck,
  RefreshCw,
  MessageSquare,
  History,
  Bell,
  Calendar,
  AlertTriangle,
  Send,
  ExternalLink,
  CheckCircle2,
  Lock,
  Edit3,
} from 'lucide-react';
import { EditarSolicitudModal } from '@/components/solicitudes/EditarSolicitudModal';

export default function SolicitudDetailClient() {
  const { user } = useAuth();
  const [isEditModalOpen, setIsEditModalOpen] = React.useState(false);
  const {
    id,
    solicitud,
    analistas,
    loading,
    error,
    isEstadoModalOpen,
    setIsEstadoModalOpen,
    isAsignarModalOpen,
    setIsAsignarModalOpen,
    nuevoEstado,
    setNuevoEstado,
    comentarioEstado,
    setComentarioEstado,
    responsableId,
    setResponsableId,
    comentarioAsignar,
    setComentarioAsignar,
    nuevoComentarioTexto,
    setNuevoComentarioTexto,
    esPublico,
    setEsPublico,
    modalError,
    setModalError,
    modalSubmitting,
    comentarioSubmitting,
    handleCambiarEstadoSubmit,
    handleAsignarSubmit,
    handleAgregarComentarioSubmit,
    fetchDetalle,
  } = useSolicitudDetalle();

  const canManage = user?.rol === RolEnum.Administrador || user?.rol === RolEnum.Analista;

  if (loading) {
    return (
      <div className="flex items-center justify-center py-20">
        <div className="w-8 h-8 border-4 border-[#0D3048]/20 border-t-[#0D3048] rounded-full animate-spin"></div>
      </div>
    );
  }

  if (error || !solicitud) {
    return (
      <div className="p-8 text-center space-y-4">
        <AlertTriangle className="w-12 h-12 text-red-500 mx-auto" />
        <h3 className="text-lg font-bold text-slate-800">{error || 'Solicitud no encontrada'}</h3>
        <Link
          href="/solicitudes"
          className="inline-flex items-center gap-2 px-4 py-2 bg-[#0D3048] text-white rounded-xl text-xs font-semibold"
        >
          <ArrowLeft className="w-4 h-4" />
          <span>Volver a la consulta</span>
        </Link>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Top Detail Bar */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-6 border-b border-slate-100">
        <div className="flex items-center gap-3">
          <Link
            href="/solicitudes"
            className="p-2.5 rounded-xl border border-slate-200 text-slate-500 hover:text-[#0D3048] hover:bg-slate-50 transition-colors"
          >
            <ArrowLeft className="w-5 h-5" />
          </Link>
          <div>
            <div className="flex items-center gap-2">
              <span className="font-mono font-bold text-lg text-[#0D3048] bg-slate-100 px-3 py-0.5 rounded-lg border border-slate-200">
                {solicitud.codigo}
              </span>
              <StateBadge estado={solicitud.estado} />
              <PriorityBadge prioridad={solicitud.prioridad} />
              {solicitud.estaVencida && (
                <span className="inline-flex items-center gap-1 px-2.5 py-1 rounded-full text-xs font-bold bg-red-100 text-red-700 border border-red-200">
                  <AlertTriangle className="w-3.5 h-3.5" />
                  Vencida
                </span>
              )}
            </div>
            <h2 className="text-xl font-bold text-slate-900 mt-1">{solicitud.titulo}</h2>
          </div>
        </div>

        {/* Actions for Staff / Author */}
        <div className="flex items-center gap-2">
          {(canManage || (user?.id === solicitud.solicitanteId && (solicitud.estado === EstadoSolicitudEnum.Registrada || solicitud.estado === EstadoSolicitudEnum.EnEsperaDelSolicitante))) && (
            <button
              onClick={() => setIsEditModalOpen(true)}
              className="inline-flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 hover:border-blue-300 hover:bg-blue-50/50 text-[#0D3048] font-semibold text-xs rounded-xl shadow-sm transition-all duration-200 cursor-pointer hover:scale-[1.02] active:scale-95"
            >
              <Edit3 className="w-4 h-4 text-blue-600" />
              <span>Editar Información</span>
            </button>
          )}

          {canManage && (
            <>
              <button
                onClick={() => {
                  setModalError(null);
                  setIsAsignarModalOpen(true);
                }}
                className="inline-flex items-center gap-2 px-4 py-2.5 bg-white border border-slate-200 hover:border-blue-300 hover:bg-blue-50/50 text-[#0D3048] font-semibold text-xs rounded-xl shadow-sm transition-all duration-200 cursor-pointer hover:scale-[1.02] active:scale-95"
              >
                <UserCheck className="w-4 h-4 text-blue-600" />
                <span>Asignar Responsable</span>
              </button>
            </>
          )}

          <button
            onClick={() => {
              setModalError(null);
              setIsEstadoModalOpen(true);
            }}
            className="inline-flex items-center gap-2 px-4 py-2.5 bg-[#0D3048] hover:bg-[#133A57] text-white font-semibold text-xs rounded-xl shadow-md transition-all duration-200 cursor-pointer hover:scale-[1.02] active:scale-95"
          >
            <RefreshCw className="w-4 h-4" />
            <span>Cambiar Estado</span>
          </button>
        </div>
      </div>

      {/* Main Details Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Left Col: Request Specs */}
        <div className="lg:col-span-2 space-y-6">
          {/* Description Card */}
          <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
            <h3 className="text-sm font-bold text-[#0D3048] uppercase tracking-wider">
              Descripción del Requerimiento
            </h3>
            <p className="text-xs text-slate-700 leading-relaxed whitespace-pre-line bg-[#EDF0F7]/40 p-4 rounded-xl border border-slate-100">
              {solicitud.descripcion}
            </p>

            {solicitud.referenciaEvidencia && (
              <div className="pt-2">
                <span className="text-[11px] font-bold text-slate-500 uppercase tracking-wider block mb-1">
                  Referencia / Evidencia:
                </span>
                <div className="flex items-center gap-2 text-xs font-mono bg-slate-50 p-3 rounded-xl border border-slate-200 text-slate-700">
                  <ExternalLink className="w-4 h-4 text-blue-600 shrink-0" />
                  <span className="truncate">{solicitud.referenciaEvidencia}</span>
                </div>
              </div>
            )}
          </div>

          {/* Comments Section */}
          <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-6">
            <div className="flex items-center justify-between">
              <h3 className="text-sm font-bold text-[#0D3048] uppercase tracking-wider flex items-center gap-2">
                <MessageSquare className="w-4 h-4 text-blue-600" />
                <span>Comentarios ({solicitud.comentarios.length})</span>
              </h3>
            </div>

            {/* Add Comment Form */}
            <form onSubmit={handleAgregarComentarioSubmit} className="space-y-3">
              <textarea
                rows={3}
                required
                value={nuevoComentarioTexto}
                onChange={(e) => setNuevoComentarioTexto(e.target.value)}
                placeholder="Escriba un nuevo comentario respecto a la solicitud..."
                className="w-full p-3.5 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none transition-all resize-none"
              />

              <div className="flex items-center justify-between">
                {canManage ? (
                  <label className="flex items-center gap-2 cursor-pointer text-xs text-slate-600 font-medium">
                    <input
                      type="checkbox"
                      checked={!esPublico}
                      onChange={(e) => setEsPublico(!e.target.checked)}
                      className="rounded border-slate-300 text-[#0D3048] focus:ring-[#0D3048]"
                    />
                    <span className="flex items-center gap-1">
                      <Lock className="w-3.5 h-3.5 text-amber-600" />
                      Comentario Interno (Solo Analistas y Admin)
                    </span>
                  </label>
                ) : (
                  <div></div>
                )}

                <button
                  type="submit"
                  disabled={comentarioSubmitting || !nuevoComentarioTexto.trim()}
                  className="inline-flex items-center gap-2 px-4 py-2 bg-[#0D3048] hover:bg-[#133A57] text-white font-semibold text-xs rounded-xl transition-colors disabled:opacity-40"
                >
                  <Send className="w-3.5 h-3.5" />
                  <span>Publicar</span>
                </button>
              </div>
            </form>

            {/* Comments List */}
            <div className="space-y-3 pt-2">
              {solicitud.comentarios.length === 0 ? (
                <p className="text-xs text-slate-400 text-center py-4">No hay comentarios aún.</p>
              ) : (
                solicitud.comentarios.map((c) => (
                  <div
                    key={c.id}
                    className={`p-4 rounded-xl border ${
                      !c.esPublico ? 'bg-amber-50/60 border-amber-200' : 'bg-[#EDF0F7]/30 border-slate-100'
                    }`}
                  >
                    <div className="flex items-center justify-between mb-1.5">
                      <div className="flex items-center gap-2">
                        <span className="font-bold text-xs text-slate-900">{c.usuarioNombre}</span>
                        <span className="text-[10px] text-slate-500 font-mono">({c.usuarioRol})</span>
                        {!c.esPublico && (
                          <span className="text-[10px] bg-amber-200 text-amber-900 font-bold px-2 py-0.5 rounded-full flex items-center gap-1">
                            <Lock className="w-3 h-3" /> Interno
                          </span>
                        )}
                      </div>
                      <span className="text-[10px] text-slate-400 font-mono">
                        {new Date(c.fecha).toLocaleString('es-DO')}
                      </span>
                    </div>
                    <p className="text-xs text-slate-700 leading-normal">{c.texto}</p>
                  </div>
                ))
              )}
            </div>
          </div>
        </div>

        {/* Right Col: Attributes & Timeline */}
        <div className="space-y-6">
          {/* Metadata Card */}
          <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4 text-xs">
            <h3 className="text-sm font-bold text-[#0D3048] uppercase tracking-wider pb-2 border-b border-slate-100">
              Información de Clasificación
            </h3>

            <div className="space-y-3">
              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Área Solicitante</span>
                <span className="font-bold text-slate-800 text-xs">{solicitud.areaNombre}</span>
              </div>

              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Tipo de Solicitud</span>
                <span className="font-bold text-slate-800 text-xs">{solicitud.tipoSolicitudNombre}</span>
              </div>

              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Usuario Solicitante</span>
                <span className="font-semibold text-slate-800">{solicitud.solicitanteNombre}</span>
              </div>

              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Analista Responsable</span>
                <span className="font-semibold text-slate-800">
                  {solicitud.responsableNombre || <span className="italic text-amber-600 font-normal">Sin asignar</span>}
                </span>
              </div>

              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Fecha de Registro</span>
                <span className="font-mono text-slate-700">
                  {new Date(solicitud.fechaCreacion).toLocaleString('es-DO')}
                </span>
              </div>

              <div>
                <span className="text-slate-400 block text-[10px] uppercase font-bold">Fecha Compromiso SLA</span>
                <span className={`font-mono font-bold ${solicitud.estaVencida ? 'text-red-600' : 'text-slate-800'}`}>
                  {new Date(solicitud.fechaCompromiso).toLocaleDateString('es-DO')}
                </span>
              </div>
            </div>
          </div>

          {/* Timeline of Status Changes */}
          <div className="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
            <h3 className="text-sm font-bold text-[#0D3048] uppercase tracking-wider flex items-center gap-2 pb-2 border-b border-slate-100">
              <History className="w-4 h-4 text-blue-600" />
              <span>Trazabilidad de Estados</span>
            </h3>

            <div className="relative pl-6 space-y-6 before:absolute before:left-2.5 before:top-2 before:bottom-2 before:w-0.5 before:bg-slate-200">
              {solicitud.historialEstados.map((h, idx) => (
                <div key={h.id} className="relative">
                  <div className="absolute -left-[23px] top-0.5 w-4 h-4 rounded-full bg-[#0D3048] border-2 border-white ring-2 ring-slate-100 flex items-center justify-center"></div>
                  <div className="text-xs">
                    <div className="flex items-center gap-2 mb-0.5">
                      <StateBadge estado={h.estadoNuevo} />
                      <span className="text-[10px] text-slate-400 font-mono">
                        {new Date(h.fecha).toLocaleDateString('es-DO')}
                      </span>
                    </div>
                    <p className="font-semibold text-slate-800 text-[11px] mt-1">{h.usuarioNombre}</p>
                    <p className="text-slate-600 text-[11px] bg-slate-50 p-2.5 rounded-lg border border-slate-100 mt-1 italic">
                      &quot;{h.comentario}&quot;
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>

      {/* Modal: Cambiar Estado */}
      <Modal isOpen={isEstadoModalOpen} onClose={() => setIsEstadoModalOpen(false)} title="Cambiar Estado de Solicitud">
        <form onSubmit={handleCambiarEstadoSubmit} className="space-y-4">
          {modalError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-xl text-red-700 text-xs font-medium flex items-center gap-2">
              <AlertTriangle className="w-4 h-4 shrink-0" />
              <span>{modalError}</span>
            </div>
          )}

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Nuevo Estado <span className="text-red-500">*</span>
            </label>
            <select
              value={nuevoEstado}
              onChange={(e) => setNuevoEstado(Number(e.target.value))}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-semibold text-slate-800 bg-white focus:border-[#0D3048] outline-none"
            >
              <option value={EstadoSolicitudEnum.Registrada}>Registrada</option>
              <option value={EstadoSolicitudEnum.EnAnalisis}>En Análisis</option>
              <option value={EstadoSolicitudEnum.EnProgreso}>En Progreso</option>
              <option value={EstadoSolicitudEnum.EnEsperaDelSolicitante}>En Espera del Solicitante</option>
              <option value={EstadoSolicitudEnum.Resuelta}>Resuelta</option>
              <option value={EstadoSolicitudEnum.Cerrada}>Cerrada</option>
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Comentario de Transición / Resolución{' '}
              {Number(nuevoEstado) === EstadoSolicitudEnum.Cerrada && <span className="text-red-500">(Obligatorio al cerrar *)</span>}
            </label>
            <textarea
              rows={4}
              required={Number(nuevoEstado) === EstadoSolicitudEnum.Cerrada}
              value={comentarioEstado}
              onChange={(e) => setComentarioEstado(e.target.value)}
              placeholder={
                Number(nuevoEstado) === EstadoSolicitudEnum.Cerrada
                  ? 'Ingrese obligatoriamente la justificación o solución entregada...'
                  : 'Motivo o detalle de la transición de estado...'
              }
              className="w-full p-3.5 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none transition-all resize-none"
            />
          </div>

          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button
              type="button"
              onClick={() => setIsEstadoModalOpen(false)}
              className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={modalSubmitting}
              className="px-5 py-2 rounded-xl bg-[#0D3048] hover:bg-[#133A57] text-white text-xs font-semibold shadow transition-all disabled:opacity-50"
            >
              {modalSubmitting ? 'Guardando...' : 'Confirmar Estado'}
            </button>
          </div>
        </form>
      </Modal>

      {/* Modal: Asignar Responsable */}
      <Modal isOpen={isAsignarModalOpen} onClose={() => setIsAsignarModalOpen(false)} title="Asignar Analista Responsable">
        <form onSubmit={handleAsignarSubmit} className="space-y-4">
          {modalError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded-xl text-red-700 text-xs font-medium">
              {modalError}
            </div>
          )}

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Seleccionar Analista Técnico <span className="text-red-500">*</span>
            </label>
            <select
              value={responsableId}
              onChange={(e) => setResponsableId(Number(e.target.value))}
              className="w-full px-4 py-3 rounded-xl border border-slate-200 text-xs font-semibold text-slate-800 bg-white focus:border-[#0D3048] outline-none"
            >
              {analistas.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.nombre} ({a.rolNombre})
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 uppercase tracking-wider mb-2">
              Comentario de Asignación (Opcional)
            </label>
            <textarea
              rows={3}
              value={comentarioAsignar}
              onChange={(e) => setComentarioAsignar(e.target.value)}
              placeholder="Instrucciones adicionales para el analista..."
              className="w-full p-3.5 rounded-xl border border-slate-200 text-xs font-medium focus:border-[#0D3048] outline-none transition-all resize-none"
            />
          </div>

          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button
              type="button"
              onClick={() => setIsAsignarModalOpen(false)}
              className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={modalSubmitting}
              className="px-5 py-2 rounded-xl bg-blue-600 hover:bg-blue-700 text-white text-xs font-semibold shadow transition-all disabled:opacity-50"
            >
              {modalSubmitting ? 'Asignando...' : 'Asignar Responsable'}
            </button>
          </div>
        </form>
      </Modal>

      {/* Modal: Editar Solicitud */}
      {solicitud && (
        <EditarSolicitudModal
          isOpen={isEditModalOpen}
          onClose={() => setIsEditModalOpen(false)}
          solicitud={solicitud}
          onActualizado={fetchDetalle}
        />
      )}
    </div>
  );
}
