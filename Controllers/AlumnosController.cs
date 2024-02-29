using DB_Enlace.models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnosService _alumnosService;

        public AlumnosController(IAlumnosService alumnosService)
        {
            _alumnosService = alumnosService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_alumnosService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var alumno = _alumnosService.GetById(id);

            if (alumno == null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Alumnos nuevoAlumno)
        {
            _alumnosService.Create(nuevoAlumno);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);

            //return CreatedAtAction(nameof(GetById), new { id = nuevoEncargado.EncargadoId }, nuevoEncargado);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Alumnos alumnosActualizado)
        {
            _alumnosService.Update(id, alumnosActualizado);
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
            _alumnosService.Delete(id);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos eliminados con éxito" }
            };
            return Ok(response);
        }
    }
}