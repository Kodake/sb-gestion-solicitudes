import React from 'react';
import { LucideIcon } from 'lucide-react';

interface StatCardProps {
  title: string;
  value: number;
  icon: LucideIcon;
  colorClass?: string;
  bgClass?: string;
  subtitle?: string;
}

export const StatCard: React.FC<StatCardProps> = ({
  title,
  value,
  icon: Icon,
  colorClass = 'text-[#0D3048]',
  bgClass = 'bg-[#EDF0F7]',
  subtitle,
}) => {
  return (
    <div className="group p-5 rounded-2xl border border-slate-200/80 bg-white shadow-sm hover:shadow-md hover:-translate-y-1 transition-all duration-300 flex items-center justify-between cursor-default">
      <div>
        <p className="text-[11px] font-bold text-slate-500 uppercase tracking-wider mb-1">{title}</p>
        <h3 className="text-3xl font-extrabold text-slate-900 group-hover:text-[#0D3048] transition-colors">{value}</h3>
        {subtitle && <p className="text-xs text-slate-500 mt-1 font-medium">{subtitle}</p>}
      </div>
      <div
        className={`w-12 h-12 rounded-2xl ${bgClass} ${colorClass} flex items-center justify-center shrink-0 transition-transform duration-300 group-hover:scale-110 group-hover:rotate-3 shadow-inner`}
      >
        <Icon className="w-6 h-6" />
      </div>
    </div>
  );
};
