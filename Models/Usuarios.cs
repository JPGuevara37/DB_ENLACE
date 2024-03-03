using System.Text.Json.Serialization;

public class Usuarios
{
    public Guid UsuarioId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Usuario_Cuenta { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool Activo { get; set; } = false;
    public DateTime Resfrescar_Token { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime ResetPasswordExpiry { get; set; }
}