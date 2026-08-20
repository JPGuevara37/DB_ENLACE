using System.Security.Claims;
using DB_Enlace.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public PerfilController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        private Guid ObtenerUsuarioId()
        {
            var valor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(valor, out var id) ? id : Guid.Empty;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var id = ObtenerUsuarioId();
            if (id == Guid.Empty)
            {
                return Unauthorized();
            }

            var perfil = _usuariosService.GetPerfil(id);
            if (perfil == null)
            {
                return NotFound();
            }

            return Ok(perfil);
        }

        [HttpPut]
        public IActionResult Update([FromBody] PerfilGuardarDto dto)
        {
            var id = ObtenerUsuarioId();
            if (id == Guid.Empty)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(dto.Usuario_Cuenta))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "El usuario es obligatorio" } });
            }

            if (_usuariosService.ExisteCuenta(dto.Usuario_Cuenta!, id))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Ese nombre de usuario ya existe" } });
            }

            _usuariosService.UpdatePerfil(id, dto);
            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Perfil actualizado" } });
        }

        [HttpPut("password")]
        public IActionResult CambiarPassword([FromBody] CambiarPasswordDto dto)
        {
            var id = ObtenerUsuarioId();
            if (id == Guid.Empty)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(dto.PasswordActual) || string.IsNullOrWhiteSpace(dto.PasswordNueva))
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "Las contraseñas son obligatorias" } });
            }

            var ok = _usuariosService.CambiarPassword(id, dto.PasswordActual, dto.PasswordNueva);
            if (!ok)
            {
                return BadRequest(new ApiResponse { status = "error", result = new { mensaje = "La contraseña actual es incorrecta" } });
            }

            return Ok(new ApiResponse { status = "ok", result = new { mensaje = "Contraseña actualizada" } });
        }
    }
}
