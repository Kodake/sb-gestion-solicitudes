'use client';

import React, { useState, useEffect } from 'react';
import {
  Users,
  Plus,
  Search,
  Edit2,
  Power,
  CheckCircle2,
  XCircle,
  RefreshCw,
  Shield,
  Key,
} from 'lucide-react';
import { usuariosService, CrearUsuarioData, ActualizarUsuarioData } from '@/services/usuariosService';
import { IUsuarioGestion, RolEnum } from '@/types';
import { Modal } from '@/components/ui/Modal';
import { ConfirmModal } from '@/components/ui/ConfirmModal';
import toast from 'react-hot-toast';

export default function UsuariosPage() {
  const [usuarios, setUsuarios] = useState<IUsuarioGestion[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedRol, setSelectedRol] = useState<string>('');

  // Modals
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [confirmUsuario, setConfirmUsuario] = useState<IUsuarioGestion | null>(null);
  const [editingUsuario, setEditingUsuario] = useState<IUsuarioGestion | null>(null);
  const [formData, setFormData] = useState<{
    nombre: string;
    correo: string;
    password?: string;
    rol: RolEnum;
    activo: boolean;
  }>({
    nombre: '',
    correo: '',
    password: '',
    rol: RolEnum.Solicitante,
    activo: true,
  });

  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadUsuarios();
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      loadUsuarios();
    }, 300);
    return () => clearTimeout(timer);
  }, [searchTerm, selectedRol]);

  const loadUsuarios = async () => {
    try {
      setLoading(true);
      const res = await usuariosService.getUsuarios({
        searchTerm: searchTerm.trim() || undefined,
        rol: selectedRol ? (Number(selectedRol) as RolEnum) : undefined,
        pageSize: 50,
      });
      if (res.success && res.data) {
        setUsuarios(res.data.items);
      }
    } catch (err) {
      console.error(err);
      toast.error('Error al cargar usuarios');
    } finally {
      setLoading(false);
    }
  };

  const handleOpenCreate = () => {
    setEditingUsuario(null);
    setFormData({
      nombre: '',
      correo: '',
      password: '',
      rol: RolEnum.Solicitante,
      activo: true,
    });
    setIsModalOpen(true);
  };

  const handleOpenEdit = (user: IUsuarioGestion) => {
    setEditingUsuario(user);
    setFormData({
      nombre: user.nombre,
      correo: user.correo,
      password: '',
      rol: user.rol,
      activo: user.activo,
    });
    setIsModalOpen(true);
  };

  const handleToggleEstado = async (id: number) => {
    try {
      const res = await usuariosService.toggleEstadoUsuario(id);
      if (res.success) {
        toast.success(res.message);
        if (typeof window !== 'undefined') {
          window.dispatchEvent(new Event('notification-refresh'));
        }
        loadUsuarios();
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al cambiar estado.');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.nombre.trim() || !formData.correo.trim()) {
      toast.error('Nombre y correo son obligatorios');
      return;
    }

    if (!editingUsuario && (!formData.password || formData.password.length < 6)) {
      toast.error('La contraseña debe tener al menos 6 caracteres');
      return;
    }

    try {
      setSubmitting(true);
      if (editingUsuario) {
        const updateData: ActualizarUsuarioData = {
          nombre: formData.nombre.trim(),
          correo: formData.correo.trim(),
          rol: Number(formData.rol),
          activo: formData.activo,
          nuevoPassword: formData.password?.trim() || undefined,
        };
        const res = await usuariosService.actualizarUsuario(editingUsuario.id, updateData);
        if (res.success) {
          toast.success(res.message);
          setIsModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadUsuarios();
        }
      } else {
        const createData: CrearUsuarioData = {
          nombre: formData.nombre.trim(),
          correo: formData.correo.trim(),
          password: formData.password!.trim(),
          rol: Number(formData.rol),
          activo: formData.activo,
        };
        const res = await usuariosService.crearUsuario(createData);
        if (res.success) {
          toast.success(res.message);
          setIsModalOpen(false);
          if (typeof window !== 'undefined') {
            window.dispatchEvent(new Event('notification-refresh'));
          }
          loadUsuarios();
        }
      }
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Error al guardar el usuario.');
    } finally {
      setSubmitting(false);
    }
  };

  const getRolBadge = (rol: RolEnum) => {
    switch (rol) {
      case RolEnum.Administrador:
        return <span className="px-2.5 py-0.5 rounded-full text-[10px] font-bold bg-amber-100 text-amber-800">Administrador</span>;
      case RolEnum.Analista:
        return <span className="px-2.5 py-0.5 rounded-full text-[10px] font-bold bg-blue-100 text-blue-800">Analista IT</span>;
      default:
        return <span className="px-2.5 py-0.5 rounded-full text-[10px] font-bold bg-emerald-100 text-emerald-800">Solicitante</span>;
    }
  };

  return (
    <div className="space-y-6">
      {/* Header Banner */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 p-6 bg-gradient-to-r from-[#0D3048] to-[#143B58] rounded-2xl text-white shadow-md border border-[#194260]">
        <div className="flex items-center gap-3.5">
          <div className="p-3 bg-white/10 rounded-xl ring-2 ring-white/10 shrink-0">
            <Users className="w-6 h-6 text-amber-300" />
          </div>
          <div>
            <h2 className="text-lg sm:text-xl font-extrabold tracking-tight">Gestión de Usuarios</h2>
            <p className="text-xs text-slate-300 mt-0.5">
              Administración de cuentas, roles de seguridad y accesos al sistema
            </p>
          </div>
        </div>

        <button
          onClick={handleOpenCreate}
          className="inline-flex items-center gap-2 px-4 py-2.5 rounded-xl bg-[#E64A19] hover:bg-[#d44315] text-white text-xs font-bold shadow-sm transition-all active:scale-95 cursor-pointer shrink-0"
        >
          <Plus className="w-4 h-4" />
          <span>Crear Usuario</span>
        </button>
      </div>

      {/* Toolbar */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 bg-white p-4 rounded-2xl border border-slate-200/80 shadow-sm">
        <div className="relative sm:col-span-2">
          <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3" />
          <input
            type="text"
            placeholder="Buscar por nombre o correo electrónico..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full text-xs pl-9 pr-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          />
        </div>

        <div>
          <select
            value={selectedRol}
            onChange={(e) => setSelectedRol(e.target.value)}
            className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-200 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
          >
            <option value="">Todos los Roles</option>
            <option value={RolEnum.Administrador}>Administrador</option>
            <option value={RolEnum.Analista}>Analista</option>
            <option value={RolEnum.Solicitante}>Solicitante</option>
          </select>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-[#0D3048] text-white text-[11px] font-bold uppercase tracking-wider">
              <th className="py-3 px-4">Usuario</th>
              <th className="py-3 px-4">Correo Electrónico</th>
              <th className="py-3 px-4">Rol del Sistema</th>
              <th className="py-3 px-4 text-center">Estado</th>
              <th className="py-3 px-4 text-right">Acciones</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-xs">
            {loading ? (
              <tr>
                <td colSpan={5} className="py-12 text-center text-slate-500">
                  <RefreshCw className="w-6 h-6 animate-spin mx-auto text-[#0D3048] mb-2" />
                  <span>Cargando usuarios...</span>
                </td>
              </tr>
            ) : usuarios.length === 0 ? (
              <tr>
                <td colSpan={5} className="py-12 text-center text-slate-400">
                  No se encontraron usuarios.
                </td>
              </tr>
            ) : (
              usuarios.map((u) => (
                <tr key={u.id} className="hover:bg-slate-50/80 transition-colors">
                  <td className="py-3.5 px-4 font-bold text-slate-900 flex items-center gap-2.5">
                    <div className="w-7 h-7 rounded-full bg-[#0D3048] text-white flex items-center justify-center text-xs font-bold">
                      {u.nombre.charAt(0).toUpperCase()}
                    </div>
                    <span>{u.nombre}</span>
                  </td>
                  <td className="py-3.5 px-4 text-slate-600 font-mono text-xs">{u.correo}</td>
                  <td className="py-3.5 px-4">{getRolBadge(u.rol)}</td>
                  <td className="py-3.5 px-4 text-center">
                    <span
                      className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full text-[10px] font-bold ${
                        u.activo ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-600'
                      }`}
                    >
                      {u.activo ? <CheckCircle2 className="w-3 h-3" /> : <XCircle className="w-3 h-3" />}
                      {u.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                  <td className="py-3.5 px-4 text-right space-x-1">
                    <button
                      onClick={() => handleOpenEdit(u)}
                      title="Editar usuario"
                      className="p-1.5 rounded-lg text-slate-600 hover:text-[#0D3048] hover:bg-blue-50 border border-transparent hover:border-blue-200 transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs"
                    >
                      <Edit2 className="w-4 h-4" />
                    </button>
                    <button
                      onClick={() => setConfirmUsuario(u)}
                      title={u.activo ? 'Desactivar usuario' : 'Activar usuario'}
                      className={`p-1.5 rounded-lg transition-all duration-200 cursor-pointer hover:scale-110 active:scale-95 shadow-xs border border-transparent ${
                        u.activo
                          ? 'text-amber-600 hover:bg-amber-100/80 hover:text-amber-700 hover:border-amber-300'
                          : 'text-emerald-600 hover:bg-emerald-100/80 hover:text-emerald-700 hover:border-emerald-300'
                      }`}
                    >
                      <Power className="w-4 h-4" />
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal: Confirmación de Cambio de Estado */}
      {confirmUsuario && (
        <ConfirmModal
          isOpen={true}
          onClose={() => setConfirmUsuario(null)}
          onConfirm={async () => {
            await handleToggleEstado(confirmUsuario.id);
          }}
          title={
            confirmUsuario.activo
              ? 'Confirmar Desactivación de Usuario'
              : 'Confirmar Activación de Usuario'
          }
          actionType={confirmUsuario.activo ? 'desactivar' : 'activar'}
          entityName={confirmUsuario.nombre}
          message={
            confirmUsuario.activo
              ? 'Al desactivar este usuario, no podrá iniciar sesión en el sistema ni gestionar solicitudes hasta que sea reactivado.'
              : 'Al activar este usuario, se restaurará su acceso inmediato al sistema con los permisos correspondientes a su rol.'
          }
        />
      )}

      {/* Modal: Usuario */}
      <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} title={editingUsuario ? 'Editar Usuario' : 'Nuevo Usuario'}>
        <form onSubmit={handleSubmit} className="space-y-4 pt-2">
          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Nombre Completo *</label>
            <input
              type="text"
              required
              maxLength={100}
              value={formData.nombre}
              onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          <div>
            <label className="block text-xs font-bold text-slate-700 mb-1">Correo Institucional (@sb.gob.do) *</label>
            <input
              type="email"
              required
              maxLength={100}
              value={formData.correo}
              onChange={(e) => setFormData({ ...formData, correo: e.target.value })}
              className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <div>
              <label className="block text-xs font-bold text-slate-700 mb-1">Rol de Acceso *</label>
              <select
                value={formData.rol}
                onChange={(e) => setFormData({ ...formData, rol: Number(e.target.value) as RolEnum })}
                className="w-full text-xs px-3 py-2.5 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
              >
                <option value={RolEnum.Administrador}>Administrador</option>
                <option value={RolEnum.Analista}>Analista IT</option>
                <option value={RolEnum.Solicitante}>Solicitante</option>
              </select>
            </div>

            <div>
              <label className="block text-xs font-bold text-slate-700 mb-1">
                {editingUsuario ? 'Nueva Contraseña (Opcional)' : 'Contraseña Inicial *'}
              </label>
              <input
                type="password"
                required={!editingUsuario}
                placeholder={editingUsuario ? 'Dejar en blanco para no cambiar' : 'Mínimo 6 caracteres'}
                value={formData.password}
                onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                className="w-full text-xs px-3 py-2 rounded-xl border border-slate-300 focus:outline-none focus:ring-2 focus:ring-[#0D3048]"
              />
            </div>
          </div>

          <div className="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button type="button" onClick={() => setIsModalOpen(false)} className="px-4 py-2 rounded-xl border border-slate-200 text-xs font-semibold">Cancelar</button>
            <button type="submit" disabled={submitting} className="px-5 py-2 rounded-xl bg-[#0D3048] text-white text-xs font-semibold shadow">
              {submitting ? 'Guardando...' : editingUsuario ? 'Actualizar Usuario' : 'Crear Usuario'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
