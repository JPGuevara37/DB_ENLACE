using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;
using webapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;


var builder = WebApplication.CreateBuilder(args);

//TOKEN
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey no configurado");
var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

// CORS: orígenes configurables (env var Cors__Origins, separados por coma)
var corsOrigins = (builder.Configuration["Cors:Origins"] ?? "http://localhost:4200")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSqlServer<EnlaceContext>(builder.Configuration.GetConnectionString("cnEnlace"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IHelloWorldService, HelloWorldService>();
//builder.Services.AddScoped<IHelloWorldService>(p => new HelloWorldService()); //Otra manera de inyectar la dependencia
builder.Services.AddScoped<IEncargadosService, EncargadosService>();
builder.Services.AddScoped<IAlumnosService, AlumnosService>();
builder.Services.AddScoped<IProfesoresService, ProfesoresService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRecursosServices, RecursosServices>();
builder.Services.AddScoped<IExampleService, ExampleService>();
builder.Services.AddScoped<IEdadesService, EdadesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IRolesMesService, RolesMesService>();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_ENLACE)");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<EnlaceContext>();
    const int maxAttempts = 12;
    for (var attempt = 1; ; attempt++)
    {
        try
        {
            dbContext.Database.EnsureCreated();
            break;
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            Console.WriteLine($"Base de datos no lista (intento {attempt}/{maxAttempts}): {ex.Message}");
            Thread.Sleep(5000);
        }
    }

    try
    {
        dbContext.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Materiales]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Materiales] (
                    [MaterialId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [Nombre] nvarchar(200) NULL,
                    [Descripcion] nvarchar(500) NULL,
                    [Fecha] datetime2 NOT NULL,
                    [Contenido] varbinary(max) NULL,
                    [ContentType] nvarchar(100) NULL,
                    [Tamano] bigint NOT NULL
                );
            END");

        dbContext.Database.ExecuteSqlRaw(@"
            UPDATE Usuarios SET Role = 'profes' WHERE Role IS NULL OR Role = '' OR Role = 'User'");

        dbContext.Database.ExecuteSqlRaw(@"
            UPDATE Usuarios SET Role = 'administrador' WHERE LTRIM(RTRIM(Usuario_Cuenta)) = 'jose.guevara'");

        dbContext.Database.ExecuteSqlRaw(@"
            UPDATE Usuarios SET Role = 'lidere' WHERE LTRIM(RTRIM(Usuario_Cuenta)) = 'Pri Araya'");

        dbContext.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_MigActivarUsuarios]') AND type in (N'U'))
            BEGIN
                UPDATE Usuarios SET Activo = 1 WHERE Activo IS NULL OR Activo = 0;
                CREATE TABLE [dbo].[_MigActivarUsuarios] ([Id] int IDENTITY(1,1) NOT NULL);
            END");

        dbContext.Database.ExecuteSqlRaw(@"
            IF COL_LENGTH('Encargados', 'Provincia') IS NULL ALTER TABLE Encargados ADD Provincia nvarchar(100) NULL;
            IF COL_LENGTH('Encargados', 'Canton') IS NULL ALTER TABLE Encargados ADD Canton nvarchar(100) NULL;
            IF COL_LENGTH('Encargados', 'Distrito') IS NULL ALTER TABLE Encargados ADD Distrito nvarchar(100) NULL;

            IF COL_LENGTH('Materiales', 'Categoria') IS NULL ALTER TABLE Materiales ADD Categoria nvarchar(100) NULL;
            IF COL_LENGTH('Materiales', 'Mes') IS NULL ALTER TABLE Materiales ADD Mes int NULL;
            IF COL_LENGTH('Materiales', 'Anno') IS NULL ALTER TABLE Materiales ADD Anno int NULL;
            IF COL_LENGTH('Materiales', 'Dia') IS NULL ALTER TABLE Materiales ADD Dia int NULL;

            IF COL_LENGTH('Recursos', 'Categoria') IS NULL ALTER TABLE Recursos ADD Categoria nvarchar(100) NULL;

            IF COL_LENGTH('Profesores', 'Categoria') IS NULL ALTER TABLE Profesores ADD Categoria nvarchar(50) NULL;

            IF COL_LENGTH('Profesores', 'Avatar') IS NULL ALTER TABLE Profesores ADD Avatar nvarchar(max) NULL;

            UPDATE Profesores SET Categoria = 'Asistente' WHERE Categoria = 'Equipo de apoyo';

            IF COL_LENGTH('Usuarios', 'Avatar') IS NULL ALTER TABLE Usuarios ADD Avatar nvarchar(max) NULL;

            IF COL_LENGTH('RolesMes', 'Dia') IS NULL ALTER TABLE RolesMes ADD Dia int NOT NULL DEFAULT 1;

            IF COL_LENGTH('RolesMes', 'Respuesta') IS NULL ALTER TABLE RolesMes ADD Respuesta nvarchar(50) NULL;

            IF COL_LENGTH('RolesMes', 'Tipo') IS NULL ALTER TABLE RolesMes ADD Tipo nvarchar(50) NULL;

            IF COL_LENGTH('RolesMes', 'Motivo') IS NULL ALTER TABLE RolesMes ADD Motivo nvarchar(500) NULL;

            ALTER TABLE RolesMes ALTER COLUMN EdadId uniqueidentifier NULL;

            IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Materiales_MaterialId')
                ALTER TABLE Materiales ADD CONSTRAINT DF_Materiales_MaterialId DEFAULT (newid()) FOR MaterialId;

            IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_RolesMes_RolMesId')
                ALTER TABLE RolesMes ADD CONSTRAINT DF_RolesMes_RolMesId DEFAULT (newid()) FOR RolMesId;

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolesMes]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[RolesMes] (
                    [RolMesId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [EdadId] uniqueidentifier NULL,
                    [PersonaId] uniqueidentifier NOT NULL,
                    [Mes] int NOT NULL,
                    [Anno] int NOT NULL,
                    [Tipo] nvarchar(50) NULL,
                    [Estado] nvarchar(50) NULL,
                    [Disponible] bit NOT NULL,
                    [Respuesta] nvarchar(50) NULL,
                    [Motivo] nvarchar(500) NULL,
                    [FechaCreacion] datetime2 NOT NULL
                );
            END

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContenidoPortal]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[ContenidoPortal] (
                    [ContenidoId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [Seccion] nvarchar(50) NULL,
                    [Titulo] nvarchar(200) NULL,
                    [Detalle] nvarchar(500) NULL,
                    [Icono] nvarchar(50) NULL,
                    [Orden] int NOT NULL
                );
            END

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaterialClase]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[MaterialClase] (
                    [MaterialClaseId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [RecursoId] uniqueidentifier NOT NULL,
                    [Clase] nvarchar(50) NULL,
                    [Cantidad] int NOT NULL
                );
            END

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CenaSenor]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[CenaSenor] (
                    [CenaSenorId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [Mes] int NOT NULL,
                    [Anno] int NOT NULL,
                    [Dia] int NOT NULL
                );
            END

            IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_CenaSenor_CenaSenorId')
                ALTER TABLE CenaSenor ADD CONSTRAINT DF_CenaSenor_CenaSenorId DEFAULT (newid()) FOR CenaSenorId;
");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error preparando la base de datos: {ex.Message}");
    }

    try
    {
        if (!dbContext.ContenidoPortal.Any(c => c.Seccion == "meta"))
        {
            var metas = new[]
            {
                "Continuar con el material propio, estudiando libros enteros de la Biblia.",
                "Capacitar al equipo de servidores para tratar niños con capacidades especiales y niños en riesgo, además de proveer más técnicas de enseñanza.",
                "Ofrecer discipulados a aquellos que quieran incorporarse al equipo para enseñar.",
                "Reunir 4 veces al año al equipo de trabajo completo para capacitación y evaluación.",
                "Realizar seguimientos individuales (1v1) a cada servidor.",
                "Realizar reunión de padres al menos una vez al año.",
                "Entrevistar a familias nuevas para actualizar la base de datos.",
            };
            for (var i = 0; i < metas.Length; i++)
            {
                dbContext.ContenidoPortal.Add(new ContenidoPortal
                {
                    ContenidoId = Guid.NewGuid(),
                    Seccion = "meta",
                    Detalle = metas[i],
                    Orden = i + 1,
                });
            }
        }

        if (!dbContext.ContenidoPortal.Any(c => c.Seccion == "actividad"))
        {
            dbContext.ContenidoPortal.AddRange(
                new ContenidoPortal { ContenidoId = Guid.NewGuid(), Seccion = "actividad", Titulo = "Escuelita de vacaciones", Detalle = "16 al 18 de julio, 2026", Icono = "fa-sun", Orden = 1 },
                new ContenidoPortal { ContenidoId = Guid.NewGuid(), Seccion = "actividad", Titulo = "Día del Niño (Evangelístico)", Detalle = "13 de septiembre", Icono = "fa-children", Orden = 2 },
                new ContenidoPortal { ContenidoId = Guid.NewGuid(), Seccion = "actividad", Titulo = "Fiesta de Navidad", Detalle = "13 de diciembre", Icono = "fa-gift", Orden = 3 }
            );
        }

        dbContext.SaveChanges();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error sembrando contenido del portal: {ex.Message}");
    }
}

app.MapControllers();

app.MapGet("/dbconexion", async ([FromServices] EnlaceContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("¡Felicidades! La base de datos ha sido creada: " + dbContext.Database.IsInMemory());
});

app.Run();