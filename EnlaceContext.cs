using Microsoft.EntityFrameworkCore;
using DB_Enlace.models;

public class EnlaceContext : DbContext
{
    public EnlaceContext(DbContextOptions<EnlaceContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Alumnos>(alumnos =>
        {

        alumnos.ToTable("Alumnos");
        alumnos.HasKey(p => p.AlumnoId);

        alumnos.Property(p => p.Nombre).IsRequired(false).HasMaxLength(150);

        alumnos.Property(p => p.Apellido).IsRequired(false);

        alumnos.Property(p => p.FechaNacimiento);

        alumnos.Property(p => p.Direccion);

        alumnos.Property(p => p.Email);

        alumnos.Property(p => p.Telefono);

        alumnos.HasOne(p => p.Encargados).WithMany(p => p.Alumnos).HasForeignKey(p => p.EncargadoId);

        alumnos.HasOne(p => p.Edades).WithMany(p => p.Alumnos).HasForeignKey(p => p.EdadId);

        });

        modelBuilder.Entity<Encargados>(encargado =>
        {

        encargado.ToTable("Encargados");
        encargado.HasKey(p => p.EncargadoId);

        encargado.Property(p => p.Nombre).IsRequired(false).HasMaxLength(50);

        encargado.Property(p => p.Apellido).IsRequired(false).HasMaxLength(50);

        encargado.Property(p => p.Direccion).IsRequired(false).HasMaxLength(50);
        
        encargado.Property(p => p.Email);

        encargado.Ignore(p => p.Telefono);

        });

        modelBuilder.Entity<Clases>(clase =>
        {

        clase.ToTable("Clases") ;
        clase.HasKey(p => p.ClaseId);

        clase.Property(p => p.Nombre).IsRequired(false).HasMaxLength(50);

        clase.Property(p => p.Descripcion).IsRequired(false).HasMaxLength(50);

        clase.HasOne(p => p.Profesores).WithMany(p => p.Clases).HasForeignKey(p => p.ProfesorId);

        });

        modelBuilder.Entity<Edades>(edad =>
        {

        edad.ToTable("Edades") ;
        edad.HasKey(p => p.EdadId);

        edad.Property(p => p.RangoEdad ).IsRequired(false);


        });

        modelBuilder.Entity<ClasesEdades>(claseEdad =>
        {
            claseEdad.ToTable("ClasesEdades");
            claseEdad.HasKey(p => p.ClaseEdadId);

            claseEdad.HasOne(p => p.Clases).WithMany(p => p.ClasesEdades).HasForeignKey(p => p.ClaseId);

            claseEdad.HasOne(p => p.Edades).WithMany(p => p.ClasesEdades).HasForeignKey(p => p.EdadId);

        });

        modelBuilder.Entity<Asignaciones>(asignacion =>
        {

            asignacion.ToTable("Asinaciones");
            asignacion.HasKey(p => p.AsignacionId);

            asignacion.HasOne(p => p.Alumnos).WithMany(p => p.Asignaciones).HasForeignKey(p => p.AlumnoId);

            asignacion.HasOne(p => p.Clases).WithMany(p => p.Asignaciones).HasForeignKey(p => p.ClaseId);

        });
    }
}