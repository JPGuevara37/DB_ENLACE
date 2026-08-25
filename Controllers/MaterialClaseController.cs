using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MaterialClaseController : ControllerBase
    {
        private readonly EnlaceContext _dbContext;

        public MaterialClaseController(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dbContext.MaterialClase.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] MaterialClase item)
        {
            item.MaterialClaseId = Guid.NewGuid();
            _dbContext.MaterialClase.Add(item);
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Material asignado" } });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] MaterialClase item)
        {
            var existente = _dbContext.MaterialClase.Find(id);
            if (existente == null)
            {
                return NotFound(new ApiResponse { status = "error", result = new { mensaje = "No encontrado" } });
            }
            existente.RecursoId = item.RecursoId;
            existente.Clase = item.Clase;
            existente.Cantidad = item.Cantidad;
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación actualizada" } });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existente = _dbContext.MaterialClase.Find(id);
            if (existente == null)
            {
                return NotFound(new ApiResponse { status = "error", result = new { mensaje = "No encontrado" } });
            }
            _dbContext.MaterialClase.Remove(existente);
            _dbContext.SaveChanges();
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Asignación eliminada" } });
        }
    }
}
