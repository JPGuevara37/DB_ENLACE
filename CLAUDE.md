# DB_ENLACE — Backend API

API REST en **.NET 10** para el sistema de gestión de la iglesia/ministerio infantil Enlace.

## Stack

- ASP.NET Core 10 (Web API), C#
- EF Core 10 + SQL Server (Azure SQL)
- Autenticación JWT (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Email: MailKit + SendGrid
- Swagger (Swashbuckle)

## Mapa del repositorio

```
DB_Enlace.csproj            ← SDK Microsoft.NET.Sdk.Web, net10.0
Program.cs                  ← arranque, DI, CORS, Swagger
EnlaceContext.cs            ← DbContext EF Core + config de entidades
Controllers/                ← endpoints REST (ver lista abajo)
Models/                     ← entidades EF Core
Models/Dto/                 ← DTOs (ResetPasswordDto)
Services/                   ← lógica + interfaces
Helpers/                    ← EmailBody
Migrations/                 ← historial EF Core
docs/modelo-datos.md        ← modelo de datos (fuente de verdad)
specs/                      ← especificaciones SDD por tarea
progress/                   ← registro de avance de subagentes
tasks.json                  ← backlog (única fuente de verdad de estado)
```

## Endpoints

| Controlador | Ruta |
|---|---|
| `EncargadosController` | `/api/encargados` |
| `AlumnosController` | `/api/alumnos` |
| `ProfesoresController` | `/api/profesores` |
| `RecursosController` | `/api/recursos` |
| `EdadController` | `/api/edad` |
| `UsuariosController` | `/api/usuarios` |
| `AutenticarController` | `/api/autenticar` |
| `ResetEmailController` | `/api/ResetEmail` |

## Entidades

Alumnos, Encargados, Profesores, Clases, Edades, ClasesEdades, Asignaciones, Usuarios, Recursos.
Detalle completo en `docs/modelo-datos.md`.

## Reglas de comportamiento

- **Regla de arranque (obligatoria):** al iniciar una sesión, lee este archivo y ejecuta
  inmediatamente `init.sh` (Git Bash/WSL) o `init.ps1` (Windows PowerShell) antes de tocar código.
  Si el script falla, detente, reporta el error exacto y no modifiques lógica de negocio.
- Borrado lógico preferido; no borrar registros de negocio físicamente.
- Secretos fuera del repo (Key Vault / variables de entorno / user-secrets). Nunca commitear
  connection strings ni credenciales.
- Respuesta estándar de escritura: `ApiResponse { status, result }`.
- Los controladores delegan en un `Service` con interfaz (no llaman al DbContext directamente
  salvo los de autenticación/reset).
- Cambios de esquema de BD van acompañados de migración EF Core.

## Comandos

```bash
dotnet build                    # compilar
dotnet run --project DB_Enlace  # correr (HTTP :5132 / Swagger en /)
dotnet ef migrations add <Nombre>
dotnet ef database update
```

## Flujo SDD

1. Lee `tasks.json` para saber qué tarea sigue.
2. Si una tarea requiere SDD (`requiere_sdd: true`), escribe la spec en `specs/{id}-{nombre}.md`
   con requerimientos EARS y diseño técnico, y ponla en `spec_ready` para aprobación humana.
3. Implementa, registra avance en `progress/`, ejecuta `init.sh`/`init.ps1` y actualiza `tasks.json`.
