using SendGrid;
using SendGrid.Helpers.Mail;

public class SendGridEmailService
{
    private readonly string _apiKey;

    public SendGridEmailService()
    {
    }

    public SendGridEmailService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<bool> SendResetPasswordEmail(string email, string resetPasswordToken)
    {
        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("ministerioenlace.ibmsa@gamial.com", "Tu Aplicación");
            var to = new EmailAddress(email);
            var subject = "Restablecimiento de Contraseña";
            var plainTextContent = $"Usa este token para restablecer tu contraseña: {resetPasswordToken}";
            var htmlContent = $"<p>Usa este token para restablecer tu contraseña: {resetPasswordToken}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);

            // El correo electrónico se envió exitosamente
            return true;
        }
        catch (Exception ex)
        {
            // Maneja cualquier error al enviar el correo.
            // Puedes registrar el error o devolver false para indicar que no se pudo enviar el correo.
            return false;
        }
    }
}
