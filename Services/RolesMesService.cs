using DB_Enlace.models;

namespace webapi.Services
{
    public class RolesMesService : IRolesMesService
    {
        private readonly EnlaceContext _dbContext;

        public RolesMesService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<RolesMes> GetAll()
        {
            return _dbContext.RolesMes.ToList();
        }

        public IEnumerable<RolesMes> GetPorMes(int mes, int anno)
        {
            return _dbContext.RolesMes.Where(r => r.Mes == mes && r.Anno == anno).ToList();
        }

        public RolesMes GetById(Guid id)
        {
            return _dbContext.RolesMes.Find(id);
        }

        public void Create(RolesMes nuevo)
        {
            _dbContext.RolesMes.Add(nuevo);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, RolesMes actualizado)
        {
            var rol = _dbContext.RolesMes.Find(id);
            if (rol != null)
            {
                rol.EdadId = actualizado.EdadId;
                rol.PersonaId = actualizado.PersonaId;
                rol.Mes = actualizado.Mes;
                rol.Anno = actualizado.Anno;
                rol.Estado = actualizado.Estado;
                rol.Disponible = actualizado.Disponible;
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var rol = _dbContext.RolesMes.Find(id);
            if (rol != null)
            {
                _dbContext.RolesMes.Remove(rol);
                _dbContext.SaveChanges();
            }
        }
    }

    public interface IRolesMesService
    {
        IEnumerable<RolesMes> GetAll();
        IEnumerable<RolesMes> GetPorMes(int mes, int anno);
        RolesMes GetById(Guid id);
        void Create(RolesMes nuevo);
        void Update(Guid id, RolesMes actualizado);
        void Delete(Guid id);
    }
}
