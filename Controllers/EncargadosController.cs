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
            return CreatedAtAction(nameof(GetById), new { id = nuevoEncargado.EncargadoId }, nuevoEncargado);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(Guid id, [FromBody] Encargados encargadoActualizado)
        {
            _encargadosService.Update(id, encargadoActualizado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _encargadosService.Delete(id);
            return NoContent();
        }
    }
}