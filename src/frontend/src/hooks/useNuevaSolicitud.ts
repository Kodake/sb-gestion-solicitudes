'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { catalogosService } from '@/services/catalogosService';
import { solicitudService, CrearSolicitudData } from '@/services/solicitudService';
import { Area, TipoSolicitud, PrioridadEnum } from '@/types';
import toast from 'react-hot-toast';

export const useNuevaSolicitud = () => {
  const router = useRouter();
  const [areas, setAreas] = useState<Area[]>([]);
  const [tipos, setTipos] = useState<TipoSolicitud[]>([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [formData, setFormData] = useState<CrearSolicitudData>({
    titulo: '',
    descripcion: '',
    prioridad: PrioridadEnum.Media,
    areaId: 0,
    tipoSolicitudId: 0,
    referenciaEvidencia: '',
    fechaCompromiso: '',
  });

  useEffect(() => {
    fetchCatalogos();
  }, []);

  const fetchCatalogos = async () => {
    try {
      const [resAreas, resTipos] = await Promise.all([
        catalogosService.getAreas(),
        catalogosService.getTiposSolicitud(),
      ]);

      if (resAreas.success) {
        setAreas(resAreas.data);
        if (resAreas.data.length > 0) {
          setFormData((prev) => ({ ...prev, areaId: resAreas.data[0].id }));
        }
      }
      if (resTipos.success) {
        setTipos(resTipos.data);
        if (resTipos.data.length > 0) {
          setFormData((prev) => ({ ...prev, tipoSolicitudId: resTipos.data[0].id }));
        }
      }
    } catch (err) {
      console.error('Error al cargar catálogos:', err);
    } finally {
      setLoading(false);
    }
  };

  const updateFormField = (field: keyof CrearSolicitudData, value: any) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!formData.titulo.trim()) {
      const msg = 'El título de la solicitud es obligatorio.';
      setError(msg);
      toast.error(msg);
      return;
    }
    if (!formData.descripcion.trim()) {
      const msg = 'La descripción detallada es obligatoria.';
      setError(msg);
      toast.error(msg);
      return;
    }
    if (!formData.areaId) {
      const msg = 'Debe seleccionar un área solicitante.';
      setError(msg);
      toast.error(msg);
      return;
    }
    if (!formData.tipoSolicitudId) {
      const msg = 'Debe seleccionar un tipo de solicitud.';
      setError(msg);
      toast.error(msg);
      return;
    }

    setSubmitting(true);
    try {
      const res = await solicitudService.crearSolicitud({
        ...formData,
        fechaCompromiso: formData.fechaCompromiso
          ? new Date(formData.fechaCompromiso).toISOString()
          : undefined,
      });

      if (res.success && res.data) {
        toast.success(`¡Solicitud ${res.data.codigo} registrada con éxito!`, {
          duration: 5000,
        });
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        router.push(`/solicitudes/${res.data.id}`);
      } else {
        const errorMsg = res.message || 'No se pudo crear la solicitud';
        setError(errorMsg);
        toast.error(errorMsg);
      }
    } catch (err: any) {
      const errorMsg = err.response?.data?.message || 'Error de comunicación con la API';
      setError(errorMsg);
      toast.error(errorMsg);
    } finally {
      setSubmitting(false);
    }
  };

  return {
    areas,
    tipos,
    loading,
    submitting,
    error,
    formData,
    updateFormField,
    handleSubmit,
  };
};
