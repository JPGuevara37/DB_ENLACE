
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using DB_Enlace.Models;
using MailKit.Security;

namespace webapi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void SendEmail(EmailModel emailModel)
        {
            if (emailModel == null)
            {
                throw new ArgumentNullException(nameof(emailModel), "EmailModel cannot be null.");
            }

            if (string.IsNullOrEmpty(emailModel.To))
            {
                throw new ArgumentException("Recipient email address cannot be null or empty.", nameof(emailModel.To));
            }

            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Ministerio Enlace", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailModel.Content
            };

            var server = _config["EmailSettings:SmtpServer"];
            var usuario = _config["EmailSettings:Username"] ?? from;
            var password = _config["EmailSettings:Password"];

            var (puerto, seguridad) = int.TryParse(_config["EmailSettings:Port"], out var puertoConfig)
                ? (puertoConfig, SecureSocketOptions.SslOnConnect)
                : (465, SecureSocketOptions.SslOnConnect);

            var opciones = new[]
            {
                (puerto, seguridad),
                (587, SecureSocketOptions.StartTls),
            };

            Exception ultimoError = new Exception("configuración SMTP no disponible");

            foreach (var opcion in opciones)
            {
                try
                {
                    using var client = new SmtpClient();
                    client.Timeout = 20000;
                    client.Connect(server, opcion.Item1, opcion.Item2);
                    client.Authenticate(usuario, password);
                    client.Send(emailMessage);
                    client.Disconnect(true);
                    return;
                }
                catch (Exception ex)
                {
                    ultimoError = ex;
                }
            }

            throw new InvalidOperationException($"No se pudo enviar el correo: {ultimoError.Message}", ultimoError);
        }
    }

    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
