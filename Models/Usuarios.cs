public class Usuarios
{
    public Guid UsuarioId{get; set;}
    public string? NombreUsuario {get; set;}
    public string? Usuario {get; set;}
    public string? Password {get; set;}
    public bool Activo {get; set;} = false;
}