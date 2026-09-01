'use client';

import React, { useState, useEffect } from 'react';
import {
  Building2,
  Plus,
  Search,
  Edit2,
  Power,
  CheckCircle2,
  XCircle,
  RefreshCw,
} from 'lucide-react';
import { entidadesService, GuardarEntidadData } from '@/services/entidadesService';
import { IEntidadGubernamental } from '@/types';
import { Modal } from '@/components/ui/Modal';
import { ConfirmModal } from '@/components/ui/ConfirmModal';
import toast from 'react-hot-toast';

export default function EntidadesGubernamentalesPage() {
  const [entidades, setEntidades] = useState<IEntidadGubernamental[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSector, setSelectedSector] = useState('');
  const [selectedPoder, setSelectedPoder] = useState('');
  const [selectedCategoria, setSelectedCategoria] = useState('');

  const [sectores, setSectores] = useState<string[]>([]);
  const [poderes, setPoderes] = useState<string[]>([]);
  const [categorias, setCategorias] = useState<string[]>([]);

  // Modal State
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [confirmEntidad, setConfirmEntidad] = useState<IEntidadGubernamental | null>(null);
  const [editingEntidad, setEditingEntidad] = useState<IEntidadGubernamental | null>(null);
  const [formData, setFormData] = useState<GuardarEntidadData>({
    nombre: '',
    categoria: '',
    poderEstado: 'Poder Ejecutivo',
    sector: '',
    activo: true,
  });
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadFilters();
    loadEntidades();
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      loadEntidades();
    }, 300);
    return () => clearTimeout(timer);
  }, [searchTerm, selectedSector, selectedPoder, selectedCategoria]);

  const loadFilters = async () => {
    try {
      const [secRes, podRes, catRes] = await Promise.all([
        entidadesService.getSectores(),
        entidadesService.getPoderes(),
        entidadesService.getCategorias(),
      ]);
      if (secRes.success && secRes.data) setSectores(secRes.data);
      if (podRes.success && podRes.data) setPoderes(podRes.data);
      if (catRes.success && catRes.data) setCategorias(catRes.data);
    } catch (err) {
      console.error('Error loading filters:', err);
    }
  };

  const loadEntidades = async () => {
    try {
      setLoading(true);
      const res = await entidadesService.getEntidades({
        searchTerm: searchTerm.trim() || undefined,
        sector: selectedSector || undefined,
        poderEstado: selectedPoder || undefined,
        categoria: selectedCategoria || undefined,
        pageSize: 100,
      });
      if (res.success && res.data) {
        setEntidades(res.data.items);
      }
    } catch (err) {
      console.error(err);
      toast.error('Error al cargar entidades gubernamentales');
    } finally {
      setLoading(false);
    }
  };

  const handleOpenCreateModal = () => {
    setEditingEntidad(null);
    setFormData({
      nombre: '',
      categoria: categorias[0] || 'Organismo Descentralizado Funcionalmente',
      poderEstado: poderes[0] || 'Poder Ejecutivo',
      sector: sectores[0] || 'Financiero',
      activo: true,
    });
    setIsModalOpen(true);
  };

  const handleOpenEditModal = (entidad: IEntidadGubernamental) => {
    setEditingEntidad(entidad);
    setFormData({
      nombre: entidad.nombre,
      categoria: entidad.categoria,
      poderEstado: entidad.poderEstado,
      sector: entidad.sector,
      activo: entidad.activo,
    });
    setIsModalOpen(true);
  };

  const handleToggleEstado = async (id: number) => {
    try {
      const res = await entidadesService.toggleEstadoEntidad(id);
      if (res.success) {
        toast.success(res.message);
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        loadEntidades();
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al cambiar estado.');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.nombre.trim() || !formData.categoria.trim() || !formData.poderEstado.trim() || !formData.sector.trim()) {
      toast.error('Por favor complete todos los campos requeridos (*)');
      return;
    }

    try {
      setSubmitting(true);
      if (editingEntidad) {
        const res = await entidadesService.actualizarEntidad(editingEntidad.id, formData);
        if (res.success) {
          toast.success(res.message || 'Entidad actualizada correctamente.');
          setIsModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadEntidades();
          loadFilters();
        }
      } else {
        const res = await entidadesService.crearEntidad(formData);
        if (res.success) {
          toast.success(res.message || 'Entidad creada correctamente.');
          setIsModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadEntidades();
          loadFilters();
        }
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al guardar entidad.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="space-y-6 animate-in fade-in duration-300">
      {/* Header Banner */}
      <div className="bg-gradient-to-r from-[#0D3048] to-[#184668] rounded-2xl p-6 text-white shadow-lg flex flex-col md:flex-row md:items-center justify-between gap-4 border border-[#194260]">
        <div className="flex items-center gap-4">
          <div className="p-3 bg-white/10 backdrop-blur-md rounded-xl border border-white/20">
            <Building2 className="w-8 h-8 text-amber-300" />
          </div>
          <div>
            <h1 className="text-xl sm:text-2xl font-black tracking-tight">Catálogo de Entidades Gubernamentales</h1>
            <p className="text-xs text-slate-300 mt-0.5">
              Catálogo oficial de 181 instituciones del Estado Dominicano conforme a especificaciones oficiales
            </p>
          </div>
        </div>

        <button
          onClick={handleOpenCreateModal}
          className="inline-flex items-center gap-2 px-4 py-2.5 rounded-xl bg-[#E64A19] hover:bg-[#d44315] text-white text-xs font-bold shadow-sm transition-all active:scale-95 cursor-pointer shrink-0"
        >
          <Plus className="w-4 h-4" />
          <span>Registrar Entidad</span>
        </button>
      </div>

      {/* Filters & Search Toolbar */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3 bg-white p-4 rounded-2xl border border-slate-200/80 shadow-sm">
        {/* Search */}
        <div className="relative sm:col-span-2 lg:col-span-1">
          <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3" />
          <input
            type="text"
            placeholder="Buscar por institución..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full text-xs pl-9 pr-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          />
        </div>

        {/* Sector Filter */}
        <div>
          <select
            value={selectedSector}
            onChange={(e) => setSelectedSector(e.target.value)}
            className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          >
            <option value="">Todos los Sectores</option>
            {sectores.map((s) => (
              <option key={s} value={s}>{s}</option>
            ))}
          </select>
        </div>

        {/* Poder Estado Filter */}
        <div>
          <select
            value={selectedPoder}
            onChange={(e) => setSelectedPoder(e.target.value)}
            className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          >
            <option value="">Todos los Poderes</option>
            {poderes.map((p) => (
              <option key={p} value={p}>{p}</option>
            ))}
          </select>
        </div>

        {/* Categoria Filter */}
        <div>
          <select
            value={selectedCategoria}
            onChange={(e) => setSelectedCategoria(e.target.value)}
            className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          >
            <option value="">Todas las Categorías</option>
            {categorias.map((c) => (
              <option key={c} value={c}>{c}</option>
            ))}
          </select>
        </div>
      </div>

      {/* Table List */}
      <div className="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-[#0D3048] text-white text-[11px] font-bold uppercase tracking-wider">
                <th className="py-3 px-4">Institución del Estado</th>
                <th className="py-3 px-4">Categoría</th>
                <th className="py-3 px-4">Poder del Estado</th>
                <th className="py-3 px-4">Sector</th>
                <th className="py-3 px-4 text-center">Estado</th>
                <th className="py-3 px-4 text-right">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100 text-xs">
              {loading ? (
                <tr>
                  <td colSpan={6} className="py-12 text-center text-slate-500">
                    <RefreshCw className="w-6 h-6 animate-spin mx-auto text-[#0D3048] mb-2" />
                    <span>Cargando catálogo de entidades...</span>
                  </td>
                </tr>
              ) : entidades.length === 0 ? (
                <tr>
                  <td colSpan={6} className="py-12 text-center text-slate-400">
                    No se encontraron entidades gubernamentales con los filtros aplicados.
                  </td>
                </tr>
              ) : (
                entidades.map((e) => (
                  <tr key={e.id} className="hover:bg-slate-50/80 transition-colors">
                    <td className="py-3.5 px-4 font-bold text-slate-900">
                      {e.nombre}
                    </td>
                    <td className="py-3.5 px-4 text-slate-600 text-[11px] max-w-xs truncate" title={e.categoria}>
                      {e.categoria}
                    </td>
                    <td className="py-3.5 px-4 text-slate-600">{e.poderEstado}</td>
                    <td className="py-3.5 px-4 font-semibold text-slate-700">{e.sector}</td>
                    <td className="py-3.5 px-4 text-center">
                      <span
                        className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold ${
                          e.activo ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-600'
                        }`}
                      >
                        {e.activo ? <CheckCircle2 className="w-3 h-3" /> : <XCircle className="w-3 h-3" />}
                        {e.activo ? 'Activa' : 'Inactiva'}
                      </span>
                    </td>
                    <td className="py-3.5 px-4 text-right space-x-1">
                      <button
                        onClick={() => handleOpenEditModal(e)}
                        title="Editar entidad"
                        className="p-1.5 rounded-lg text-slate-600 hover:text-[#0D3048] hover:bg-blue-50 border border-transparent hover:border-blue-200 transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs"
                      >
                        <Edit2 className="w-4 h-4" />
                      </button>
                      <button
                        onClick={() => setConfirmEntidad(e)}
                        title={e.activo ? 'Desactivar entidad' : 'Activar entidad'}
                        className={`p-1.5 rounded-lg transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs border border-transparent ${
                          e.activo
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
      </div>

      {/* Modal: Confirmación de Cambio de Estado de Entidad */}
      {confirmEntidad && (
        <ConfirmModal
          isOpen={true}
          onClose={() => setConfirmEntidad(null)}
          onConfirm={async () => {
            await handleToggleEstado(confirmEntidad.id);
          }}
          title={
            confirmEntidad.activo
              ? 'Confirmar Desactivación de Entidad'
              : 'Confirmar Activación de Entidad'
          }
          actionType={confirmEntidad.activo ? 'desactivar' : 'activar'}
          entityName={confirmEntidad.nombre}
          message={
            confirmEntidad.activo
              ? 'Al desactivar esta entidad pública, no estará disponible para asociarse a nuevas solicitudes institucionales.'
              : 'Al activar esta entidad pública, estará disponible nuevamente en el catálogo para selección de los usuarios.'
          }
        />
      )}

      {/* Modal: Crear / Editar Entidad (Únicamente los 4 campos oficiales del catálogo) */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingEntidad ? 'Editar Entidad Gubernamental' : 'Registrar Nueva Entidad Gubernamental'}
      >
        <form onSubmit={handleSubmit} className="space-y-4 pt-2">
          {/* 1. Nombre Oficial */}
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Nombre Oficial de la Entidad *</label>
            <input
              type="text"
              required
              maxLength={250}
              placeholder="e.g., Superintendencia de Bancos"
              value={formData.nombre}
              onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          {/* 2. Poder del Estado */}
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Poder del Estado *</label>
            <input
              type="text"
              required
              maxLength={100}
              placeholder="e.g., Poder Ejecutivo, Poder Legislativo, Poder Judicial"
              value={formData.poderEstado}
              onChange={(e) => setFormData({ ...formData, poderEstado: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          {/* 3. Sector */}
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Sector *</label>
            <input
              type="text"
              required
              maxLength={150}
              placeholder="e.g., Financiero, Hacienda, Salud, Educación"
              value={formData.sector}
              onChange={(e) => setFormData({ ...formData, sector: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          {/* 4. Categoría */}
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Categoría *</label>
            <input
              type="text"
              required
              maxLength={150}
              placeholder="e.g., Organismo Descentralizado Funcionalmente, Ministerio"
              value={formData.categoria}
              onChange={(e) => setFormData({ ...formData, categoria: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          <div className="pt-3 border-t border-slate-100 flex items-center justify-end gap-2">
            <button
              type="button"
              onClick={() => setIsModalOpen(false)}
              className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold hover:bg-slate-50 transition-colors cursor-pointer"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={submitting}
              className="px-5 py-2 rounded-xl bg-[#0D3048] hover:bg-[#143B58] text-white text-xs font-semibold shadow transition-all active:scale-95 disabled:opacity-50 cursor-pointer"
            >
              {submitting ? 'Guardando...' : editingEntidad ? 'Actualizar Entidad' : 'Guardar Entidad'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
