using System.Net.Mail;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

namespace webapi.Services
{
    public class EdadesService : IEdadesService
    {
        private readonly EnlaceContext _dbContext;

        public EdadesService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Edades> GetAll()
        {
            return _dbContext.Edades.ToList();
        }

        public Edades GetById(Guid id)
        {
            return _dbContext.Edades.Find(id);
        }

        public void Create(Edades nuevoEdad)
        {
            _dbContext.Edades.Add(nuevoEdad);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Edades edadActualizado)
        {
            var edad = _dbContext.Edades.Find(id);

            if (edad != null)
            {
                edad.RangoEdad = edadActualizado.RangoEdad;
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var edad = _dbContext.Edades.Find(id);

            if (edad != null)
            {
                _dbContext.Edades.Remove(edad);
                _dbContext.SaveChanges();
            }
        }
    }
    

    public interface IEdadesService
    {
        IEnumerable<Edades> GetAll();
        Edades GetById(Guid id);
        void Create(Edades nuevoEdad);
        void Update(Guid id, Edades edadActualizado);
        void Delete(Guid id);
    }
}