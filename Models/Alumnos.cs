using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DB_Enlace.models;

public class Alumnos{
    
   // [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AlumnoId { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    [Column(TypeName = "date")]
    public DateTime FechaNacimiento { get; set; }
    public string? Direccion { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public Guid EncargadoId { get; set; }
    [JsonIgnore]
    public virtual Encargados Encargados { get; set; }
    [Required]
    public Guid EdadId { get; set; }
    [JsonIgnore]
    public virtual Edades Edades { get; set; }
    [JsonIgnore]
    public virtual ICollection<Asignaciones>? Asignaciones { get; set; }

}