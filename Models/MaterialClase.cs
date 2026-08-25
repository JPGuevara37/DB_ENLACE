using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Enlace.models;

public class MaterialClase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MaterialClaseId { get; set; }

    public Guid RecursoId { get; set; }

    public string Clase { get; set; } = "";

    public int Cantidad { get; set; }
}
