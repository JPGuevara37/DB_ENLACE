using System.Net.Mail;
using DB_Enlace.Models.Dto;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

namespace webapi.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly EnlaceContext _dbContext;

        public UsuariosService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UsuarioAdminDto> GetAll()
        {
            return _dbContext.Usuarios
                .OrderBy(u => u.Nombre)
                .Select(u => MapToDto(u))
                .ToList();
        }

        public UsuarioAdminDto GetById(Guid id)
        {
            var usuario = _dbContext.Usuarios.Find(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public void Create(UsuarioGuardarDto dto)
        {
            var usuario = new Usuarios
            {
                UsuarioId = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Usuario_Cuenta = dto.Usuario_Cuenta,
                Password = PasswordHasher.HashPassword(dto.Password),
                Email = dto.Email,
                Role = dto.Role,
                Activo = dto.Activo
            };

            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, UsuarioGuardarDto dto)
        {
            var usuario = _dbContext.Usuarios.Find(id);

            if (usuario == null)
            {
                return;
            }

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Usuario_Cuenta = dto.Usuario_Cuenta;
            usuario.Email = dto.Email;
            usuario.Role = dto.Role;
            usuario.Activo = dto.Activo;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                usuario.Password = PasswordHasher.HashPassword(dto.Password);
            }

            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var usuario = _dbContext.Usuarios.Find(id);

            if (usuario != null)
            {
                _dbContext.Usuarios.Remove(usuario);
                _dbContext.SaveChanges();
            }
        }

        public bool ExisteCuenta(string usuarioCuenta, Guid? excluirId = null)
        {
            return _dbContext.Usuarios.Any(u =>
                u.Usuario_Cuenta == usuarioCuenta &&
                (excluirId == null || u.UsuarioId != excluirId.Value));
        }

        private static UsuarioAdminDto MapToDto(Usuarios u)
        {
            return new UsuarioAdminDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Usuario_Cuenta = u.Usuario_Cuenta,
                Email = u.Email,
                Role = u.Role,
                Activo = u.Activo
            };
        }
    }

    public interface IUsuariosService
    {
        IEnumerable<UsuarioAdminDto> GetAll();
        UsuarioAdminDto GetById(Guid id);
        void Create(UsuarioGuardarDto nuevoUsuario);
        void Update(Guid id, UsuarioGuardarDto usuarioActualizado);
        void Delete(Guid id);
        bool ExisteCuenta(string usuarioCuenta, Guid? excluirId = null);
    }
}
