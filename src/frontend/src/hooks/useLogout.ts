'use client';

import { useState } from 'react';
import { useAuth } from '@/context/AuthContext';

export const useLogout = () => {
  const { logout } = useAuth();
  const [isLogoutModalOpen, setIsLogoutModalOpen] = useState(false);

  const openLogoutModal = () => setIsLogoutModalOpen(true);
  const closeLogoutModal = () => setIsLogoutModalOpen(false);

  const confirmLogout = (onBeforeLogout?: () => void) => {
    setIsLogoutModalOpen(false);
    if (onBeforeLogout) {
      onBeforeLogout();
    }
    logout();
  };

  return {
    isLogoutModalOpen,
    openLogoutModal,
    closeLogoutModal,
    confirmLogout,
  };
};
