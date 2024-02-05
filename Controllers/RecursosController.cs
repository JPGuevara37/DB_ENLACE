using DB_Enlace.models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class RecursosController : ControllerBase
    {
        private readonly IRecursosServices _recursosServices;

        public RecursosController(IRecursosServices recursosServices)
        {
            _recursosServices = recursosServices;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_recursosServices.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var usuario = _recursosServices.GetById(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Recursos nuevoRecurso)
        {
            _recursosServices.Create(nuevoRecurso);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);

            //return CreatedAtAction(nameof(GetById), new { id = nuevoEncargado.EncargadoId }, nuevoEncargado);
        } 

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Recursos recursoActualizado)
        {
            _recursosServices.Update(id, recursoActualizado);
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
            _recursosServices.Delete(id);
              var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos eliminados con éxito" }
            };
                return Ok(response);
        }
    }
}