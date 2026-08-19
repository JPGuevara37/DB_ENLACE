# ============================================================
#  Arnes de verificacion - DB_ENLACE (backend .NET 10)
#  Ejecuta el diagnostico del entorno antes de tocar codigo.
#  Retorna 0 si todo esta en verde; distinto de 0 si algo falla.
# ============================================================
$ErrorActionPreference = "Stop"

Write-Host "== [1/4] Verificando herramientas =="
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "'dotnet' no esta instalado o no esta en el PATH."
    exit 1
}
dotnet --version

Write-Host "== [2/4] Verificando archivos criticos del arnes =="
foreach ($f in @("CLAUDE.md", "tasks.json")) {
    if (-not (Test-Path -LiteralPath $f)) {
        Write-Error "Falta el archivo '$f'."
        exit 1
    }
}

Write-Host "== [3/4] Compilando =="
dotnet build DB_Enlace.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Error "La compilacion fallo. Revisa el reporte de 'dotnet build'."
    exit $LASTEXITCODE
}

Write-Host "== [4/4] Ejecutando tests (si existen) =="
$testProjects = Get-ChildItem -Recurse -Depth 3 -Include "*test*.csproj", "*tests*.csproj" -ErrorAction SilentlyContinue
if ($testProjects) {
    dotnet test
    if ($LASTEXITCODE -ne 0) {
        Write-Error "'dotnet test' fallo."
        exit $LASTEXITCODE
    }
} else {
    Write-Host "No se detecto un proyecto de tests; se omite 'dotnet test'."
}

Write-Host ""
Write-Host "== OK: entorno en verde =="
exit 0
