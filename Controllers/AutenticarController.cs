// Archivo: UsuariosController.cs
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using DB_Enlace.models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
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

        public AutenticarController(
            IAuthService authService,
            EnlaceContext dbContext,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _dbContext = dbContext; // Asigna el dbContext correctamente
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
        public async Task<IActionResult> ResgistroDeUsuario([FromBody] Usuarios usuarioObj)
        {
            if (usuarioObj == null)
                return BadRequest();

            //check Usuario_Cuenta
            if (await CheckUsuarioCuentaExisteAsync(usuarioObj.Usuario_Cuenta))
                return BadRequest(new { Message = "El usuario ya existe, por favor elige otro nombre de usuario" });

            //Check correo
            if (await CheckEmailExisteAsync(usuarioObj.Email))
                return BadRequest(new { Message = "El correo ya existe, por favor elige otro correo" });

            //Check password
            var passMessage = CheckPasswordStrength(usuarioObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

            usuarioObj.Password = PasswordHasher.HashPassword(usuarioObj.Password);
            usuarioObj.Role = "User";
            usuarioObj.Token = "";
            await _dbContext.Usuarios.AddAsync(usuarioObj);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Registro exitoso"
            });
        }

        private Task<bool> CheckUsuarioCuentaExisteAsync(string? email)
            => _dbContext.Usuarios.AnyAsync(p => p.Email == email);

        private Task<bool> CheckEmailExisteAsync(string? username)
            => _dbContext.Usuarios.AnyAsync(p => p.Usuario_Cuenta == username);

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 8)
                sb.Append("la contrasena tiene que tener mínimo 8 caracteres" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]")
                && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("La contrasena tiene que tener caracteres alfanuméricos y mayúsculas(a-z, A-Z, 0-9)" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("La contrasena tiene que tener caracteres especiales" + Environment.NewLine);
            return sb.ToString();
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

