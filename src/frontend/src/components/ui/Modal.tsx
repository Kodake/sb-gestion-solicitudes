'use client';

import React, { useEffect, useState } from 'react';
import { createPortal } from 'react-dom';
import { X } from 'lucide-react';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
  size?: 'sm' | 'md' | 'lg' | 'xl' | '2xl';
}

export const Modal: React.FC<ModalProps> = ({ isOpen, onClose, title, children, size = 'lg' }) => {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  useEffect(() => {
    if (!isOpen) return;

    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };

    document.body.style.overflow = 'hidden';
    window.addEventListener('keydown', handleEscape);

    return () => {
      document.body.style.overflow = 'unset';
      window.removeEventListener('keydown', handleEscape);
    };
  }, [isOpen, onClose]);

  if (!isOpen || !mounted) return null;

  const sizeClasses: Record<string, string> = {
    sm: 'max-w-sm',
    md: 'max-w-md',
    lg: 'max-w-lg',
    xl: 'max-w-xl',
    '2xl': 'max-w-2xl',
  };
  const sizeClass = sizeClasses[size] || 'max-w-lg';

  const modalContent = (
    <div className="fixed inset-0 z-[99999] flex items-center justify-center p-4 sm:p-6 bg-slate-900/60 backdrop-blur-sm transition-all duration-300">
      <div 
        className="fixed inset-0 bg-transparent" 
        onClick={onClose} 
        aria-hidden="true" 
      />
      <div className={`relative bg-white rounded-2xl ${sizeClass} w-full p-6 shadow-2xl border border-slate-100 flex flex-col max-h-[85vh] animate-zoom-in z-10 my-auto`}>
        {/* Modal Header */}
        <div className="flex items-center justify-between pb-4 border-b border-slate-100 mb-4">
          <h2 className="text-lg font-bold text-[#0D3048]">{title}</h2>
          <button
            onClick={onClose}
            className="p-1.5 rounded-xl text-slate-400 hover:text-slate-700 hover:bg-slate-100 transition-colors active:scale-95 cursor-pointer"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Modal Body */}
        <div className="flex-1 overflow-y-auto pr-1">{children}</div>
      </div>
    </div>
  );

  return createPortal(modalContent, document.body);
};
