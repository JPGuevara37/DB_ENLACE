using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class Clases
{
            
    [Key]
    public Guid ClaseId { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public Guid ProfesorId { get; set; }
    [JsonIgnore]
    public virtual Profesores Profesor { get; set; }
    [JsonIgnore]
    public virtual ICollection<Asignaciones> Asignaciones { get; set; }
    [JsonIgnore]
    public virtual ICollection<ClasesEdades> ClasesEdades { get; set; }
    
}