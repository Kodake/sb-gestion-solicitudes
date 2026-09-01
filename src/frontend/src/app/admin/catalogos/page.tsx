'use client';

import React, { useState, useEffect } from 'react';
import { Layers, Plus, Edit2, Power, CheckCircle2, XCircle, RefreshCw, Bookmark, FolderTree } from 'lucide-react';
import { catalogosService, GuardarAreaDto, GuardarTipoSolicitudDto } from '@/services/catalogosService';
import { IArea, ITipoSolicitud } from '@/types';
import { Modal } from '@/components/ui/Modal';
import { ConfirmModal } from '@/components/ui/ConfirmModal';
import toast from 'react-hot-toast';

export default function CatalogosPage() {
  const [activeTab, setActiveTab] = useState<'areas' | 'tipos'>('areas');
  const [areas, setAreas] = useState<IArea[]>([]);
  const [tipos, setTipos] = useState<ITipoSolicitud[]>([]);
  const [loading, setLoading] = useState(false);

  // Confirm Modal State
  const [confirmState, setConfirmState] = useState<{
    tipo: 'area' | 'tipoSolicitud';
    id: number;
    nombre: string;
    activo: boolean;
  } | null>(null);

  // Modals
  const [isAreaModalOpen, setIsAreaModalOpen] = useState(false);
  const [editingArea, setEditingArea] = useState<IArea | null>(null);
  const [areaForm, setAreaForm] = useState<GuardarAreaDto>({ nombre: '', descripcion: '', activa: true });

  const [isTipoModalOpen, setIsTipoModalOpen] = useState(false);
  const [editingTipo, setEditingTipo] = useState<ITipoSolicitud | null>(null);
  const [tipoForm, setTipoForm] = useState<GuardarTipoSolicitudDto>({ nombre: '', descripcion: '', activo: true });

  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    try {
      setLoading(true);
      if (activeTab === 'areas') {
        const res = await catalogosService.getAreas(false);
        if (res.success && res.data) setAreas(res.data);
      } else {
        const res = await catalogosService.getTiposSolicitud(false);
        if (res.success && res.data) setTipos(res.data);
      }
    } catch (err) {
      console.error(err);
      toast.error('Error al cargar datos del catálogo.');
    } finally {
      setLoading(false);
    }
  };

  // --- ÁREAS ---
  const handleOpenCreateArea = () => {
    setEditingArea(null);
    setAreaForm({ nombre: '', descripcion: '', activa: true });
    setIsAreaModalOpen(true);
  };

  const handleOpenEditArea = (area: IArea) => {
    setEditingArea(area);
    setAreaForm({ nombre: area.nombre, descripcion: area.descripcion || '', activa: area.activa });
    setIsAreaModalOpen(true);
  };

  const handleToggleArea = async (id: number) => {
    try {
      const res = await catalogosService.toggleEstadoArea(id);
      if (res.success) {
        toast.success(res.message);
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        loadData();
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al cambiar estado.');
    }
  };

  const handleSaveArea = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!areaForm.nombre.trim()) {
      toast.error('El nombre del área es requerido');
      return;
    }

    try {
      setSubmitting(true);
      if (editingArea) {
        const res = await catalogosService.actualizarArea(editingArea.id, areaForm);
        if (res.success) {
          toast.success(res.message);
          setIsAreaModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadData();
        }
      } else {
        const res = await catalogosService.crearArea(areaForm);
        if (res.success) {
          toast.success(res.message);
          setIsAreaModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadData();
        }
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al guardar el área.');
    } finally {
      setSubmitting(false);
    }
  };

  // --- TIPOS DE SOLICITUD ---
  const handleOpenCreateTipo = () => {
    setEditingTipo(null);
    setTipoForm({ nombre: '', descripcion: '', activo: true });
    setIsTipoModalOpen(true);
  };

  const handleOpenEditTipo = (tipo: ITipoSolicitud) => {
    setEditingTipo(tipo);
    setTipoForm({ nombre: tipo.nombre, descripcion: tipo.descripcion || '', activo: tipo.activo });
    setIsTipoModalOpen(true);
  };

  const handleToggleTipo = async (id: number) => {
    try {
      const res = await catalogosService.toggleEstadoTipoSolicitud(id);
      if (res.success) {
        toast.success(res.message);
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        loadData();
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al cambiar estado.');
    }
  };

  const handleSaveTipo = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!tipoForm.nombre.trim()) {
      toast.error('El nombre del tipo de solicitud es requerido');
      return;
    }

    try {
      setSubmitting(true);
      if (editingTipo) {
        const res = await catalogosService.actualizarTipoSolicitud(editingTipo.id, tipoForm);
        if (res.success) {
          toast.success(res.message);
          setIsTipoModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadData();
        }
      } else {
        const res = await catalogosService.crearTipoSolicitud(tipoForm);
        if (res.success) {
          toast.success(res.message);
          setIsTipoModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadData();
        }
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al guardar el tipo de solicitud.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="space-y-6">
      {/* Header Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 p-6 bg-gradient-to-r from-[#0D3048] to-[#143B58] rounded-2xl text-white shadow-md border border-[#194260]">
        <div className="flex items-center gap-3.5">
          <div className="p-3 bg-white/10 rounded-xl ring-2 ring-white/10 shrink-0">
            <Layers className="w-6 h-6 text-amber-300" />
          </div>
          <div>
            <h2 className="text-lg sm:text-xl font-extrabold tracking-tight">Mantenimiento de Catálogos</h2>
            <p className="text-xs text-slate-300 mt-0.5">
              Gestión y configuración de Áreas y Tipos de Solicitud de la Superintendencia de Bancos
            </p>
          </div>
        </div>

        <button
          onClick={activeTab === 'areas' ? handleOpenCreateArea : handleOpenCreateTipo}
          className="inline-flex items-center gap-2 px-4 py-2.5 rounded-xl bg-[#E64A19] hover:bg-[#d44315] text-white text-xs font-bold shadow-sm transition-all active:scale-95 cursor-pointer shrink-0"
        >
          <Plus className="w-4 h-4" />
          <span>{activeTab === 'areas' ? 'Nueva Área' : 'Nuevo Tipo'}</span>
        </button>
      </div>

      {/* Tabs */}
      <div className="flex items-center gap-2 border-b border-slate-200">
        <button
          onClick={() => setActiveTab('areas')}
          className={`flex items-center gap-2 px-5 py-3 text-xs font-bold border-b-2 transition-all cursor-pointer ${
            activeTab === 'areas'
              ? 'border-[#0D3048] text-[#0D3048]'
              : 'border-transparent text-slate-500 hover:text-slate-800'
          }`}
        >
          <FolderTree className="w-4 h-4" />
          <span>Áreas ({areas.length})</span>
        </button>

        <button
          onClick={() => setActiveTab('tipos')}
          className={`flex items-center gap-2 px-5 py-3 text-xs font-bold border-b-2 transition-all cursor-pointer ${
            activeTab === 'tipos'
              ? 'border-[#0D3048] text-[#0D3048]'
              : 'border-transparent text-slate-500 hover:text-slate-800'
          }`}
        >
          <Bookmark className="w-4 h-4" />
          <span>Tipos de Solicitud ({tipos.length})</span>
        </button>
      </div>

      {/* Content Table */}
      <div className="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-[#0D3048] text-white text-[11px] font-bold uppercase tracking-wider">
              <th className="py-3 px-4">Nombre</th>
              <th className="py-3 px-4">Descripción</th>
              <th className="py-3 px-4 text-center">Estado</th>
              <th className="py-3 px-4 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-xs">
            {loading ? (
              <tr>
                <td colSpan={4} className="py-12 text-center text-slate-500">
                  <RefreshCw className="w-6 h-6 animate-spin mx-auto text-[#0D3048] mb-2" />
                  <span>Cargando catálogo...</span>
                </td>
              </tr>
            ) : activeTab === 'areas' ? (
              areas.map((a) => (
                <tr key={a.id} className="hover:bg-slate-50/80 transition-colors">
                  <td className="py-3.5 px-4 font-bold text-slate-900">{a.nombre}</td>
                  <td className="py-3.5 px-4 text-slate-600">{a.descripcion || <span className="italic text-slate-400">Sin descripción</span>}</td>
                  <td className="py-3.5 px-4 text-center">
                    <span
                      className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold ${
                        a.activa ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-600'
                      }`}
                    >
                      {a.activa ? <CheckCircle2 className="w-3 h-3" /> : <XCircle className="w-3 h-3" />}
                      {a.activa ? 'Activa' : 'Inactiva'}
                    </span>
                  </td>
                  <td className="py-3.5 px-4 text-right space-x-1">
                    <button
                      onClick={() => handleOpenEditArea(a)}
                      title="Editar área"
                      className="p-1.5 rounded-lg text-slate-600 hover:text-[#0D3048] hover:bg-blue-50 border border-transparent hover:border-blue-200 transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs"
                    >
                      <Edit2 className="w-4 h-4" />
                    </button>
                    <button
                      onClick={() =>
                        setConfirmState({
                          tipo: 'area',
                          id: a.id,
                          nombre: a.nombre,
                          activo: a.activa,
                        })
                      }
                      title={a.activa ? 'Desactivar área' : 'Activar área'}
                      className={`p-1.5 rounded-lg transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs border border-transparent ${
                        a.activa
                          ? 'text-amber-600 hover:bg-amber-100/80 hover:text-amber-700 hover:border-amber-300'
                          : 'text-emerald-600 hover:bg-emerald-100/80 hover:text-emerald-700 hover:border-emerald-300'
                      }`}
                    >
                      <Power className="w-4 h-4" />
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              tipos.map((t) => (
                <tr key={t.id} className="hover:bg-slate-50/80 transition-colors">
                  <td className="py-3.5 px-4 font-bold text-slate-900">{t.nombre}</td>
                  <td className="py-3.5 px-4 text-slate-600">{t.descripcion || <span className="italic text-slate-400">Sin descripción</span>}</td>
                  <td className="py-3.5 px-4 text-center">
                    <span
                      className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold ${
                        t.activo ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-600'
                      }`}
                    >
                      {t.activo ? <CheckCircle2 className="w-3 h-3" /> : <XCircle className="w-3 h-3" />}
                      {t.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                  <td className="py-3.5 px-4 text-right space-x-1">
                    <button
                      onClick={() => handleOpenEditTipo(t)}
                      title="Editar tipo de solicitud"
                      className="p-1.5 rounded-lg text-slate-600 hover:text-[#0D3048] hover:bg-blue-50 border border-transparent hover:border-blue-200 transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs"
                    >
                      <Edit2 className="w-4 h-4" />
                    </button>
                    <button
                      onClick={() =>
                        setConfirmState({
                          tipo: 'tipoSolicitud',
                          id: t.id,
                          nombre: t.nombre,
                          activo: t.activo,
                        })
                      }
                      title={t.activo ? 'Desactivar tipo de solicitud' : 'Activar tipo de solicitud'}
                      className={`p-1.5 rounded-lg transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs border border-transparent ${
                        t.activo
                          ? 'text-amber-600 hover:bg-amber-100/80 hover:text-amber-700 hover:border-amber-300'
                          : 'text-emerald-600 hover:bg-emerald-100/80 hover:text-emerald-700 hover:border-emerald-300'
                      }`}
                    >
                      <Power className="w-4 h-4" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal: Confirmación de Cambio de Estado en Catálogo */}
      {confirmState && (
        <ConfirmModal
          isOpen={true}
          onClose={() => setConfirmState(null)}
          onConfirm={async () => {
            if (confirmState.tipo === 'area') {
              await handleToggleArea(confirmState.id);
            } else {
              await handleToggleTipo(confirmState.id);
            }
          }}
          title={
            confirmState.activo
              ? `Confirmar Desactivación de ${confirmState.tipo === 'area' ? 'Área' : 'Tipo de Solicitud'}`
              : `Confirmar Activación de ${confirmState.tipo === 'area' ? 'Área' : 'Tipo de Solicitud'}`
          }
          actionType={confirmState.activo ? 'desactivar' : 'activar'}
          entityName={confirmState.nombre}
          message={
            confirmState.activo
              ? `Al desactivar este elemento de catálogo, no se podrá seleccionar al registrar nuevas solicitudes.`
              : `Al activar este elemento de catálogo, estará disponible nuevamente para los usuarios.`
          }
        />
      )}

      {/* Modal: Área */}
      <Modal isOpen={isAreaModalOpen} onClose={() => setIsAreaModalOpen(false)} title={editingArea ? 'Editar Área' : 'Nueva Área'}>
        <form onSubmit={handleSaveArea} className="space-y-4 pt-2">
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Nombre del Área *</label>
            <input
              type="text"
              required
              maxLength={100}
              value={areaForm.nombre}
              onChange={(e) => setAreaForm({ ...areaForm, nombre: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Descripción</label>
            <textarea
              rows={3}
              maxLength={250}
              value={areaForm.descripcion}
              onChange={(e) => setAreaForm({ ...areaForm, descripcion: e.target.value })}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>
          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button type="button" onClick={() => setIsAreaModalOpen(false)} className="px-4 py-2 rounded-xl border border-slate-200 text-xs font-semibold">Cancelar</button>
            <button type="submit" disabled={submitting} className="px-5 py-2 rounded-xl bg-[#0D3048] text-white text-xs font-semibold shadow">
              {submitting ? 'Guardando...' : 'Guardar Área'}
            </button>
          </div>
        </form>
      </Modal>

      {/* Modal: Tipo de Solicitud */}
      <Modal isOpen={isTipoModalOpen} onClose={() => setIsTipoModalOpen(false)} title={editingTipo ? 'Editar Tipo de Solicitud' : 'Nuevo Tipo de Solicitud'}>
        <form onSubmit={handleSaveTipo} className="space-y-4 pt-2">
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Nombre del Tipo de Solicitud *</label>
            <input
              type="text"
              required
              maxLength={100}
              value={tipoForm.nombre}
              onChange={(e) => setTipoForm({ ...tipoForm, nombre: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Descripción</label>
            <textarea
              rows={3}
              maxLength={250}
              value={tipoForm.descripcion}
              onChange={(e) => setTipoForm({ ...tipoForm, descripcion: e.target.value })}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>
          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button type="button" onClick={() => setIsTipoModalOpen(false)} className="px-4 py-2 rounded-xl border border-slate-200 text-xs font-semibold">Cancelar</button>
            <button type="submit" disabled={submitting} className="px-5 py-2 rounded-xl bg-[#0D3048] text-white text-xs font-semibold shadow">
              {submitting ? 'Guardando...' : 'Guardar Tipo'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
