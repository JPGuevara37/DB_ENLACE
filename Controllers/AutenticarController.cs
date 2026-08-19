// Archivo: UsuariosController.cs
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using DB_Enlace.models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;


namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class AutenticarController : ControllerBase
    {

        private readonly EnlaceContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AutenticarController(
            IAuthService authService,
            EnlaceContext dbContext,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _dbContext = dbContext; // Asigna el dbContext correctamente
            _authService = authService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Usuarios usuarioObj)
        {
            if (usuarioObj == null)
            {
                return BadRequest();
            }

            var usuario = await _dbContext.Usuarios
                .FirstOrDefaultAsync(p => p.Usuario_Cuenta == usuarioObj.Usuario_Cuenta);

            if (usuario == null)
                return NotFound(new { Message = "Usuario incorrecto" });

            if (!PasswordHasher.VerifyPassword(usuarioObj.Password, usuario.Password))
            {
                return BadRequest(new { Message = "Contrasena esta incorrecta" });
            }

            usuario.Token = CreateJwt(usuario);

            return Ok(new
            {
                Token = usuario.Token,
                Message = "Login exitoso"
            });
        }

        [HttpPost("register")]
        public IActionResult ResgistroDeUsuario()
        {
            return Forbid();
        }

        private string CreateJwt(Usuarios usuario)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, usuario.Role),
                new Claim(ClaimTypes.Name,$"{usuario.Nombre}{usuario.Apellido}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}

