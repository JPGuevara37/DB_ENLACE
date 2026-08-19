using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class RolesMes
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RolMesId { get; set; }

    public Guid EdadId { get; set; }
    public Guid PersonaId { get; set; }

    public int Mes { get; set; }
    public int Anno { get; set; }

    public string Estado { get; set; } = "Propuesta";

    public bool Disponible { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
