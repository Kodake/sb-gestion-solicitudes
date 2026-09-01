import { test, expect } from '@playwright/test';

test.describe('Portal de Autenticación y Flujos de Usuario', () => {
  test('La página de inicio de sesión carga correctamente con los elementos institucionales', async ({ page }) => {
    await page.goto('/login');

    // Validar logo institucional de la Superintendencia de Bancos
    const logo = page.locator('img[alt="Superintendencia de Bancos"]');
    await expect(logo).toBeVisible();

    // Validar título y subtítulo del sistema
    await expect(page.getByText('Gestión de Solicitudes Internas')).toBeVisible();
    await expect(page.getByText('Ingrese sus credenciales de acceso institucional')).toBeVisible();

    // Validar campos de entrada
    const emailInput = page.locator('input[type="email"]');
    const passwordInput = page.locator('input[type="password"]');
    const submitBtn = page.locator('button[type="submit"]');

    await expect(emailInput).toBeVisible();
    await expect(passwordInput).toBeVisible();
    await expect(submitBtn).toBeVisible();
    await expect(submitBtn).toContainText('Iniciar Sesión');
  });

  test('Los botones de selección rápida rellenan el correo correspondiente', async ({ page }) => {
    await page.goto('/login');

    const emailInput = page.locator('input[type="email"]');

    // Click en Admin
    await page.getByRole('button', { name: /Admin/i }).click();
    await expect(emailInput).toHaveValue('admin@sb.gob.do');

    // Click en Analista
    await page.getByRole('button', { name: /Analista/i }).click();
    await expect(emailInput).toHaveValue('analista.tech@sb.gob.do');

    // Click en Solicitante
    await page.getByRole('button', { name: /Solicitante/i }).click();
    await expect(emailInput).toHaveValue('juan.perez@sb.gob.do');
  });

  test('Redirección a login cuando no se está autenticado', async ({ page }) => {
    // Intentar acceder a una ruta protegida sin token
    await page.goto('/dashboard');
    await expect(page).toHaveURL(/.*\/login/);
  });
});
