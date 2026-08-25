using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class Profesores
{
// [Key]
    public Guid ProfesorId { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Email { get; set; }

    public string Telefono { get; set; }

    public string Categoria { get; set; } = "Profesor";

    public string? Avatar { get; set; }

    [JsonIgnore]
    public virtual ICollection<Clases> Clases { get; set; }
    
}