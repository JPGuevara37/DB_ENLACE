using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using DB_Enlace.models;

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
            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Datos insertados con éxito" }
            };

            return Ok(response);
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