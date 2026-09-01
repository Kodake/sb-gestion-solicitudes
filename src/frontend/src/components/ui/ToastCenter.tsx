'use client';

import React, { useState } from 'react';
import toast, { useToasterStore, Toast } from 'react-hot-toast';
import {
  CheckCircle2,
  AlertOctagon,
  Info,
  XCircle,
  Trash2,
  BellRing,
  ChevronDown,
  ChevronUp,
} from 'lucide-react';

export const ToastCenter: React.FC = () => {
  const { toasts } = useToasterStore();
  const [isExpanded, setIsExpanded] = useState(true);
  const visibleToasts = toasts.filter((t) => t.visible);

  if (visibleToasts.length === 0) return null;

  const handleClearAll = () => {
    toast.dismiss();
  };

  const getToastIcon = (t: Toast) => {
    if (t.icon) return <span className="text-base shrink-0">{t.icon}</span>;

    switch (t.type) {
      case 'success':
        return <CheckCircle2 className="w-4 h-4 text-emerald-400 shrink-0" />;
      case 'error':
        return <AlertOctagon className="w-4 h-4 text-red-400 shrink-0 animate-bounce" />;
      case 'loading':
        return (
          <div className="w-3.5 h-3.5 border-2 border-amber-300/30 border-t-amber-400 rounded-full animate-spin shrink-0" />
        );
      default:
        return <Info className="w-4 h-4 text-blue-400 shrink-0" />;
    }
  };

  return (
    <div
      style={{ zIndex: 999999 }}
      className="fixed top-4 right-4 max-w-sm w-full space-y-2 pointer-events-auto select-none animate-fade-in"
    >
      {/* Bounded Notification Container Panel */}
      <div className="bg-[#0D3048]/95 backdrop-blur-xl border border-[#1d4b6c] rounded-2xl shadow-2xl p-2.5 space-y-2 transition-all">
        {/* Panel Header Bar */}
        <div className="flex items-center justify-between px-2.5 py-1.5 bg-[#133852]/80 rounded-xl text-white text-xs">
          {/* Title & Badge Toggle */}
          <button
            onClick={() => setIsExpanded(!isExpanded)}
            className="flex items-center gap-2 hover:text-amber-300 transition-colors cursor-pointer group"
          >
            <span className="relative flex h-2 w-2">
              <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-amber-400 opacity-75"></span>
              <span className="relative inline-flex rounded-full h-2 w-2 bg-amber-500"></span>
            </span>
            <span className="font-extrabold tracking-wide text-slate-100 flex items-center gap-1.5">
              <BellRing className="w-3.5 h-3.5 text-amber-400 group-hover:scale-110 transition-transform" />
              <span>Notificaciones</span>
              <span className="bg-[#E64A19] text-white text-[10px] font-mono font-extrabold px-1.5 py-0.2 rounded-full shadow-xs">
                {visibleToasts.length}
              </span>
            </span>
            {isExpanded ? (
              <ChevronUp className="w-3.5 h-3.5 text-slate-300 group-hover:text-white" />
            ) : (
              <ChevronDown className="w-3.5 h-3.5 text-slate-300 group-hover:text-white" />
            )}
          </button>

          {/* Clear All Toasts Action */}
          <button
            onClick={handleClearAll}
            className="inline-flex items-center gap-1 px-2.5 py-1 rounded-lg bg-white/10 hover:bg-red-500/20 text-slate-300 hover:text-red-300 border border-white/10 hover:border-red-400/40 text-[11px] font-semibold transition-all duration-200 active:scale-95 cursor-pointer"
          >
            <Trash2 className="w-3 h-3" />
            <span>Limpiar todas</span>
          </button>
        </div>

        {/* Scrollable & Bounded List of Stacked Toast Cards */}
        {isExpanded && (
          <div className="max-h-64 overflow-y-auto space-y-2 pr-1 custom-scrollbar transition-all duration-300">
            {visibleToasts.map((t) => (
              <div
                key={t.id}
                className="group relative flex items-start gap-2.5 p-3 bg-gradient-to-r from-[#0D3048] to-[#143B58] text-white border border-[#194260] rounded-xl shadow-md transition-all duration-200 hover:border-amber-400/50 hover:ring-1 hover:ring-amber-300/30"
              >
                {/* Type Icon */}
                <div className="pt-0.5">{getToastIcon(t)}</div>

                {/* Message Content */}
                <div className="flex-1 min-w-0 pr-6">
                  <div className="text-[11px] font-semibold text-slate-100 leading-snug break-words">
                    {typeof t.message === 'function' ? t.message(t) : t.message}
                  </div>
                </div>

                {/* Individual Close Button (Circle X) */}
                <button
                  onClick={() => toast.dismiss(t.id)}
                  title="Eliminar notificación"
                  className="absolute top-2.5 right-2.5 text-slate-400 hover:text-white hover:bg-white/20 p-1 rounded-full transition-all duration-200 hover:scale-110 active:scale-90 cursor-pointer"
                >
                  <XCircle className="w-3.5 h-3.5 text-slate-300 hover:text-red-400 transition-colors" />
                </button>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};
