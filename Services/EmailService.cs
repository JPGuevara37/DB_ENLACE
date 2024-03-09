
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using DB_Enlace.Models;

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
            try
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

                // Use Body property instead of Content
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = emailModel.Content
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (log, throw, etc.)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }

    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}

