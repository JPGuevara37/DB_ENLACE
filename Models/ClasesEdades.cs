using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class ClasesEdades
{
// [Key]
    public Guid ClaseEdadId { get; set; }

    public Guid ClaseId { get; set; }

    [JsonIgnore]
    public virtual Clases Clases { get; set; }

    public Guid EdadId { get; set; }

    [JsonIgnore]
    public virtual Edades Edades { get; set; }
  
    
}