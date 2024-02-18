using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;
using webapi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//TOKEN

// Obtén la clave secreta de Jwt desde appsettings.json
var secretKey = builder.Configuration.GetSection("Jwt:SecretKey").Value;

if (secretKey != null)
{
    // Crea la clave de seguridad simétrica
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key
        };
    });
}
else
{
    // Manejo de error o mensaje de advertencia
}

//permitir conexion a:
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_ENLACE)");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<EnlaceContext>();
    dbContext.Database.EnsureCreated();
}

app.MapControllers();

app.MapGet("/dbconexion", async ([FromServices] EnlaceContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("¡Felicidades! La base de datos ha sido creada: " + dbContext.Database.IsInMemory());
});

app.Run();

/*
var app = builder.Build();

app.MapGet("/hello", () => "Hello World!");.

app.MapGet("/dbconexion", async ([FromServices] EnlaceContext DbContext) =>
{
    DbContext.Database.EnsureCreated();
    return Results.Ok("Felicidades base de Datos creada: " + DbContext.Database.IsInMemory());
});

app.MapGet("/api/alumnos", async ([FromServices] EnlaceContext DbContext) =>
{
    return Results.Ok(DbContext.Alumnos.Include(p=> p.AlumnoId));
});



app.MapPost("/api/alumnos", async ([FromServices] EnlaceContext DbContext, [FromBody] Alumnos alumnos) =>
{
    alumnos.AlumnoId = alumnos.AlumnoId;
    alumnos.FechaNacimiento = DateTime.Now;
    await DbContext.AddAsync(alumnos);
    //await DbContext.Tareas.AddAsync(tarea);

    await DbContext.SaveChangesAsync();

    return Results.Ok("Dato insertado");
});

app.MapPut("/api/tareas/{id}", async ([FromServices] EnlaceContext DbContext, [FromBody] Alumnos tarea, [FromRoute] Guid id) =>
{
    var tareaActual = DbContext.Alumnos.Find(id);

    if(tareaActual!=null)
    {
        tareaActual.AlumnoId = tarea.AlumnoId;
        tareaActual.Nombre = tarea.Nombre;
        tareaActual.Apellido = tarea.Apellido;
        tareaActual.FechaNacimiento = tarea.FechaNacimiento;

        await DbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound("error");
});


app.MapDelete("/api/tareas/{id}", async ([FromServices] EnlaceContext dbContext, [FromRoute] Guid id) =>
{
	var tarea = await dbContext.Alumnos.FindAsync(id);
	if (tarea == null)
	{
		return Results.NotFound();
	}
	dbContext.Alumnos.Remove(tarea);
	await dbContext.SaveChangesAsync();
	return Results.Ok();
});


/*app.MapGet("/api/encargados", async ([FromServices] EnlaceContext dbContext) =>
{
    var encargados = await dbContext.Encargados
        .Select(e => new {e.Nombre, e.Telefono, e.Direccion, e.Email })
        .ToListAsync();

    return Results.Ok(encargados);
});*/
