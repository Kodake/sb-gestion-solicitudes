# 📸 Guía y Evidencias Visuales de Flujos por Rol (Playwright E2E)
**Sistema de Gestión de Solicitudes Internas — Superintendencia de Bancos (SB)**

Este documento recopila la ejecución automatizada con **Playwright** de todas las vistas, flujos operativos, reglas de negocio y componentes de la plataforma clasificados por cada rol de usuario y responsividad.

---

## 📑 Índice de Contenidos

1. [🔐 Portal de Autenticación / Login](#-1-portal-de-autenticación--login)
2. [👑 Flujo del Rol Administrador (`admin@sb.gob.do`)](#-2-flujo-del-rol-administrador-adminsbgobdo)
3. [💻 Flujo del Rol Analista IT (`analista.tech@sb.gob.do`)](#-3-flujo-del-rol-analista-it-analistatechsbgobdo)
4. [📝 Flujo del Rol Solicitante (`juan.perez@sb.gob.do`)](#-4-flujo-del-rol-solicitante-juanperezsbgobdo)
5. [📱 Vistas Responsivas (Dispositivos Móviles)](#-5-vistas-responsivas-dispositivos-móviles)
6. [⚙️ Cómo Ejecutar las Pruebas de Captura](#-6-cómo-ejecutar-las-pruebas-de-captura)

---

## 🔐 1. Portal de Autenticación / Login

Vista de inicio de sesión con paleta institucional de la Superintendencia de Bancos, selector rápido de perfiles de prueba y validación de credenciales JWT.

| Evidencia | Descripción |
|---|---|
| `docs/screenshots/01_login_portal.png` | Formulario de autenticación institucional, credenciales de demostración y botón de acceso seguro. |

---

## 👑 2. Flujo del Rol Administrador (`admin@sb.gob.do`)

El Administrador tiene acceso total a todos los módulos: analítica global, gestión de usuarios, entidades gubernamentales, catálogos del sistema, edición y asignación de requerimientos.

### 2.1 Tablero de Control y Alertas
- **Dashboard Ejecutivo**: Resumen de indicadores clave de desempeño (KPIs), solicitudes registradas, en análisis, resueltas y gráficos de distribución.
  - 🖼️ `docs/screenshots/admin_01_dashboard.png`
- **Bandeja de Notificaciones en Tiempo Real**: Campana con badge numérico, lista de eventos recientes en base de datos y acciones de eliminación/limpieza.
  - 🖼️ `docs/screenshots/admin_02_notificaciones_campana.png`

### 2.2 Gestión Integral de Solicitudes
- **Listado y Filtros Avanzados**: Búsqueda por código, título, estado, prioridad y área destino con paginación.
  - 🖼️ `docs/screenshots/admin_03_solicitudes_listado.png`
- **Detalle de la Solicitud**: Vista completa del requerimiento con línea de tiempo histórica, datos del solicitante y analista asignado.
  - 🖼️ `docs/screenshots/admin_04_solicitud_detalle.png`
- **Modal de Edición de Solicitud**: Modificación de título, descripción, prioridad, área y fecha compromiso.
  - 🖼️ `docs/screenshots/admin_05_solicitud_editar_modal.png`
- **Modal de Asignación Técnica**: Selección del analista o administrador responsable con comentario de asignación.
  - 🖼️ `docs/screenshots/admin_06_solicitud_asignar_modal.png`
- **Modal de Cambio de Estado**: Transición de estado respetando la matriz formal con comentario obligatorio para cierre.
  - 🖼️ `docs/screenshots/admin_07_solicitud_cambiar_estado_modal.png`

### 2.3 Mantenimientos del Sistema
- **Gestión de Usuarios**: Tabla de usuarios con búsqueda, filtro por rol y botón de activación/desactivación.
  - 🖼️ `docs/screenshots/admin_08_usuarios_gestion.png`
- **Modal de Confirmación de Estado de Usuario**: Confirmación segura antes de activar o suspender un usuario.
  - 🖼️ `docs/screenshots/admin_10_usuarios_confirmar_estado_modal.png`
- **Entidades Gubernamentales**: Catálogo oficial de entidades del Estado dominicano por sector y poder del Estado.
  - 🖼️ `docs/screenshots/admin_11_entidades_listado.png`
- **Administración de Catálogos**: Mantenimiento dual de Áreas institucionales y Tipos de Solicitud.
  - 🖼️ `docs/screenshots/admin_13_catalogos_gestion.png`
  - 🖼️ `docs/screenshots/admin_14_catalogos_crear_area_modal.png`

---

## 💻 3. Flujo del Rol Analista IT (`analista.tech@sb.gob.do`)

El Analista IT se enfoca en la resolución operativa, atención de incidentes, avance en el flujo de estados y comunicación técnica con el solicitante.

| Paso / Vista | Evidencia | Descripción Operativa |
|---|---|---|
| **Dashboard del Analista** | `docs/screenshots/analista_01_dashboard.png` | Métricas orientadas a solicitudes asignadas, pendientes de respuesta y resueltas. |
| **Bandeja de Atención** | `docs/screenshots/analista_02_solicitudes_atencion.png` | Filtro enfocado en requerimientos bajo su responsabilidad técnica. |
| **Detalle Técnico y Notas** | `docs/screenshots/analista_03_solicitud_detalle_tecnico.png` | Interfaz para agregar comentarios públicos o notas internas confidenciales entre analistas. |

---

## 📝 4. Flujo del Rol Solicitante (`juan.perez@sb.gob.do`)

El Solicitante es el usuario final institucional que registra necesidades y hace seguimiento a sus casos.

| Paso / Vista | Evidencia | Descripción Operativa |
|---|---|---|
| **Dashboard de Solicitante** | `docs/screenshots/solicitante_01_dashboard.png` | Vista simplificada con el estado de sus propios tickets de servicio. |
| **Registro de Requerimiento** | `docs/screenshots/solicitante_02_solicitud_nueva.png` | Formulario guiado con selección de área destino, tipo de requerimiento, prioridad y enlaces de evidencia. |
| **Mis Solicitudes** | `docs/screenshots/solicitante_03_solicitudes_propias.png` | Tabla con historial de solicitudes generadas por el usuario autenticado. |

---

## 📱 5. Vistas Responsivas (Dispositivos Móviles)

La aplicación fue construida con diseño adaptativo (*Mobile-First*), garantizando una experiencia óptima en teléfonos inteligentes y tabletas.

| Dispositivo / Vista | Evidencia | Detalles |
|---|---|---|
| **Dashboard Móvil (iPhone)** | `docs/screenshots/responsive_01_dashboard_mobile.png` | Menú colapsable, tarjetas KPI apiladas y gráficos adaptados a pantallas estrechas. |
| **Listado de Solicitudes Móvil** | `docs/screenshots/responsive_02_solicitudes_mobile.png` | Tabla responsiva con desplazamiento y tarjetas condensadas. |

---

## ⚙️ 6. Cómo Ejecutar las Pruebas de Captura

Para re-ejecutar toda la suite automatizada de capturas en cualquier momento:

1. Asegúrate de tener el Backend y Frontend corriendo (`http://localhost:5000` y `http://localhost:3000`).
2. En la carpeta `src/frontend`, ejecuta:
   ```powershell
   npm run test:screenshots
   ```
3. El script de Playwright ejecutará la secuencia headless en alta resolución (2x Retina) y actualizará automáticamente todas las imágenes en la carpeta `docs/screenshots/`.
