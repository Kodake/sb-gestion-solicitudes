# ADR-004: Despliegue en Contenedores con Next.js Standalone Runner

## Estado
Aceptado

## Contexto
El frontend está construido sobre **Next.js 16 (App Router)** y cuenta con rutas dinámicas (`/solicitudes/[id]`). Un enfoque estático puro mediante `output: 'export'` e imágenes Nginx no es compatible con el renderizado dinámico en demanda de Next.js ni permite aprovechar optimizaciones de servidor.

## Decisión
Se configura Next.js con `output: 'standalone'` en `next.config.ts` y se diseña un **Dockerfile multi-stage** basado en `node:20-alpine`:
1. **Etapa `deps`:** Instalación determinista de dependencias con `npm ci`.
2. **Etapa `builder`:** Compilación optimizada de producción (`npm run build`).
3. **Etapa `runner`:** Imagen liviana sin privilegios de root (`nextjs:nodejs`), copiando únicamente `.next/standalone`, `.next/static` y `public`, ejecutando `node server.js` en el puerto `3000`.

## Consecuencias
- **Positivas:** Contenedor Docker liviano (<150MB), totalmente funcional y reproducible en cualquier entorno con `docker-compose up`.
- **Negativas:** Requiere un runtime mínimo de Node.js en lugar de un servidor de archivos puramente estático como Nginx.
