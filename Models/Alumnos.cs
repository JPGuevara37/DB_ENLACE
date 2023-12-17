using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DB_Enlace.models;

public class Alumnos{
    
    [Key]
    public Guid AlumnoId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? Direccion { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public Guid EncargadoId { get; set; }
    [JsonIgnore]
    public virtual required Encargados Encargado { get; set; }
    [Required]
    [MaxLength(50)]
    public Guid EdadId { get; set; }
    [JsonIgnore]
    public virtual required Edades Edad { get; set; }
    [JsonIgnore]
    public virtual required ICollection<Asignaciones> Asignaciones { get; set; }

}