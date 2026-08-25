using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class ContenidoPortalController : ControllerBase
    {
        private readonly EnlaceContext _dbContext;

        public ContenidoPortalController(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{seccion}")]
        public IActionResult GetPorSeccion(string seccion)
        {
            return Ok(_dbContext.ContenidoPortal
                .Where(c => c.Seccion == seccion)
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Titulo)
                .ToList());
        }

        [HttpPost]
        [Authorize(Roles = "administrador")]
        public IActionResult Create([FromBody] ContenidoPortal item)
        {
            item.ContenidoId = Guid.NewGuid();
            _dbContext.ContenidoPortal.Add(item);
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Guardado con éxito" } });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrador")]
        public IActionResult Update(Guid id, [FromBody] ContenidoPortal item)
        {
            var existente = _dbContext.ContenidoPortal.Find(id);
            if (existente == null)
            {
                return NotFound(new ApiResponse { status = "error", result = new { mensaje = "No encontrado" } });
            }
            existente.Titulo = item.Titulo;
            existente.Detalle = item.Detalle;
            existente.Icono = item.Icono;
            existente.Orden = item.Orden;
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Actualizado con éxito" } });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrador")]
        public IActionResult Delete(Guid id)
        {
            var existente = _dbContext.ContenidoPortal.Find(id);
            if (existente == null)
            {
                return NotFound(new ApiResponse { status = "error", result = new { mensaje = "No encontrado" } });
            }
            _dbContext.ContenidoPortal.Remove(existente);
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Eliminado con éxito" } });
        }
    }
}
