﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DB_Enlace.Migrations
{
    [DbContext(typeof(EnlaceContext))]
    [Migration("20240201053844_cambiosApiStatus")]
    partial class cambiosApiStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DB_Enlace.models.Alumnos", b =>
                {
                    b.Property<Guid>("AlumnoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EdadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EncargadoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlumnoId");

                    b.HasIndex("EdadId");

                    b.HasIndex("EncargadoId");

                    b.ToTable("Alumnos", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Asignaciones", b =>
                {
                    b.Property<Guid>("AsignacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AlumnoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClaseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AsignacionId");

                    b.HasIndex("AlumnoId");

                    b.HasIndex("ClaseId");

                    b.ToTable("Asignaciones", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Clases", b =>
                {
                    b.Property<Guid>("ClaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ProfesorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClaseId");

                    b.HasIndex("ProfesorId");

                    b.ToTable("Clases", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.ClasesEdades", b =>
                {
                    b.Property<Guid>("ClaseEdadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EdadId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClaseEdadId");

                    b.HasIndex("ClaseId");

                    b.HasIndex("EdadId");

                    b.ToTable("ClasesEdades", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Edades", b =>
                {
                    b.Property<Guid>("EdadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RangoEdad")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EdadId");

                    b.ToTable("Edades", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Encargados", b =>
                {
                    b.Property<Guid>("EncargadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Direccion")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Telefono")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EncargadoId");

                    b.ToTable("Encargados", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Profesores", b =>
                {
                    b.Property<Guid>("ProfesorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProfesorId");

                    b.ToTable("Profesores", (string)null);
                });

            modelBuilder.Entity("Recursos", b =>
                {
                    b.Property<Guid>("RecursosId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Activo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("Activo");

                    b.Property<string>("Articulo")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Cantidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Categoria")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("RecursosId");

                    b.ToTable("Recursos", (string)null);
                });

            modelBuilder.Entity("Usuarios", b =>
                {
                    b.Property<Guid>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Activo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false)
                        .HasColumnName("Activo");

                    b.Property<string>("NombreUsuario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Usuario")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("DB_Enlace.models.Alumnos", b =>
                {
                    b.HasOne("DB_Enlace.models.Edades", "Edades")
                        .WithMany("Alumnos")
                        .HasForeignKey("EdadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_Enlace.models.Encargados", "Encargados")
                        .WithMany("Alumnos")
                        .HasForeignKey("EncargadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Edades");

                    b.Navigation("Encargados");
                });

            modelBuilder.Entity("DB_Enlace.models.Asignaciones", b =>
                {
                    b.HasOne("DB_Enlace.models.Alumnos", "Alumnos")
                        .WithMany("Asignaciones")
                        .HasForeignKey("AlumnoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_Enlace.models.Clases", "Clases")
                        .WithMany("Asignaciones")
                        .HasForeignKey("ClaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alumnos");

                    b.Navigation("Clases");
                });

            modelBuilder.Entity("DB_Enlace.models.Clases", b =>
                {
                    b.HasOne("DB_Enlace.models.Profesores", "Profesores")
                        .WithMany("Clases")
                        .HasForeignKey("ProfesorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profesores");
                });

            modelBuilder.Entity("DB_Enlace.models.ClasesEdades", b =>
                {
                    b.HasOne("DB_Enlace.models.Clases", "Clases")
                        .WithMany("ClasesEdades")
                        .HasForeignKey("ClaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB_Enlace.models.Edades", "Edades")
                        .WithMany("ClasesEdades")
                        .HasForeignKey("EdadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clases");

                    b.Navigation("Edades");
                });

            modelBuilder.Entity("DB_Enlace.models.Alumnos", b =>
                {
                    b.Navigation("Asignaciones");
                });

            modelBuilder.Entity("DB_Enlace.models.Clases", b =>
                {
                    b.Navigation("Asignaciones");

                    b.Navigation("ClasesEdades");
                });

            modelBuilder.Entity("DB_Enlace.models.Edades", b =>
                {
                    b.Navigation("Alumnos");

                    b.Navigation("ClasesEdades");
                });

            modelBuilder.Entity("DB_Enlace.models.Encargados", b =>
                {
                    b.Navigation("Alumnos");
                });

            modelBuilder.Entity("DB_Enlace.models.Profesores", b =>
                {
                    b.Navigation("Clases");
                });
#pragma warning restore 612, 618
        }
    }
}
