'use client';

import React, { useState, useEffect } from 'react';
import { Modal } from '@/components/ui/Modal';
import { ISolicitudDetalle, IArea, ITipoSolicitud, PrioridadEnum } from '@/types';
import { catalogosService } from '@/services/catalogosService';
import { solicitudService } from '@/services/solicitudService';
import { Edit3, Save, AlertCircle } from 'lucide-react';
import toast from 'react-hot-toast';

interface EditarSolicitudModalProps {
  isOpen: boolean;
  onClose: () => void;
  solicitud: ISolicitudDetalle;
  onActualizado: () => void;
}

export const EditarSolicitudModal: React.FC<EditarSolicitudModalProps> = ({
  isOpen,
  onClose,
  solicitud,
  onActualizado,
}) => {
  const [titulo, setTitulo] = useState(solicitud.titulo);
  const [descripcion, setDescripcion] = useState(solicitud.descripcion);
  const [areaId, setAreaId] = useState(solicitud.areaId);
  const [tipoSolicitudId, setTipoSolicitudId] = useState(solicitud.tipoSolicitudId);
  const [prioridad, setPrioridad] = useState(solicitud.prioridad);
  const [fechaCompromiso, setFechaCompromiso] = useState(
    solicitud.fechaCompromiso ? solicitud.fechaCompromiso.split('T')[0] : ''
  );
  const [referenciaEvidencia, setReferenciaEvidencia] = useState(solicitud.referenciaEvidencia || '');

  const [areas, setAreas] = useState<IArea[]>([]);
  const [tipos, setTipos] = useState<ITipoSolicitud[]>([]);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (isOpen) {
      setTitulo(solicitud.titulo);
      setDescripcion(solicitud.descripcion);
      setAreaId(solicitud.areaId);
      setTipoSolicitudId(solicitud.tipoSolicitudId);
      setPrioridad(solicitud.prioridad);
      setFechaCompromiso(solicitud.fechaCompromiso ? solicitud.fechaCompromiso.split('T')[0] : '');
      setReferenciaEvidencia(solicitud.referenciaEvidencia || '');

      loadCatalogos();
    }
  }, [isOpen, solicitud]);

  const loadCatalogos = async () => {
    try {
      setLoading(true);
      const [areasRes, tiposRes] = await Promise.all([
        catalogosService.getAreas(true),
        catalogosService.getTiposSolicitud(true),
      ]);
      if (areasRes.success) setAreas(areasRes.data);
      if (tiposRes.success) setTipos(tiposRes.data);
    } catch (err) {
      console.error(err);
      toast.error('Error al cargar catálogos.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!titulo.trim()) {
      toast.error('El título es obligatorio');
      return;
    }
    if (!descripcion.trim()) {
      toast.error('La descripción es obligatoria');
      return;
    }

    try {
      setSaving(true);
      const res = await solicitudService.actualizarSolicitud(solicitud.id, {
        titulo: titulo.trim(),
        descripcion: descripcion.trim(),
        areaId: Number(areaId),
        tipoSolicitudId: Number(tipoSolicitudId),
        prioridad: Number(prioridad),
        fechaCompromiso: fechaCompromiso ? new Date(fechaCompromiso).toISOString() : undefined,
        referenciaEvidencia: referenciaEvidencia.trim() || undefined,
      });

      if (res.success) {
        toast.success(res.message || 'Solicitud actualizada exitosamente.');
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        onActualizado();
        onClose();
      } else {
        toast.error(res.message || 'No se pudo actualizar la solicitud.');
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al guardar los cambios.');
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Editar Información de Solicitud">
      <form onSubmit={handleSubmit} className="space-y-4 pt-2">
        <div className="bg-slate-50 p-3 rounded-xl border border-slate-200 text-xs text-slate-600 flex items-center gap-2">
          <Edit3 className="w-4 h-4 text-blue-600 shrink-0" />
          <span>Editando solicitud: <strong className="text-[#0D3048]">{solicitud.codigo}</strong></span>
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-700 mb-1">Título de la Solicitud *</label>
          <input
            type="text"
            value={titulo}
            onChange={(e) => setTitulo(e.target.value)}
            required
            maxLength={150}
            className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          />
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-700 mb-1">Descripción Detallada *</label>
          <textarea
            rows={3}
            value={descripcion}
            onChange={(e) => setDescripcion(e.target.value)}
            required
            maxLength={2000}
            className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          />
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Área Destino *</label>
            <select
              value={areaId}
              onChange={(e) => setAreaId(Number(e.target.value))}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            >
              {areas.map((a) => (
                <option key={a.id} value={a.id}>{a.nombre}</option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Tipo de Solicitud *</label>
            <select
              value={tipoSolicitudId}
              onChange={(e) => setTipoSolicitudId(Number(e.target.value))}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            >
              {tipos.map((t) => (
                <option key={t.id} value={t.id}>{t.nombre}</option>
              ))}
            </select>
          </div>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Prioridad *</label>
            <select
              value={prioridad}
              onChange={(e) => setPrioridad(Number(e.target.value))}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            >
              <option value={PrioridadEnum.Baja}>Baja</option>
              <option value={PrioridadEnum.Media}>Media</option>
              <option value={PrioridadEnum.Alta}>Alta</option>
              <option value={PrioridadEnum.Critica}>Crítica</option>
            </select>
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Fecha Compromiso</label>
            <input
              type="date"
              value={fechaCompromiso}
              onChange={(e) => setFechaCompromiso(e.target.value)}
              className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>
        </div>

        <div>
          <label className="block text-xs font-bold text-slate-700 mb-1">Referencia o Evidencia</label>
          <input
            type="text"
            value={referenciaEvidencia}
            onChange={(e) => setReferenciaEvidencia(e.target.value)}
            placeholder="Enlace o ubicación de evidencia física"
            maxLength={500}
            className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          />
        </div>

        <div className="pt-3 border-t border-slate-100 flex items-center justify-end gap-2">
          <button
            type="button"
            onClick={onClose}
            className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold hover:bg-slate-50 transition-colors"
          >
            Cancelar
          </button>
          <button
            type="submit"
            disabled={saving}
            className="inline-flex items-center gap-1.5 px-5 py-2 rounded-xl bg-[#0D3048] hover:bg-[#143B58] text-white text-xs font-semibold shadow transition-all active:scale-95 disabled:opacity-50"
          >
            <Save className="w-3.5 h-3.5" />
            <span>{saving ? 'Guardando...' : 'Guardar Cambios'}</span>
          </button>
        </div>
      </form>
    </Modal>
  );
};
