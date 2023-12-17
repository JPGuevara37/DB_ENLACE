using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DB_Enlace.models;

public class Encargados{
        
    [Key]
    public Guid EncargadoId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Direccion { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    [JsonIgnore]
    public virtual ICollection<Alumnos> Alumnos { get; set; }
    
}