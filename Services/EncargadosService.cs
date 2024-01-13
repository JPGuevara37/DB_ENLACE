using DB_Enlace.models;

namespace webapi.Services
{
    public interface IEncargadosService
    {
        IEnumerable<Encargados> GetAll();
        Encargados GetById(Guid id);
        void Create(Encargados nuevoEncargado);
        void Update(Guid id, Encargados encargadoActualizado);
        void Delete(Guid id);
    }

    public class EncargadosService : IEncargadosService
    {
        private readonly EnlaceContext _dbContext;

        public EncargadosService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Encargados> GetAll()
        {
            return _dbContext.Encargados.ToList();
        }

        public Encargados GetById(Guid id)
        {
            return _dbContext.Encargados.Find(id);
        }

        public void Create(Encargados nuevoEncargado)
        {
            _dbContext.Encargados.Add(nuevoEncargado);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Encargados encargadoActualizado)
        {
            var encargado = _dbContext.Encargados.Find(id);

            if (encargado != null)
            {
                encargado.Nombre = encargadoActualizado.Nombre;
                encargado.Apellido = encargadoActualizado.Apellido;
                encargado.Direccion = encargadoActualizado.Direccion;
                encargado.Email = encargadoActualizado.Email;
                encargado.Telefono = encargadoActualizado.Telefono;

                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var encargado = _dbContext.Encargados.Find(id);

            if (encargado != null)
            {
                _dbContext.Encargados.Remove(encargado);
                _dbContext.SaveChanges();
            }
        }
    }
}