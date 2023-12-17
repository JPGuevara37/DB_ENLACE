using Microsoft.EntityFrameworkCore;
using DB_Enlace;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<EnlaceContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
