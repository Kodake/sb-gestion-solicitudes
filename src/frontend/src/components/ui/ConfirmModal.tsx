'use client';

import React, { useState } from 'react';
import { Modal } from './Modal';
import { AlertTriangle, CheckCircle2, Power, RefreshCw } from 'lucide-react';

interface ConfirmModalProps {
  isOpen: boolean;
  onClose: () => void;
  onConfirm: () => Promise<void> | void;
  title: string;
  message: string;
  entityName?: string;
  actionType?: 'activar' | 'desactivar' | 'eliminar';
  confirmText?: string;
  cancelText?: string;
}

export const ConfirmModal: React.FC<ConfirmModalProps> = ({
  isOpen,
  onClose,
  onConfirm,
  title,
  message,
  entityName,
  actionType = 'desactivar',
  confirmText,
  cancelText = 'Cancelar',
}) => {
  const [loading, setLoading] = useState(false);

  const isDeactivating = actionType === 'desactivar' || actionType === 'eliminar';

  const defaultConfirmText = isDeactivating
    ? actionType === 'eliminar'
      ? 'Sí, Eliminar'
      : 'Sí, Desactivar'
    : 'Sí, Activar';

  const handleConfirm = async () => {
    try {
      setLoading(true);
      await onConfirm();
      onClose();
    } catch (error) {
      console.error('Error in confirm action:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title={title}>
      <div className="space-y-4 pt-1">
        {/* Warning / Confirmation Banner */}
        <div
          className={`flex items-start gap-3.5 p-4 rounded-xl border text-xs ${
            isDeactivating
              ? 'bg-amber-50/80 border-amber-200 text-amber-900'
              : 'bg-emerald-50/80 border-emerald-200 text-emerald-900'
          }`}
        >
          {isDeactivating ? (
            <AlertTriangle className="w-5 h-5 text-amber-600 shrink-0 mt-0.5" />
          ) : (
            <CheckCircle2 className="w-5 h-5 text-emerald-600 shrink-0 mt-0.5" />
          )}

          <div className="space-y-1 leading-normal">
            <p className="font-bold text-slate-900">
              {entityName ? (
                <>
                  ¿Está seguro que desea {actionType}{' '}
                  <span className="text-[#0D3048] font-extrabold underline decoration-amber-500/40">{entityName}</span>?
                </>
              ) : (
                `¿Está seguro que desea ${actionType} este registro?`
              )}
            </p>
            <p className="text-slate-600 leading-relaxed">{message}</p>
          </div>
        </div>

        {/* Footer Action Buttons */}
        <div className="pt-3 border-t border-slate-100 flex items-center justify-end gap-2">
          <button
            type="button"
            disabled={loading}
            onClick={onClose}
            className="px-4 py-2 rounded-xl border border-slate-200 text-slate-600 text-xs font-semibold hover:bg-slate-50 transition-colors disabled:opacity-50 cursor-pointer"
          >
            {cancelText}
          </button>

          <button
            type="button"
            disabled={loading}
            onClick={handleConfirm}
            className={`inline-flex items-center gap-1.5 px-5 py-2 rounded-xl text-white text-xs font-semibold shadow transition-all active:scale-95 disabled:opacity-50 cursor-pointer ${
              isDeactivating
                ? 'bg-red-600 hover:bg-red-700 focus:ring-2 focus:ring-red-400/50'
                : 'bg-emerald-600 hover:bg-emerald-700 focus:ring-2 focus:ring-emerald-400/50'
            }`}
          >
            {loading ? (
              <RefreshCw className="w-3.5 h-3.5 animate-spin" />
            ) : (
              <Power className="w-3.5 h-3.5" />
            )}
            <span>{confirmText || defaultConfirmText}</span>
          </button>
        </div>
      </div>
    </Modal>
  );
};
