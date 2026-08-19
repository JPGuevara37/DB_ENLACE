using DB_Enlace.models;

namespace webapi.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly EnlaceContext _dbContext;

        public MaterialService(EnlaceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Material> GetAll()
        {
            return _dbContext.Materiales
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }

        public Material GetById(Guid id)
        {
            return _dbContext.Materiales.Find(id);
        }

        public void Create(Material nuevoMaterial)
        {
            _dbContext.Materiales.Add(nuevoMaterial);
            _dbContext.SaveChanges();
        }

        public void Update(Guid id, Material materialActualizado)
        {
            var material = _dbContext.Materiales.Find(id);

            if (material != null)
            {
                material.Nombre = materialActualizado.Nombre;
                material.Descripcion = materialActualizado.Descripcion;
                material.Categoria = materialActualizado.Categoria;
                material.Mes = materialActualizado.Mes;
                material.Anno = materialActualizado.Anno;
                material.Dia = materialActualizado.Dia;

                if (materialActualizado.Contenido != null && materialActualizado.Contenido.Length > 0)
                {
                    material.Contenido = materialActualizado.Contenido;
                    material.ContentType = materialActualizado.ContentType;
                    material.Tamano = materialActualizado.Contenido.Length;
                }

                _dbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var material = _dbContext.Materiales.Find(id);

            if (material != null)
            {
                _dbContext.Materiales.Remove(material);
                _dbContext.SaveChanges();
            }
        }
    }

    public interface IMaterialService
    {
        IEnumerable<Material> GetAll();
        Material GetById(Guid id);
        void Create(Material nuevoMaterial);
        void Update(Guid id, Material materialActualizado);
        void Delete(Guid id);
    }
}
