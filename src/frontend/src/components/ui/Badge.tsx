import React from 'react';
import { EstadoSolicitudEnum, PrioridadEnum } from '@/types';

interface StateBadgeProps {
  estado: EstadoSolicitudEnum | string;
}

export const StateBadge: React.FC<StateBadgeProps> = ({ estado }) => {
  const getColors = () => {
    const num = Number(estado);
    if (!isNaN(num)) {
      switch (num) {
        case EstadoSolicitudEnum.Registrada:
          return 'bg-sky-100 text-sky-800 border-sky-300';
        case EstadoSolicitudEnum.EnAnalisis:
          return 'bg-purple-100 text-purple-800 border-purple-300';
        case EstadoSolicitudEnum.EnProgreso:
          return 'bg-amber-100 text-amber-900 border-amber-400 font-bold';
        case EstadoSolicitudEnum.EnEsperaDelSolicitante:
          return 'bg-orange-100 text-orange-800 border-orange-300';
        case EstadoSolicitudEnum.Resuelta:
          return 'bg-emerald-100 text-emerald-800 border-emerald-300';
        case EstadoSolicitudEnum.Cerrada:
          return 'bg-slate-200 text-slate-700 border-slate-300';
      }
    }

    const str = String(estado).toLowerCase();
    if (str.includes('progreso')) return 'bg-amber-100 text-amber-900 border-amber-400 font-bold';
    if (str.includes('análisis') || str.includes('analisis')) return 'bg-purple-100 text-purple-800 border-purple-300';
    if (str.includes('espera')) return 'bg-orange-100 text-orange-800 border-orange-300';
    if (str.includes('resuelta')) return 'bg-emerald-100 text-emerald-800 border-emerald-300';
    if (str.includes('cerrada')) return 'bg-slate-200 text-slate-700 border-slate-300';
    return 'bg-sky-100 text-sky-800 border-sky-300';
  };

  const getLabel = () => {
    const num = Number(estado);
    if (!isNaN(num)) {
      switch (num) {
        case EstadoSolicitudEnum.Registrada: return 'Registrada';
        case EstadoSolicitudEnum.EnAnalisis: return 'En Análisis';
        case EstadoSolicitudEnum.EnProgreso: return 'En Progreso';
        case EstadoSolicitudEnum.EnEsperaDelSolicitante: return 'En Espera del Solicitante';
        case EstadoSolicitudEnum.Resuelta: return 'Resuelta';
        case EstadoSolicitudEnum.Cerrada: return 'Cerrada';
      }
    }
    return String(estado);
  };

  return (
    <span
      style={{ whiteSpace: 'nowrap', display: 'inline-flex', alignItems: 'center' }}
      className={`px-3 py-1 rounded-full text-xs font-bold border whitespace-nowrap shrink-0 max-w-max select-none ${getColors()}`}
    >
      {getLabel()}
    </span>
  );
};

interface PriorityBadgeProps {
  prioridad: PrioridadEnum | string;
}

export const PriorityBadge: React.FC<PriorityBadgeProps> = ({ prioridad }) => {
  const getColors = () => {
    const num = Number(prioridad);
    if (!isNaN(num)) {
      switch (num) {
        case PrioridadEnum.Baja:
          return 'bg-[#E2F5EC] text-emerald-800 border-emerald-300';
        case PrioridadEnum.Media:
          return 'bg-blue-50 text-blue-800 border-blue-200';
        case PrioridadEnum.Alta:
          return 'bg-amber-100 text-amber-900 border-amber-300';
        case PrioridadEnum.Critica:
          return 'bg-red-100 text-red-800 border-red-300 font-bold animate-pulse';
      }
    }

    const str = String(prioridad).toLowerCase();
    if (str.includes('baja')) return 'bg-[#E2F5EC] text-emerald-800 border-emerald-300';
    if (str.includes('media')) return 'bg-blue-50 text-blue-800 border-blue-200';
    if (str.includes('alta')) return 'bg-amber-100 text-amber-900 border-amber-300';
    if (str.includes('critica') || str.includes('crítica')) return 'bg-red-100 text-red-800 border-red-300 font-bold animate-pulse';
    return 'bg-slate-100 text-slate-800 border-slate-200';
  };

  const getLabel = () => {
    const num = Number(prioridad);
    if (!isNaN(num)) {
      switch (num) {
        case PrioridadEnum.Baja: return 'Baja';
        case PrioridadEnum.Media: return 'Media';
        case PrioridadEnum.Alta: return 'Alta';
        case PrioridadEnum.Critica: return 'Crítica';
      }
    }
    return String(prioridad);
  };

  return (
    <span
      style={{ whiteSpace: 'nowrap', display: 'inline-flex', alignItems: 'center' }}
      className={`px-3 py-1 rounded-full text-xs font-bold border whitespace-nowrap shrink-0 max-w-max select-none ${getColors()}`}
    >
      {getLabel()}
    </span>
  );
};
