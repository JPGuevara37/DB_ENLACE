using DB_Enlace.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "administrador")]
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
        public IActionResult Create([FromBody] UsuarioGuardarDto nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Usuario_Cuenta))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "El usuario es obligatorio" } });
            }

            if (string.IsNullOrWhiteSpace(nuevoUsuario.Password))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "La contraseña es obligatoria" } });
            }

            if (_usuariosService.ExisteCuenta(nuevoUsuario.Usuario_Cuenta!))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Ese nombre de usuario ya existe" } });
            }

            _usuariosService.Create(nuevoUsuario);

            var response = new ApiResponse
            {
                status = "ok",
                result = new { mensaje = "Usuario creado con éxito" }
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UsuarioGuardarDto usuarioActualizado)
        {
            if (_usuariosService.GetById(id) == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(usuarioActualizado.Usuario_Cuenta))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "El usuario es obligatorio" } });
            }

            if (_usuariosService.ExisteCuenta(usuarioActualizado.Usuario_Cuenta!, id))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Ese nombre de usuario ya existe" } });
            }

            _usuariosService.Update(id, usuarioActualizado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (_usuariosService.GetById(id) == null)
            {
                return NotFound();
            }

            _usuariosService.Delete(id);
            return NoContent();
        }
    }
}
