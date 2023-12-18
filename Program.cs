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

app.Run();
