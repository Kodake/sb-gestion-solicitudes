'use client';

import { useState, useEffect } from 'react';
import { useParams } from 'next/navigation';
import { solicitudService } from '@/services/solicitudService';
import { catalogosService } from '@/services/catalogosService';
import { SolicitudDetalle, EstadoSolicitudEnum, User, RolEnum } from '@/types';
import toast from 'react-hot-toast';

export const useSolicitudDetalle = () => {
  const params = useParams();
  const id = Number(params?.id);

  const [solicitud, setSolicitud] = useState<SolicitudDetalle | null>(null);
  const [analistas, setAnalistas] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Modals state
  const [isEstadoModalOpen, setIsEstadoModalOpen] = useState(false);
  const [isAsignarModalOpen, setIsAsignarModalOpen] = useState(false);

  // Form states
  const [nuevoEstado, setNuevoEstado] = useState<number>(EstadoSolicitudEnum.EnProgreso);
  const [comentarioEstado, setComentarioEstado] = useState('');
  const [modalError, setModalError] = useState<string | null>(null);
  const [modalSubmitting, setModalSubmitting] = useState(false);

  const [responsableId, setResponsableId] = useState<number>(0);
  const [comentarioAsignar, setComentarioAsignar] = useState('');

  // Comment section state
  const [nuevoComentarioTexto, setNuevoComentarioTexto] = useState('');
  const [esPublico, setEsPublico] = useState(true);
  const [comentarioSubmitting, setComentarioSubmitting] = useState(false);

  useEffect(() => {
    if (id) {
      fetchDetalle();
      fetchAnalistas();
    }
  }, [id]);

  const fetchDetalle = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await solicitudService.getSolicitudById(id);
      if (res.success && res.data) {
        setSolicitud(res.data);
        setNuevoEstado(res.data.estado);
      } else {
        setError(res.message || 'Solicitud no encontrada');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al cargar detalle');
    } finally {
      setLoading(false);
    }
  };

  const fetchAnalistas = async () => {
    try {
      const res = await catalogosService.getUsuarios();
      if (res.success) {
        const staff = res.data.filter(
          (u) => u.rol === RolEnum.Analista || u.rol === RolEnum.Administrador
        );
        setAnalistas(staff);
        if (staff.length > 0) setResponsableId(staff[0].id);
      }
    } catch (err) {
      console.error('Error al cargar personal:', err);
    }
  };

  const handleCambiarEstadoSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setModalError(null);

    if (Number(nuevoEstado) === EstadoSolicitudEnum.Cerrada && !comentarioEstado.trim()) {
      const msg = 'Es obligatorio ingresar un comentario de resolución para poder cerrar una solicitud.';
      setModalError(msg);
      toast.error(msg);
      return;
    }

    setModalSubmitting(true);
    try {
      const res = await solicitudService.cambiarEstado(id, Number(nuevoEstado), comentarioEstado.trim());
      if (res.success) {
        toast.success('¡Estado de la solicitud actualizado correctamente!');
        setIsEstadoModalOpen(false);
        setComentarioEstado('');
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        fetchDetalle();
      } else {
        const errorMsg = res.message || 'Falló el cambio de estado';
        setModalError(errorMsg);
        toast.error(errorMsg);
      }
    } catch (err: any) {
      const errorMsg = err.response?.data?.message || 'Error al cambiar estado';
      setModalError(errorMsg);
      toast.error(errorMsg);
    } finally {
      setModalSubmitting(false);
    }
  };

  const handleAsignarSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setModalError(null);

    if (!responsableId) {
      const msg = 'Debe seleccionar un responsable técnico.';
      setModalError(msg);
      toast.error(msg);
      return;
    }

    setModalSubmitting(true);
    try {
      const res = await solicitudService.asignarResponsable(id, responsableId, comentarioAsignar.trim());
      if (res.success) {
        toast.success('¡Analista responsable asignado con éxito!');
        setIsAsignarModalOpen(false);
        setComentarioAsignar('');
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        fetchDetalle();
      } else {
        const errorMsg = res.message || 'Falló la asignación';
        setModalError(errorMsg);
        toast.error(errorMsg);
      }
    } catch (err: any) {
      const errorMsg = err.response?.data?.message || 'Error al asignar responsable';
      setModalError(errorMsg);
      toast.error(errorMsg);
    } finally {
      setModalSubmitting(false);
    }
  };

  const handleAgregarComentarioSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!nuevoComentarioTexto.trim()) return;

    setComentarioSubmitting(true);
    try {
      const res = await solicitudService.agregarComentario(id, nuevoComentarioTexto.trim(), esPublico);
      if (res.success) {
        toast.success('Comentario publicado exitosamente.');
        setNuevoComentarioTexto('');
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        fetchDetalle();
      }
    } catch (err: any) {
      toast.error(err.response?.data?.message || 'Error al publicar comentario');
    } finally {
      setComentarioSubmitting(false);
    }
  };

  return {
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
  };
};
