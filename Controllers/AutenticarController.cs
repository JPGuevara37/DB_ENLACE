// Archivo: UsuariosController.cs
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class AutenticarController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AutenticarController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Autenticar([FromBody] UsuarioLoginRequest request)
        {
            var usuario = _authService.Autenticar(request.Usuario, request.Password);

            if (usuario == null)
            {
                return StatusCode(403, new { status = true, result = new { error_msj = "Usuario o contrasena incorrecta" } });

            }

            var token = _authService.GenerarToken(usuario);

            return Ok(new{status = "ok",result = new { token } });

        }
    }
}

