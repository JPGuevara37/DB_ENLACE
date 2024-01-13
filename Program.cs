using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;
using webapi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<EnlaceContext>(builder.Configuration.GetConnectionString("cnEnlace"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IHelloWorldService, HelloWorldService>();
//builder.Services.AddScoped<IHelloWorldService>(p => new HelloWorldService()); //Otra manera de inyectar la dependencia
builder.Services.AddScoped<IEncargadosService, EncargadosService>();
builder.Services.AddScoped<IAlumnosService, AlumnosService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseTimeMiddleware();

app.MapControllers();

app.Run();


/*
var app = builder.Build();

app.MapGet("/hello", () => "Hello World!");

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
