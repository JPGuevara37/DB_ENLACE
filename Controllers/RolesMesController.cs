using DB_Enlace.models;
using DB_Enlace.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public RolesMesController(IRolesMesService rolesMesService, EnlaceContext dbContext)
        {
            _rolesMesService = rolesMesService;
            _dbContext = dbContext;
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

            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Respuesta guardada" } });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _rolesMesService.Delete(id);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación eliminada" } });
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
