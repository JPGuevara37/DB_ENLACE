using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DB_Enlace.models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace webapi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly EnlaceContext _dbContext;

        public AuthService(IConfiguration configuration, EnlaceContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public Usuarios Autenticar(string nombreUsuario, string password)
        {
            return _dbContext.Usuarios.FirstOrDefault(u => u.Usuario_Cuenta == nombreUsuario && u.Password == password);
        }


        public string GenerarToken(Usuarios usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),  // Puedes agregar más claims según necesites
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Usuario_Cuenta),
            // ... otros claims
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public interface IAuthService
    {
        Usuarios Autenticar(string nombreUsuario, string password);
        string GenerarToken(Usuarios usuario);
    }
}
