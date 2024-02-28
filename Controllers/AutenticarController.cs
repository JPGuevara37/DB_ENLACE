// Archivo: UsuariosController.cs
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using DB_Enlace.models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class AutenticarController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly EnlaceContext _dbContext;

        public AutenticarController(IAuthService authService, EnlaceContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext; // Asigna el dbContext correctamente
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
            var resetPasswordToken = _authService.GenerarTokenRestablecerContrasena(usuario);

            return Ok(new { status = "ok", result = new { token, resetPasswordToken } });
        }

        [HttpPost("{reset-password}")]
        public async Task<IActionResult> ResetearContrasena([FromBody] ResetPasswordRequest request)
        {
            // Validar el modelo y asegurarse de que el correo electrónico existe en tu sistema.
            var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.Email == request.Email);

            if (usuario == null)
            {
                return NotFound(new { status = false, result = new { error_msj = "Correo electrónico no encontrado" } });
            }

            // Generar y enviar el token de restablecimiento de contraseña.
            var resetPasswordToken = _authService.GenerarTokenRestablecerContrasena(usuario);

            // Aquí debes proporcionar tu clave de API de SendGrid
            var sendGridApiKey = "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";

            var emailService = new SendGridEmailService(sendGridApiKey);
            var enviadoExitosamente = await emailService.SendResetPasswordEmail(request.Email, resetPasswordToken);

            if (enviadoExitosamente)
            {
                return Ok(new { status = true, result = new { mensaje = "Token de restablecimiento enviado exitosamente" } });
            }
            else
            {
                return StatusCode(500, new { status = false, result = new { error_msj = "Error al enviar el token de restablecimiento" } });
            }
        }
    }
}

