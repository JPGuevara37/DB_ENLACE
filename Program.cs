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

            IF COL_LENGTH('Profesores', 'Categoria') IS NULL ALTER TABLE Profesores ADD Categoria nvarchar(50) NULL;

            IF COL_LENGTH('RolesMes', 'Dia') IS NULL ALTER TABLE RolesMes ADD Dia int NOT NULL DEFAULT 1;

            IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_Materiales_MaterialId')
                ALTER TABLE Materiales ADD CONSTRAINT DF_Materiales_MaterialId DEFAULT (newid()) FOR MaterialId;

            IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = 'DF_RolesMes_RolMesId')
                ALTER TABLE RolesMes ADD CONSTRAINT DF_RolesMes_RolMesId DEFAULT (newid()) FOR RolMesId;

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolesMes]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[RolesMes] (
                    [RolMesId] uniqueidentifier NOT NULL PRIMARY KEY,
                    [EdadId] uniqueidentifier NOT NULL,
                    [PersonaId] uniqueidentifier NOT NULL,
                    [Mes] int NOT NULL,
                    [Anno] int NOT NULL,
                    [Estado] nvarchar(50) NULL,
                    [Disponible] bit NOT NULL,
                    [FechaCreacion] datetime2 NOT NULL
                );
            END");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error preparando la base de datos: {ex.Message}");
    }
}

app.MapControllers();

app.MapGet("/dbconexion", async ([FromServices] EnlaceContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("¡Felicidades! La base de datos ha sido creada: " + dbContext.Database.IsInMemory());
});

app.Run();