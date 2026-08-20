using System;

namespace DB_Enlace.Models.Dto
{
    public class UsuarioAdminDto
    {
        public Guid UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Usuario_Cuenta { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool Activo { get; set; }
    }

    public class UsuarioGuardarDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Usuario_Cuenta { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool Activo { get; set; }
    }

    public class PerfilDto
    {
        public Guid UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Usuario_Cuenta { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Avatar { get; set; }
    }

    public class PerfilGuardarDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Usuario_Cuenta { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
    }

    public class CambiarPasswordDto
    {
        public string? PasswordActual { get; set; }
        public string? PasswordNueva { get; set; }
    }
}
