'use client';

import { useState, useEffect } from 'react';
import { solicitudService, FiltrosSolicitudParams } from '@/services/solicitudService';
import { catalogosService } from '@/services/catalogosService';
import { Area, PaginatedList, Solicitud } from '@/types';

export const useSolicitudesList = () => {
  const [data, setData] = useState<PaginatedList<Solicitud> | null>(null);
  const [areas, setAreas] = useState<Area[]>([]);
  const [loading, setLoading] = useState(true);

  // Filters state with default pageSize 5 to make pagination interactive immediately
  const [filtros, setFiltros] = useState<FiltrosSolicitudParams>({
    pageNumber: 1,
    pageSize: 5,
    searchTerm: '',
    estado: undefined,
    prioridad: undefined,
    areaId: undefined,
  });

  useEffect(() => {
    fetchAreas();
  }, []);

  useEffect(() => {
    fetchSolicitudes();
  }, [filtros.pageNumber, filtros.pageSize, filtros.estado, filtros.prioridad, filtros.areaId]);

  const fetchAreas = async () => {
    try {
      const res = await catalogosService.getAreas();
      if (res.success) setAreas(res.data);
    } catch (err) {
      console.error('Error al cargar áreas:', err);
    }
  };

  const fetchSolicitudes = async () => {
    setLoading(true);
    try {
      const res = await solicitudService.getSolicitudes(filtros);
      if (res.success) {
        setData(res.data);
      }
    } catch (err) {
      console.error('Error al cargar solicitudes:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setFiltros((prev) => ({ ...prev, pageNumber: 1 }));
    fetchSolicitudes();
  };

  const handleResetFilters = () => {
    setFiltros({
      pageNumber: 1,
      pageSize: 5,
      searchTerm: '',
      estado: undefined,
      prioridad: undefined,
      areaId: undefined,
    });
  };

  const updateFiltro = (field: keyof FiltrosSolicitudParams, value: any) => {
    setFiltros((prev) => ({
      ...prev,
      [field]: value,
      ...(field !== 'pageNumber' ? { pageNumber: 1 } : {}),
    }));
  };

  return {
    data,
    areas,
    loading,
    filtros,
    setFiltros,
    updateFiltro,
    handleSearchSubmit,
    handleResetFilters,
    fetchSolicitudes,
  };
};
