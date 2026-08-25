using System.ComponentModel.DataAnnotations.Schema;

namespace DB_Enlace.models;

public class ContenidoPortal
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ContenidoId { get; set; }

    public string Seccion { get; set; } = "meta";

    public string? Titulo { get; set; }

    public string? Detalle { get; set; }

    public string? Icono { get; set; }

    public int Orden { get; set; }
}
