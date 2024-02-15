using DB_Enlace.models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class EdadController : ControllerBase
    {
        private readonly IEdadesService _edadesService;

        public EdadController(IEdadesService edadesService)
        {
            _edadesService = edadesService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_edadesService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var edad = _edadesService.GetById(id);

            if (edad == null)
            {
                return NotFound();
            }

            return Ok(edad);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Edades nuevoEdad)
        {
            _edadesService.Create(nuevoEdad);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);

            //return CreatedAtAction(nameof(GetById), new { id = nuevoEncargado.EncargadoId }, nuevoEncargado);
        } 

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Edades edadesActualizado)
        {
            _edadesService.Update(id, edadesActualizado);
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
            _edadesService.Delete(id);
              var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos eliminados con éxito" }
            };
                return Ok(response);
        }
    }
}