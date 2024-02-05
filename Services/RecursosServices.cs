using System.Net.Mail;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

namespace webapi.Services
{
    public class RecursosServices : IRecursosServices
    {
        private readonly EnlaceContext _dbContext;

        public RecursosServices (EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Recursos> GetAll()
        {
            return _dbContext.Recursos.ToList();
        }

        public Recursos GetById(Guid id)
        {
            return _dbContext.Recursos.Find(id);
        }

        public void Create(Recursos nuevoRecurso)
        {
            _dbContext.Recursos.Add(nuevoRecurso);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Recursos RecursoActualizado)
        {
            var recurso = _dbContext.Recursos.Find(id);

            if (recurso != null)
            {
                recurso.Articulo = RecursoActualizado.Articulo;
                recurso.Numero_Locker = RecursoActualizado.Numero_Locker;
                recurso.Cantidad = RecursoActualizado.Cantidad;
                recurso.Descripcion = RecursoActualizado.Descripcion;
                recurso.Activo = RecursoActualizado.Activo;

                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var recurso = _dbContext.Recursos.Find(id);

            if (recurso != null)
            {
                _dbContext.Recursos.Remove(recurso);
                _dbContext.SaveChanges();
            }
        }
    }
    public interface IRecursosServices
    {
        IEnumerable<Recursos> GetAll();
        Recursos GetById(Guid id);
        void Create(Recursos nuevoRecurso);
        void Update(Guid id, Recursos RecursoActualizado);
        void Delete(Guid id);
    }
}