'use client';

import { useState, useEffect } from 'react';
import { dashboardService } from '@/services/dashboardService';
import { DashboardResumen } from '@/types';

export const useDashboard = () => {
  const [data, setData] = useState<DashboardResumen | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchDashboard();
  }, []);

  const fetchDashboard = async () => {
    setLoading(true);
    try {
      const res = await dashboardService.getResumen();
      if (res.success) {
        setData(res.data);
      }
    } catch (err) {
      console.error('Error al cargar dashboard:', err);
    } finally {
      setLoading(false);
    }
  };

  const totalReqs = data?.totalSolicitudes || 1;

  return {
    data,
    loading,
    totalReqs,
    refetch: fetchDashboard,
  };
};
