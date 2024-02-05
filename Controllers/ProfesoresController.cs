using DB_Enlace.models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class ProfesoresController : ControllerBase
    {
        private readonly IProfesoresService _profesoresService;

        public ProfesoresController(IProfesoresService profesoresService)
        {
            _profesoresService = profesoresService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_profesoresService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var encargado = _profesoresService.GetById(id);

            if (encargado == null)
            {
                return NotFound();
            }

            return Ok(encargado);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Profesores nuevoProfesor)
        {
            _profesoresService.Create(nuevoProfesor);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Profesores profesorActualizado)
        {
            _profesoresService.Update(id, profesorActualizado);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos modificados con éxito" }
            };
                return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _profesoresService.Delete(id);
              var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos eliminados con éxito" }
            };
                return Ok(response);
        }
    }
}