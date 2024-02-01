using DB_Enlace.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class EncargadosController : ControllerBase
    {
        private readonly IEncargadosService _encargadosService;

        public EncargadosController(IEncargadosService encargadosService)
        {
            _encargadosService = encargadosService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_encargadosService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var encargado = _encargadosService.GetById(id);

            if (encargado == null)
            {
                return NotFound();
            }

            return Ok(encargado);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Encargados nuevoEncargado)
        {
            _encargadosService.Create(nuevoEncargado);
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);

            //return CreatedAtAction(nameof(GetById), new { id = nuevoEncargado.EncargadoId }, nuevoEncargado);
        } 

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Encargados encargadoActualizado)
        {
            _encargadosService.Update(id, encargadoActualizado);
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
            _encargadosService.Delete(id);
              var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos eliminados con éxito" }
            };
                return Ok(response);
        }
    }
}