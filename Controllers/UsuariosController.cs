using DB_Enlace.models;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_usuariosService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var usuario = _usuariosService.GetById(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Usuarios nuevoUsuario)
        {
            _usuariosService.Create(nuevoUsuario);
            return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.UsuarioId }, nuevoUsuario);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Usuarios usuarioActualizado)
        {
            _usuariosService.Update(id, usuarioActualizado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _usuariosService.Delete(id);
            return NoContent();
        }
    }
}