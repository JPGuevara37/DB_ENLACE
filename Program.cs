using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DB_Enlace.models;
using webapi.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors; // Para EnableCorsAttribute
using Microsoft.AspNetCore.Mvc; // Si también estás utilizando ControllerBase u otros componentes de MVC


var builder = WebApplication.CreateBuilder(args);

//TOKEN

// Obtén la clave secreta de Jwt desde appsettings.json
/*var secretKey = builder.Configuration.GetSection("Jwt:SecretKey").Value;

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
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
}
else
{
    // Manejo de error o mensaje de advertencia
}
*/
//permitir conexion a:
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });

    // Agregar una segunda política CORS si es necesario
    options.AddPolicy("PoliticaProduccion", builder =>
    {
        builder.WithOrigins("https://jolly-wave-0788d9610.4.azurestaticapps.net")
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