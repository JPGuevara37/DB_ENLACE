#!/usr/bin/env bash
set -euo pipefail

# ============================================================
#  Arnes de verificacion — DB_ENLACE (backend .NET 10)
#  Ejecuta el diagnostico del entorno antes de tocar codigo.
#  Retorna 0 si todo esta en verde; distinto de 0 si algo falla.
# ============================================================

echo "== [1/4] Verificando herramientas =="
command -v dotnet >/dev/null 2>&1 || { echo "ERROR: 'dotnet' no esta instalado o no esta en el PATH."; exit 1; }
dotnet --version

echo "== [2/4] Verificando archivos criticos del arnes =="
for f in CLAUDE.md tasks.json; do
  if [ ! -f "$f" ]; then
    echo "ERROR: falta el archivo '$f'."
    exit 1
  fi
done

echo "== [3/4] Compilando =="
dotnet build DB_Enlace.csproj
BUILD_EXIT=$?
if [ "$BUILD_EXIT" -ne 0 ]; then
  echo "ERROR: la compilacion fallo. Revisa el reporte de 'dotnet build'."
  exit "$BUILD_EXIT"
fi

echo "== [4/4] Ejecutando tests (si existen) =="
TEST_PROJECTS=$(find . -maxdepth 3 -iname '*test*.csproj' -o -iname '*tests*.csproj' 2>/dev/null | head -n 1)
if [ -n "$TEST_PROJECTS" ]; then
  dotnet test
  TEST_EXIT=$?
  if [ "$TEST_EXIT" -ne 0 ]; then
    echo "ERROR: 'dotnet test' fallo."
    exit "$TEST_EXIT"
  fi
else
  echo "No se detecto un proyecto de tests; se omite 'dotnet test'."
fi

echo ""
echo "== OK: entorno en verde =="
exit 0
