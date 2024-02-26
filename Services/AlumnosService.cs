using System.Net.Mail;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

namespace webapi.Services
{
    public class AlumnosService : IAlumnosService
    {
        private readonly EnlaceContext _dbContext;

        public AlumnosService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Alumnos> GetAll()
        {
            return _dbContext.Alumnos.OrderBy(alumno => alumno.Nombre).ToList();
        }

        public Alumnos GetById(Guid id)
        {
            return _dbContext.Alumnos.Find(id);
        }

        public void Create(Alumnos nuevoAlumno)
        {
            _dbContext.Alumnos.Add(nuevoAlumno);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Alumnos alumnoActualizado)
        {
            var alumno = _dbContext.Alumnos.Find(id);

            if (alumno != null)
            {
                alumno.Nombre = alumnoActualizado.Nombre;
                alumno.Apellido = alumnoActualizado.Apellido;
                alumno.FechaNacimiento = alumnoActualizado.FechaNacimiento;
                alumno.Direccion = alumnoActualizado.Direccion;
                alumno.Email = alumnoActualizado.Email;
                alumno.Telefono = alumnoActualizado.Telefono;
                alumno.EncargadoId = alumnoActualizado.EncargadoId;
                alumno.EdadId = alumnoActualizado.EdadId;

                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var alumno = _dbContext.Alumnos.Find(id);

            if (alumno != null)
            {
                _dbContext.Alumnos.Remove(alumno);
                _dbContext.SaveChanges();
            }
        }
    }


    public interface IAlumnosService
    {
        IEnumerable<Alumnos> GetAll();
        Alumnos GetById(Guid id);
        void Create(Alumnos nuevoAlumno);
        void Update(Guid id, Alumnos AlumnoActualizado);
        void Delete(Guid id);
    }
}