using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapi.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using DB_Enlace.Models.Dto;
using DB_Enlace.Models;
using DB_Enlace.Helpers;
using System.Security.Cryptography;




namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class ResetEmailController : ControllerBase
    {

        private readonly EnlaceContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public ResetEmailController(
            IAuthService authService,
            EnlaceContext dbContext,
            IConfiguration configuration,
            IEmailService emailService
            )
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(p => p.Email == email);

            if (usuario is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El correo no existe"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            usuario.ResetPasswordToken = emailToken;
            usuario.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Restablecimiento de contraseña", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _dbContext.Entry(usuario).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Correo Enviado!!"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {

            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var usuario = await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(p => p.Email == resetPasswordDto.Email);
            if (usuario is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Usuario no existe"
                });
            }
            Console.WriteLine($"Usuario: {usuario}");
            var tokenCode = usuario.ResetPasswordToken;
            DateTime emailTokenExpiry = usuario.ResetPasswordExpiry;
            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Enlace de reset no existe"
                });
            }
            usuario.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _dbContext.Entry(usuario).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Contraseña restablecida"
            });


        }
    }
}