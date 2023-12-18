using Microsoft.EntityFrameworkCore;
using DB_Enlace;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<EnlaceContext>(builder.Configuration.GetConnectionString("cnEnlace"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] EnlaceContext DbContext) =>
{
    DbContext.Database.EnsureCreated();
    return Results.Ok("Felicidades base de Datos creada: " + DbContext.Database.IsInMemory());
});

app.MapGet("/api/Alumnos", async ([FromServices] EnlaceContext DbContext) =>
{
    return Results.Ok(DbContext.Alumnos.Include(p=> p.AlumnoId));
});

app.MapPost("/api/tareas", async ([FromServices] EnlaceContext DbContext, [FromBody] Alumnos alumnos) =>
{
    alumnos.AlumnoId = Guid.NewGuid();
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

app.Run();
