using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class Material
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MaterialId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime Fecha { get; set; }

    [JsonIgnore]
    public byte[]? Contenido { get; set; }

    public string? ContentType { get; set; }
    public long Tamano { get; set; }
}
