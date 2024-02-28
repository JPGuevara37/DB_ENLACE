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
        public async Task<bool> EnviarTokenRestablecerContrasena(string email, string resetPasswordToken)
        {
            // Implementa la lógica para enviar el token de restablecimiento de contraseña (por ejemplo, por correo electrónico).
            // Puedes utilizar librerías de envío de correos electrónicos, como SendGrid o SmtpClient.

            try
            {
                // Código para enviar el token al usuario por correo electrónico.
                // Ejemplo con SendGrid:
                var emailService = new SendGridEmailService();
                await emailService.SendResetPasswordEmail(email, resetPasswordToken);

                return true;
            }
            catch (Exception ex)
            {
                // Maneja cualquier error al enviar el correo.
                // Puedes registrar el error o devolver false para indicar que no se pudo enviar el correo.
                return false;
            }
        }

        public AuthService(IConfiguration configuration, EnlaceContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public Usuarios Autenticar(string nombreUsuario, string password)
        {
            return _dbContext.Usuarios.FirstOrDefault(u => u.Usuario_Cuenta == nombreUsuario && u.Password == password);
        }

        public string GenerarTokenRestablecerContrasena(Usuarios usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:ResetPasswordSecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Usuario_Cuenta),
            // Puedes agregar más claims según sea necesario
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ResetPasswordExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
        string GenerarTokenRestablecerContrasena(Usuarios usuario);
        Task<bool> EnviarTokenRestablecerContrasena(string email, string resetPasswordToken);
    }
}
