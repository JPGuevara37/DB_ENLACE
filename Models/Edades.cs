using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DB_Enlace.models;

public class Edades
{
    
  // [Key]
  [NotMapped]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid EdadId { get; set; }

    public string RangoEdad { get; set; }

    [JsonIgnore]
    public virtual ICollection<Alumnos> Alumnos { get; set; }

    [JsonIgnore]
    public virtual ICollection<ClasesEdades> ClasesEdades { get; set; }
  
}