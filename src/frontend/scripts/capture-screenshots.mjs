import { chromium } from 'playwright';
import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const BASE_URL = 'http://localhost:3000';
const OUTPUT_DIR = path.resolve(__dirname, '../../../docs/screenshots');

if (!fs.existsSync(OUTPUT_DIR)) {
  fs.mkdirSync(OUTPUT_DIR, { recursive: true });
}

async function loginUser(page, email, password) {
  // Clear local storage and cookies
  await page.goto(`${BASE_URL}/login`, { waitUntil: 'networkidle' });
  await page.evaluate(() => {
    localStorage.clear();
    sessionStorage.clear();
  });
  await page.reload({ waitUntil: 'networkidle' });
  await page.waitForTimeout(500);

  await page.fill('input[type="email"]', email);
  await page.fill('input[type="password"]', password);
  await page.click('button[type="submit"]');
  await page.waitForURL('**/dashboard', { timeout: 15000 });
  await page.waitForTimeout(1000);
}

async function run() {
  console.log('🚀 Iniciando suite completa de capturas por ROL con Playwright...');
  console.log(`📁 Carpeta de salida: ${OUTPUT_DIR}`);

  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext({
    viewport: { width: 1440, height: 900 },
    deviceScaleFactor: 2,
  });

  const page = await context.newPage();

  try {
    // ==========================================
    // 0. PANTALLA DE LOGIN
    // ==========================================
    console.log('\n--- 0. PANTALLA DE LOGIN ---');
    await page.goto(`${BASE_URL}/login`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1000);
    await page.screenshot({ path: path.join(OUTPUT_DIR, '01_login_portal.png'), fullPage: true });

    // ==========================================
    // 1. FLUJO COMPLETO: ROL ADMINISTRADOR
    // ==========================================
    console.log('\n--- 1. FLUJO COMPLETO: ROL ADMINISTRADOR (admin@sb.gob.do) ---');
    await loginUser(page, 'admin@sb.gob.do', 'Admin123!');

    // 1.1 Dashboard Administrador
    console.log('📸 Capturando Dashboard Administrador...');
    await page.goto(`${BASE_URL}/dashboard`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_01_dashboard.png'), fullPage: true });

    // 1.2 Notificaciones en Campana
    console.log('📸 Capturando Dropdown de Notificaciones...');
    const bellBtn = page.locator('button[aria-label="Notificaciones"], button[title*="Notificaciones"]').first();
    if (await bellBtn.isVisible()) {
      await bellBtn.click();
      await page.waitForTimeout(800);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_02_notificaciones_campana.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.3 Listado de Solicitudes
    console.log('📸 Capturando Listado de Solicitudes...');
    await page.goto(`${BASE_URL}/solicitudes`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_03_solicitudes_listado.png'), fullPage: true });

    // 1.4 Detalle de Solicitud (Administrador)
    console.log('📸 Capturando Detalle de Solicitud...');
    await page.goto(`${BASE_URL}/solicitudes/1`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1500);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_04_solicitud_detalle.png'), fullPage: true });

    // 1.5 Modal de Edición de Solicitud
    console.log('📸 Capturando Modal de Edición de Solicitud...');
    const btnEditar = page.locator('button:has-text("Editar Información")').first();
    if (await btnEditar.isVisible()) {
      await btnEditar.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_05_solicitud_editar_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.6 Modal de Asignar Responsable
    console.log('📸 Capturando Modal de Asignar Responsable...');
    const btnAsignar = page.locator('button:has-text("Asignar Responsable")').first();
    if (await btnAsignar.isVisible()) {
      await btnAsignar.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_06_solicitud_asignar_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.7 Modal de Cambio de Estado
    console.log('📸 Capturando Modal de Cambio de Estado...');
    const btnEstado = page.locator('button:has-text("Cambiar Estado")').first();
    if (await btnEstado.isVisible()) {
      await btnEstado.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_07_solicitud_cambiar_estado_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.8 Módulo Gestión de Usuarios
    console.log('📸 Capturando Gestión de Usuarios...');
    await page.goto(`${BASE_URL}/admin/usuarios`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_08_usuarios_gestion.png'), fullPage: true });

    // 1.9 Modal Nuevo Usuario
    console.log('📸 Capturando Modal de Creación de Usuario...');
    const btnNuevoUsuario = page.locator('button:has-text("Nuevo Usuario")').first();
    if (await btnNuevoUsuario.isVisible()) {
      await btnNuevoUsuario.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_09_usuarios_crear_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.10 Modal Confirmación Toggle Usuario
    console.log('📸 Capturando Modal de Confirmación de Toggle de Usuario...');
    const btnToggleUser = page.locator('button[title*="Desactivar"], button[title*="Activar"]').first();
    if (await btnToggleUser.isVisible()) {
      await btnToggleUser.click();
      await page.waitForTimeout(500);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_10_usuarios_confirmar_estado_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.11 Módulo Entidades Gubernamentales
    console.log('📸 Capturando Módulo Entidades Gubernamentales...');
    await page.goto(`${BASE_URL}/admin/entidades`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_11_entidades_listado.png'), fullPage: true });

    // 1.12 Modal Nueva Entidad
    console.log('📸 Capturando Modal Nueva Entidad Gubernamental...');
    const btnNuevaEntidad = page.locator('button:has-text("Nueva Entidad")').first();
    if (await btnNuevaEntidad.isVisible()) {
      await btnNuevaEntidad.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_12_entidades_crear_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // 1.13 Módulo Catálogos
    console.log('📸 Capturando Administración de Catálogos...');
    await page.goto(`${BASE_URL}/admin/catalogos`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_13_catalogos_gestion.png'), fullPage: true });

    // 1.14 Modal Nueva Área
    console.log('📸 Capturando Modal Nueva Área...');
    const btnNuevaArea = page.locator('button:has-text("Nueva Área")').first();
    if (await btnNuevaArea.isVisible()) {
      await btnNuevaArea.click();
      await page.waitForTimeout(600);
      await page.screenshot({ path: path.join(OUTPUT_DIR, 'admin_14_catalogos_crear_area_modal.png') });
      await page.keyboard.press('Escape');
      await page.waitForTimeout(300);
    }

    // ==========================================
    // 2. FLUJO COMPLETO: ROL ANALISTA IT
    // ==========================================
    console.log('\n--- 2. FLUJO COMPLETO: ROL ANALISTA IT (analista.tech@sb.gob.do) ---');
    await loginUser(page, 'analista.tech@sb.gob.do', 'Analista123!');

    // 2.1 Dashboard Analista
    console.log('📸 Capturando Dashboard de Analista IT...');
    await page.goto(`${BASE_URL}/dashboard`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'analista_01_dashboard.png'), fullPage: true });

    // 2.2 Bandeja de Solicitudes del Analista
    console.log('📸 Capturando Listado de Solicitudes asignadas al Analista...');
    await page.goto(`${BASE_URL}/solicitudes`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'analista_02_solicitudes_atencion.png'), fullPage: true });

    // 2.3 Detalle Técnico y Comentarios (Analista)
    console.log('📸 Capturando Detalle Técnico y Notas Internas...');
    await page.goto(`${BASE_URL}/solicitudes/1`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1500);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'analista_03_solicitud_detalle_tecnico.png'), fullPage: true });

    // ==========================================
    // 3. FLUJO COMPLETO: ROL SOLICITANTE
    // ==========================================
    console.log('\n--- 3. FLUJO COMPLETO: ROL SOLICITANTE (juan.perez@sb.gob.do) ---');
    await loginUser(page, 'juan.perez@sb.gob.do', 'User123!');

    // 3.1 Dashboard Solicitante
    console.log('📸 Capturando Dashboard de Solicitante...');
    await page.goto(`${BASE_URL}/dashboard`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'solicitante_01_dashboard.png'), fullPage: true });

    // 3.2 Formulario Registro Nueva Solicitud
    console.log('📸 Capturando Formulario de Registro de Solicitud...');
    await page.goto(`${BASE_URL}/solicitudes/nueva`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'solicitante_02_solicitud_nueva.png'), fullPage: true });

    // 3.3 Listado de Solicitudes Propias
    console.log('📸 Capturando Listado de Solicitudes Propias...');
    await page.goto(`${BASE_URL}/solicitudes`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'solicitante_03_solicitudes_propias.png'), fullPage: true });

    // ==========================================
    // 4. VISTAS RESPONSIVAS MÓVILES (iPhone)
    // ==========================================
    console.log('\n--- 4. VISTAS RESPONSIVAS (Móvil) ---');
    await page.setViewportSize({ width: 390, height: 844 });
    await page.goto(`${BASE_URL}/dashboard`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'responsive_01_dashboard_mobile.png'), fullPage: true });

    await page.goto(`${BASE_URL}/solicitudes`, { waitUntil: 'networkidle' });
    await page.waitForTimeout(1200);
    await page.screenshot({ path: path.join(OUTPUT_DIR, 'responsive_02_solicitudes_mobile.png'), fullPage: true });

    console.log('\n🎉 ¡Suite completa de capturas por ROL generada exitosamente!');
  } catch (err) {
    console.error('❌ Error capturando pantallas:', err);
  } finally {
    await browser.close();
  }
}

run();
