# Script de compilación y validación local de la solución .NET 8
Write-Host "==========================================================" -ForegroundColor Cipher
Write-Host " Superintendencia de Bancos (SB) - CI Validation Script  " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cipher

$SolutionPath = "src/backend/SB.GestionSolicitudes.sln"
$TestPath = "src/backend/SB.GestionSolicitudes.Tests/SB.GestionSolicitudes.Tests.csproj"

Write-Host "`n[1/3] Restando dependencias NuGet..." -ForegroundColor Yellow
dotnet restore $SolutionPath
if ($LASTEXITCODE -ne 0) { Write-Error "Falló la restauración de paquetes."; exit 1 }

Write-Host "`n[2/3] Compilando solución .NET 8..." -ForegroundColor Yellow
dotnet build $SolutionPath --configuration Release --no-restore
if ($LASTEXITCODE -ne 0) { Write-Error "Falló la compilación."; exit 1 }

Write-Host "`n[3/3] Ejecutando Pruebas Unitarias e Integración (xUnit)..." -ForegroundColor Yellow
dotnet test $TestPath --configuration Release --no-build --verbosity normal
if ($LASTEXITCODE -ne 0) { Write-Error "Las pruebas automatizadas fallaron."; exit 1 }

Write-Host "`n==========================================================" -ForegroundColor Green
Write-Host " ¡TODAS LAS VALIDACIONES SE COMPLETARON EXITOSAMENTE! ✔ " -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Green
