using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Enlace.models;

public class CenaSenor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CenaSenorId { get; set; }

    public int Mes { get; set; }
    public int Anno { get; set; }
    public int Dia { get; set; }
}
