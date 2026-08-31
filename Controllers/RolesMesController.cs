using DB_Enlace.models;
using DB_Enlace.Models;
using DB_Enlace.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Claims;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RolesMesController : ControllerBase
    {
        private readonly IRolesMesService _rolesMesService;
        private readonly EnlaceContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RolesMesController(
            IRolesMesService rolesMesService,
            EnlaceContext dbContext,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _rolesMesService = rolesMesService;
            _dbContext = dbContext;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_rolesMesService.GetAll());
        }

        [HttpGet("{mes}/{anno}")]
        public IActionResult GetPorMes(int mes, int anno)
        {
            return Ok(_rolesMesService.GetPorMes(mes, anno));
        }

        [HttpGet("mias")]
        public async Task<IActionResult> GetMias()
        {
            var profesor = await ObtenerProfesorActualAsync();
            if (profesor == null)
            {
                return Ok(new List<RolesMes>());
            }

            var asignaciones = await _dbContext.RolesMes
                .Where(r => r.PersonaId == profesor.ProfesorId)
                .OrderByDescending(r => r.Anno)
                .ThenByDescending(r => r.Mes)
                .ThenByDescending(r => r.Dia)
                .ToListAsync();

            return Ok(asignaciones);
        }

        [HttpPost]
        public IActionResult Create([FromBody] RolesMes nuevo)
        {
            _rolesMesService.Create(nuevo);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación guardada" } });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] RolesMes actualizado)
        {
            _rolesMesService.Update(id, actualizado);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación actualizada" } });
        }

        [HttpPut("{id}/respuesta")]
        public async Task<IActionResult> SetRespuesta(Guid id, [FromBody] RespuestaRolDto dto)
        {
            var rol = await _dbContext.RolesMes.FindAsync(id);
            if (rol == null)
            {
                return NotFound(new ApiResponse { status = "error", result = new { mensaje = "Asignación no encontrada" } });
            }

            var esAdmin = User.IsInRole("administrador") || User.IsInRole("lidere");
            if (!esAdmin)
            {
                var profesor = await ObtenerProfesorActualAsync();
                if (profesor == null || rol.PersonaId != profesor.ProfesorId)
                {
                    return Forbid();
                }
            }

            if (dto.Respuesta != null && dto.Respuesta != "Aceptada" && dto.Respuesta != "Rechazada" && dto.Respuesta != "")
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Respuesta inválida" } });
            }

            rol.Respuesta = string.IsNullOrEmpty(dto.Respuesta) ? null : dto.Respuesta;
            await _dbContext.SaveChangesAsync();

            if (rol.Respuesta == "Aceptada")
            {
                EnviarNotificacionAceptacion(rol);
            }

            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Respuesta guardada" } });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _rolesMesService.Delete(id);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación eliminada" } });
        }

        private void EnviarNotificacionAceptacion(RolesMes rol)
        {
            try
            {
                var destinatario = _configuration["EmailSettings:NotificacionAsignacionTo"];
                if (string.IsNullOrWhiteSpace(destinatario))
                {
                    return;
                }

                var profesor = _dbContext.Profesores.FirstOrDefault(p => p.ProfesorId == rol.PersonaId);
                var nombreProfesor = profesor != null ? $"{profesor.Nombre} {profesor.Apellido}".Trim() : "Un profesor";

                string clase;
                if (rol.Tipo == "CenaSenor")
                {
                    clase = "Cena del Señor";
                }
                else
                {
                    var edad = rol.EdadId.HasValue
                        ? _dbContext.Edades.FirstOrDefault(e => e.EdadId == rol.EdadId.Value)
                        : null;
                    clase = edad?.RangoEdad ?? "Clase";
                }

                var fecha = new DateTime(rol.Anno, rol.Mes, rol.Dia)
                    .ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

                var contenido = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head><meta charset='UTF-8'></head>
                    <body style='font-family:Arial,sans-serif;padding:20px;background:#f5f5f5;'>
                        <div style='max-width:600px;margin:0 auto;background:#fff;padding:24px;border-radius:10px;'>
                            <h2 style='color:#005a65;margin-top:0;'>Asignación confirmada</h2>
                            <p><strong>{nombreProfesor}</strong> confirmó su asignación.</p>
                            <table style='width:100%;border-collapse:collapse;'>
                                <tr><td style='padding:8px;color:#666;'>Clase</td><td style='padding:8px;font-weight:bold;'>{clase}</td></tr>
                                <tr><td style='padding:8px;color:#666;'>Fecha</td><td style='padding:8px;font-weight:bold;'>{fecha}</td></tr>
                            </table>
                            <p style='color:#8592a6;font-size:12px;'>Ministerio infantil Enlace.</p>
                        </div>
                    </body>
                    </html>";

                var emailModel = new EmailModel(destinatario, $"[Enlace] {nombreProfesor} confirmó su asignación", contenido);
                _emailService.SendEmail(emailModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo enviar la notificación de aceptación: {ex.Message}");
            }
        }

        private async Task<Profesores?> ObtenerProfesorActualAsync()
        {
            var usuarioIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioIdValue) || !Guid.TryParse(usuarioIdValue, out var usuarioId))
            {
                return null;
            }

            var usuario = await _dbContext.Usuarios.FindAsync(usuarioId);
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
            {
                return null;
            }

            return await _dbContext.Profesores
                .FirstOrDefaultAsync(p => p.Email != null && p.Email.Trim() == usuario.Email.Trim());
        }
    }
}
