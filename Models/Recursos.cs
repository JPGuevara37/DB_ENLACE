using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

public class Recursos
{
    //[Key]
    public Guid RecursosId { get; set; }
    public string? Articulo { get; set; }
    public int Numero_Locker { get; set; }  
    public int? Cantidad { get; set; }
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }

    // Agregar la nueva propiedad Numero_Locker
}