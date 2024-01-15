using System.Net.Mail;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

namespace webapi.Services
{
    public class ProfesoresService : IProfesoresService
    {
        private readonly EnlaceContext _dbContext;

        public ProfesoresService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Profesores> GetAll()
        {
            return _dbContext.Profesores.ToList();
        }

        public Profesores GetById(Guid id)
        {
            return _dbContext.Profesores.Find(id);
        }

        public void Create(Profesores nuevoProfesor)
        {
            _dbContext.Profesores.Add(nuevoProfesor);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Profesores ProfesorActualizado)
        {
            var profesor = _dbContext.Profesores.Find(id);

            if (profesor != null)
            {
                profesor.Nombre = ProfesorActualizado.Nombre;
                profesor.Apellido = ProfesorActualizado.Apellido;
                profesor.Email = ProfesorActualizado.Email;
                profesor.Telefono = ProfesorActualizado.Telefono;

                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var profesor = _dbContext.Profesores.Find(id);

            if (profesor != null)
            {
                _dbContext.Profesores.Remove(profesor);
                _dbContext.SaveChanges();
            }
        }
    }
    public interface IProfesoresService
    {
        IEnumerable<Profesores> GetAll();
        Profesores GetById(Guid id);
        void Create(Profesores nuevoProfesor);
        void Update(Guid id, Profesores profesorActualizado);
        void Delete(Guid id);
    }
}