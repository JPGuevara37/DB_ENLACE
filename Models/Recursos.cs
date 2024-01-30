using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

public class Recursos{

    //[Key]
    public Guid RecursosId {get; set;}
    public string? Articulo {get; set;}
    public bool Activo {get; set;}
    public string? Categoria {get; set;}
    public string? Cantidad {get; set;}
}