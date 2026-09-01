import type { Metadata } from 'next';
import './globals.css';
import { AuthProvider } from '@/context/AuthContext';
import { AppLayout } from '@/components/layout/AppLayout';
import { Toaster } from 'react-hot-toast';

export const metadata: Metadata = {
  title: 'Gestión de Solicitudes Internas - Superintendencia de Bancos',
  description: 'Sistema empresarial de registro, gestión y trazabilidad de solicitudes de servicios tecnológicos de la Superintendencia de Bancos (SB).',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="es">
      <body>
        <AuthProvider>
          <AppLayout>{children}</AppLayout>
          <Toaster
            position="top-right"
            toastOptions={{
              duration: 4000,
              style: {
                background: '#0D3048',
                color: '#ffffff',
                fontSize: '13px',
                fontWeight: '600',
                borderRadius: '12px',
                border: '1px solid rgba(255, 255, 255, 0.15)',
                boxShadow: '0 10px 25px -5px rgba(0, 0, 0, 0.2)',
              },
              success: {
                iconTheme: {
                  primary: '#10B981',
                  secondary: '#ffffff',
                },
              },
              error: {
                iconTheme: {
                  primary: '#EF4444',
                  secondary: '#ffffff',
                },
              },
            }}
          />
        </AuthProvider>
      </body>
    </html>
  );
}
