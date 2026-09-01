'use client';

import React, { useState } from 'react';
import { StateBadge, PriorityBadge } from './Badge';

// --- PIE / DONUT CHART FOR ESTADO ---
interface EstadoPieChartData {
  estado: number;
  estadoNombre: string;
  cantidad: number;
}

interface EstadoPieChartProps {
  data: EstadoPieChartData[];
  total: number;
}

export const EstadoPieChart: React.FC<EstadoPieChartProps> = ({ data, total }) => {
  const [hoveredIdx, setHoveredIdx] = useState<number | null>(null);

  const getStateColor = (estado: number) => {
    switch (estado) {
      case 1: return { color: '#0EA5E9', bg: 'bg-sky-500' }; // Registrada
      case 2: return { color: '#0D3048', bg: 'bg-[#0D3048]' }; // En Análisis
      case 3: return { color: '#F59E0B', bg: 'bg-amber-500' }; // En Progreso
      case 4: return { color: '#F97316', bg: 'bg-orange-500' }; // En Espera
      case 5: return { color: '#10B981', bg: 'bg-emerald-500' }; // Resuelta
      case 6: return { color: '#64748B', bg: 'bg-slate-500' }; // Cerrada
      default: return { color: '#3B82F6', bg: 'bg-blue-500' };
    }
  };

  const totalValid = total || 1;
  const radius = 54;
  const circumference = 2 * Math.PI * radius; // ~339.29

  let accumulatedPercent = 0;
  const slices = data.map((item, idx) => {
    const percent = item.cantidad / totalValid;
    const strokeDasharray = `${percent * circumference} ${circumference}`;
    const strokeDashoffset = -accumulatedPercent * circumference;
    accumulatedPercent += percent;
    const colorObj = getStateColor(item.estado);

    return {
      ...item,
      percent,
      pctDisplay: Math.round(percent * 100),
      strokeDasharray,
      strokeDashoffset,
      color: colorObj.color,
      bg: colorObj.bg,
      idx,
    };
  });

  const activeSlice = hoveredIdx !== null ? slices[hoveredIdx] : null;

  return (
    <div className="flex flex-col sm:flex-row items-center gap-6">
      {/* SVG Donut Chart */}
      <div className="relative w-44 h-44 shrink-0 flex items-center justify-center">
        <svg className="w-full h-full -rotate-90 transform" viewBox="0 0 140 140">
          {/* Background Ring */}
          <circle
            cx="70"
            cy="70"
            r={radius}
            className="stroke-slate-100"
            strokeWidth="16"
            fill="transparent"
          />

          {/* Donut Slices */}
          {slices.map((slice) => {
            if (slice.cantidad === 0) return null;
            const isHovered = hoveredIdx === slice.idx;
            return (
              <circle
                key={slice.estado}
                cx="70"
                cy="70"
                r={radius}
                stroke={slice.color}
                strokeWidth={isHovered ? '20' : '16'}
                strokeDasharray={slice.strokeDasharray}
                strokeDashoffset={slice.strokeDashoffset}
                fill="transparent"
                onMouseEnter={() => setHoveredIdx(slice.idx)}
                onMouseLeave={() => setHoveredIdx(null)}
                className="transition-all duration-300 cursor-pointer origin-center"
              />
            );
          })}
        </svg>

        {/* Center Donut Label */}
        <div className="absolute inset-0 flex flex-col items-center justify-center text-center pointer-events-none p-2">
          <span className="text-2xl font-black text-[#0D3048] font-mono leading-none">
            {activeSlice ? activeSlice.cantidad : total}
          </span>
          <span className="text-[10px] font-bold text-slate-400 uppercase tracking-wider mt-1 truncate max-w-[100px]">
            {activeSlice ? activeSlice.estadoNombre : 'Total'}
          </span>
        </div>
      </div>

      {/* Legend & Breakdown list */}
      <div className="flex-1 w-full space-y-2">
        {slices.map((slice) => {
          const isHovered = hoveredIdx === slice.idx;
          return (
            <div
              key={slice.estado}
              onMouseEnter={() => setHoveredIdx(slice.idx)}
              onMouseLeave={() => setHoveredIdx(null)}
              className={`flex items-center justify-between p-2 rounded-xl border transition-all cursor-pointer ${
                isHovered ? 'bg-slate-50 border-[#0D3048]/30 shadow-sm scale-102' : 'bg-white border-slate-100'
              }`}
            >
              <div className="flex items-center gap-2">
                <span className={`w-2.5 h-2.5 rounded-full ${slice.bg} shadow-xs shrink-0`} />
                <StateBadge estado={slice.estado} />
              </div>

              <div className="text-right font-mono">
                <span className="text-xs font-bold text-slate-800">{slice.cantidad}</span>
                <span className="text-[10px] text-slate-400 ml-1 font-semibold">({slice.pctDisplay}%)</span>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

// --- PIE / DONUT CHART FOR PRIORIDAD ---
interface PieChartData {
  prioridad: number;
  prioridadNombre: string;
  cantidad: number;
}

interface PieChartProps {
  data: PieChartData[];
  total: number;
}

export const PrioridadPieChart: React.FC<PieChartProps> = ({ data, total }) => {
  const [hoveredIdx, setHoveredIdx] = useState<number | null>(null);

  const getPriorityColor = (prioridad: number) => {
    switch (prioridad) {
      case 1: return { color: '#10B981', label: 'Baja', bg: 'bg-emerald-500' }; // Baja
      case 2: return { color: '#3B82F6', label: 'Media', bg: 'bg-blue-500' }; // Media
      case 3: return { color: '#F59E0B', label: 'Alta', bg: 'bg-amber-500' }; // Alta
      case 4: return { color: '#EF4444', label: 'Crítica', bg: 'bg-red-500' }; // Crítica
      default: return { color: '#64748B', label: 'Normal', bg: 'bg-slate-500' };
    }
  };

  const totalValid = total || 1;
  const radius = 54;
  const circumference = 2 * Math.PI * radius; // ~339.29

  let accumulatedPercent = 0;
  const slices = data.map((item, idx) => {
    const percent = item.cantidad / totalValid;
    const strokeDasharray = `${percent * circumference} ${circumference}`;
    const strokeDashoffset = -accumulatedPercent * circumference;
    accumulatedPercent += percent;
    const colorObj = getPriorityColor(item.prioridad);

    return {
      ...item,
      percent,
      pctDisplay: Math.round(percent * 100),
      strokeDasharray,
      strokeDashoffset,
      color: colorObj.color,
      bg: colorObj.bg,
      idx,
    };
  });

  const activeSlice = hoveredIdx !== null ? slices[hoveredIdx] : null;

  return (
    <div className="flex flex-col sm:flex-row items-center gap-6">
      {/* SVG Donut Chart */}
      <div className="relative w-44 h-44 shrink-0 flex items-center justify-center">
        <svg className="w-full h-full -rotate-90 transform" viewBox="0 0 140 140">
          {/* Background Ring */}
          <circle
            cx="70"
            cy="70"
            r={radius}
            className="stroke-slate-100"
            strokeWidth="16"
            fill="transparent"
          />

          {/* Donut Slices */}
          {slices.map((slice) => {
            if (slice.cantidad === 0) return null;
            const isHovered = hoveredIdx === slice.idx;
            return (
              <circle
                key={slice.prioridad}
                cx="70"
                cy="70"
                r={radius}
                stroke={slice.color}
                strokeWidth={isHovered ? '20' : '16'}
                strokeDasharray={slice.strokeDasharray}
                strokeDashoffset={slice.strokeDashoffset}
                fill="transparent"
                onMouseEnter={() => setHoveredIdx(slice.idx)}
                onMouseLeave={() => setHoveredIdx(null)}
                className="transition-all duration-300 cursor-pointer origin-center"
              />
            );
          })}
        </svg>

        {/* Center Donut Label */}
        <div className="absolute inset-0 flex flex-col items-center justify-center text-center pointer-events-none p-2">
          <span className="text-2xl font-black text-[#0D3048] font-mono leading-none">
            {activeSlice ? activeSlice.cantidad : total}
          </span>
          <span className="text-[10px] font-bold text-slate-400 uppercase tracking-wider mt-1 truncate max-w-[100px]">
            {activeSlice ? activeSlice.prioridadNombre : 'Total'}
          </span>
        </div>
      </div>

      {/* Legend & Breakdown list */}
      <div className="flex-1 w-full space-y-2.5">
        {slices.map((slice) => {
          const isHovered = hoveredIdx === slice.idx;
          return (
            <div
              key={slice.prioridad}
              onMouseEnter={() => setHoveredIdx(slice.idx)}
              onMouseLeave={() => setHoveredIdx(null)}
              className={`flex items-center justify-between p-2.5 rounded-xl border transition-all cursor-pointer ${
                isHovered ? 'bg-slate-50 border-[#0D3048]/30 shadow-sm scale-102' : 'bg-white border-slate-100'
              }`}
            >
              <div className="flex items-center gap-2.5">
                <span className={`w-3 h-3 rounded-full ${slice.bg} shadow-xs shrink-0`} />
                <PriorityBadge prioridad={slice.prioridad} />
              </div>

              <div className="text-right font-mono">
                <span className="text-xs font-bold text-slate-800">{slice.cantidad}</span>
                <span className="text-[10px] text-slate-400 ml-1 font-semibold">({slice.pctDisplay}%)</span>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
