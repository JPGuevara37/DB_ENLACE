using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace DB_Enlace.models;

public class Edades
{
    
  // [Key]
    public Guid EdadId { get; set; }

    public string RangoEdad { get; set; }

    [JsonIgnore]
    public virtual ICollection<Alumnos> Alumnos { get; set; }

    [JsonIgnore]
    public virtual ICollection<ClasesEdades> ClasesEdades { get; set; }
  
}