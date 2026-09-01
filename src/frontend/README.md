# Frontend - Sistema de Gestión de Solicitudes (Next.js 16)

Aplicación Web Frontend desarrollada con **Next.js 16 (App Router)**, **React 19**, **TypeScript** y **TailwindCSS** para el Sistema de Gestión de Solicitudes Internas de la Superintendencia de Bancos (SB).

---

## 📋 Requisitos Previos

- **Node.js** (Versión 18.0 o superior instalada en el sistema).
  - Verificar con: `node -v`
- **npm** (Incluido por defecto con Node.js).
  - Verificar con: `npm -v`

---

## 🚀 Instalación y Ejecución

1. **Navegar a la carpeta del frontend:**
   ```bash
   cd src/frontend
   ```

2. **Instalar las dependencias del proyecto:**
   ```bash
   npm i
   ```

3. **Iniciar el servidor de desarrollo:**
   ```bash
   npm run dev
   ```

4. **Acceder en el navegador:**
   - URL: `http://localhost:3000`

---

## 🎨 Características Principales

- **Selector de Rol Institucional:** Botón interactivo de 1 clic en el header para probar los perfiles `Administrador`, `Analista` y `Solicitante`.
- **Tablero KPI & Gráficos Donut SVG:** Métricas de solicitudes Abiertas, Cerradas y Vencidas.
- **Centro de Notificaciones Toast:** Panel lateral flotante con historial de eventos.
- **Diseño Mobile-First & Responsive Drawer:** Adaptabilidad en dispositivos móviles y de escritorio.
