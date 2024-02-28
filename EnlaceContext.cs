using Microsoft.EntityFrameworkCore;
using DB_Enlace.models;

public class EnlaceContext : DbContext
{
    public DbSet<Alumnos> Alumnos { get; set; }
    public DbSet<Encargados> Encargados { get; set; }
    public DbSet<Clases> Clases { get; set; }
    public DbSet<Edades> Edades { get; set; }
    public DbSet<ClasesEdades> ClasesEdades { get; set; }
    public DbSet<Asignaciones> Asignaciones { get; set; }
    public DbSet<Profesores> Profesores { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<Recursos> Recursos { get; set; }


    public EnlaceContext(DbContextOptions<EnlaceContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Alumnos>(alumnos =>
        {

            alumnos.ToTable("Alumnos");
            alumnos.HasKey(p => p.AlumnoId);
            //alumnos.Property(p => p.AlumnoId).HasConversion(p => BitConverter.ToInt32(p.ToByteArray(), 0), p => new Guid(BitConverter.GetBytes(p)));

            alumnos.Property(p => p.Nombre).IsRequired(false).HasMaxLength(150);

            alumnos.Property(p => p.Apellido).IsRequired(false);

            alumnos.Property(p => p.FechaNacimiento);

            alumnos.Property(p => p.Direccion).IsRequired(false).HasMaxLength(200);

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

            encargado.Property(p => p.Direccion).IsRequired(false).HasMaxLength(200);

            encargado.Property(p => p.Email);

            encargado.Property(p => p.Telefono);

            //encargado.Property(p => p.EncargadoId).HasConversion(p => BitConverter.ToInt32(p.ToByteArray(), 0), p => new Guid(BitConverter.GetBytes(p)));

            //encargado.Property(p => p.EncargadoId).HasConversion(p => p.ToString(),p => Guid.Parse(p));

        });

        modelBuilder.Entity<Clases>(clase =>
        {

            clase.ToTable("Clases");
            clase.HasKey(p => p.ClaseId);

            clase.Property(p => p.Nombre).IsRequired(false).HasMaxLength(50);

            clase.Property(p => p.Descripcion).IsRequired(false).HasMaxLength(200);

            clase.HasOne(p => p.Profesores).WithMany(p => p.Clases).HasForeignKey(p => p.ProfesorId);

        });

        modelBuilder.Entity<Edades>(edad =>
        {

            edad.ToTable("Edades");
            edad.HasKey(p => p.EdadId);

            //edad.Property(p => p.EdadId).HasConversion(p => BitConverter.ToInt32(p.ToByteArray(), 0), p => new Guid(BitConverter.GetBytes(p)));

            edad.Property(p => p.RangoEdad).IsRequired(false);


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

            asignacion.ToTable("Asignaciones");
            asignacion.HasKey(p => p.AsignacionId);

            asignacion.HasOne(p => p.Alumnos).WithMany(p => p.Asignaciones).HasForeignKey(p => p.AlumnoId);

            asignacion.HasOne(p => p.Clases).WithMany(p => p.Asignaciones).HasForeignKey(p => p.ClaseId);

        });

        modelBuilder.Entity<Profesores>(profesores =>
        {
            profesores.ToTable("Profesores");
            profesores.HasKey(p => p.ProfesorId);

            profesores.Property(p => p.Nombre);

            profesores.Property(p => p.Apellido);

            profesores.Property(p => p.Email);

            profesores.Property(p => p.Telefono);



        });

        modelBuilder.Entity<Usuarios>(usuarios =>
        {
            usuarios.ToTable("Usuarios");
            usuarios.HasKey(p => p.UsuarioId);

            usuarios.Property(p => p.Nombre);

            usuarios.Property(p => p.Apellido);

            usuarios.Property(p => p.Usuario_Cuenta);

            usuarios.Property(p => p.Password);

            usuarios.Property(p => p.Email);

            usuarios.Property(p => p.Token);

            usuarios.Property(p => p.Role);

            usuarios.Property(p => p.Activo).HasColumnName("Activo").HasColumnType("bit").HasDefaultValue(true);

        });


        modelBuilder.Entity<Recursos>(recursos =>
        {

            recursos.ToTable("Recursos");
            recursos.HasKey(p => p.RecursosId);
            //alumnos.Property(p => p.AlumnoId).HasConversion(p => BitConverter.ToInt32(p.ToByteArray(), 0), p => new Guid(BitConverter.GetBytes(p)));

            recursos.Property(p => p.Articulo).IsRequired(false).HasMaxLength(150);

            recursos.Property(p => p.Numero_Locker);

            recursos.Property(p => p.Cantidad);

            recursos.Property(p => p.Descripcion).IsRequired(false).HasMaxLength(350);

            recursos.Property(p => p.Activo).HasColumnName("Activo").HasColumnType("bit").HasDefaultValue(false);
        });
    }
}