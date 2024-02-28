using System.Net.Mail;
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

        public IEnumerable<Usuarios> GetAll()
        {
            return _dbContext.Usuarios.ToList();
        }

        public Usuarios GetById(Guid id)
        {
            return _dbContext.Usuarios.Find(id);
        }

        public void Create(Usuarios nuevoUsusario)
        {
            _dbContext.Usuarios.Add(nuevoUsusario);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Usuarios usuarioActualizado)
        {
            var usuario = _dbContext.Usuarios.Find(id);

            if (usuario != null)
            {
                usuario.Nombre = usuarioActualizado.Nombre;
                usuario.Apellido = usuarioActualizado.Apellido;
                usuario.Usuario_Cuenta = usuarioActualizado.Usuario_Cuenta;
                usuario.Password = usuarioActualizado.Password;
                usuario.Email = usuarioActualizado.Email;
                usuario.Token = usuarioActualizado.Token;
                usuario.Role = usuarioActualizado.Role;
                usuario.Activo = usuarioActualizado.Activo;

                _dbContext.SaveChanges();
            }
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

    }
    public interface IUsuariosService
    {
        IEnumerable<Usuarios> GetAll();
        Usuarios GetById(Guid id);
        void Create(Usuarios nuevoUsuario);
        void Update(Guid id, Usuarios usuarioActualizado);
        void Delete(Guid id);
    }
}