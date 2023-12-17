using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DB_Enlace.models;

public class Asignaciones
{
    [Key]
    public Guid AsignacionId { get; set; }

    public Guid AlumnoId { get; set; }

    [JsonIgnore]
    public virtual Alumnos Alumno { get; set; }

    public Guid ClaseId { get; set; }

    [JsonIgnore]
    public virtual Clases Clase { get; set; }
}