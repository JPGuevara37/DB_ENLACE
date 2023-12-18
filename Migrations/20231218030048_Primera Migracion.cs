using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_Enlace.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Edades",
                columns: table => new
                {
                    EdadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RangoEdad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edades", x => x.EdadId);
                });

            migrationBuilder.CreateTable(
                name: "Encargados",
                columns: table => new
                {
                    EncargadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encargados", x => x.EncargadoId);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    ProfesorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.ProfesorId);
                });

            migrationBuilder.CreateTable(
                name: "Alumnos",
                columns: table => new
                {
                    AlumnoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncargadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdadId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.AlumnoId);
                    table.ForeignKey(
                        name: "FK_Alumnos_Edades_EdadId",
                        column: x => x.EdadId,
                        principalTable: "Edades",
                        principalColumn: "EdadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnos_Encargados_EncargadoId",
                        column: x => x.EncargadoId,
                        principalTable: "Encargados",
                        principalColumn: "EncargadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfesorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.ClaseId);
                    table.ForeignKey(
                        name: "FK_Clases_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "ProfesorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asinaciones",
                columns: table => new
                {
                    AsignacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlumnoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asinaciones", x => x.AsignacionId);
                    table.ForeignKey(
                        name: "FK_Asinaciones_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "AlumnoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asinaciones_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "ClaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClasesEdades",
                columns: table => new
                {
                    ClaseEdadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasesEdades", x => x.ClaseEdadId);
                    table.ForeignKey(
                        name: "FK_ClasesEdades_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "ClaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClasesEdades_Edades_EdadId",
                        column: x => x.EdadId,
                        principalTable: "Edades",
                        principalColumn: "EdadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_EdadId",
                table: "Alumnos",
                column: "EdadId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_EncargadoId",
                table: "Alumnos",
                column: "EncargadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asinaciones_AlumnoId",
                table: "Asinaciones",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asinaciones_ClaseId",
                table: "Asinaciones",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Clases_ProfesorId",
                table: "Clases",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClasesEdades_ClaseId",
                table: "ClasesEdades",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClasesEdades_EdadId",
                table: "ClasesEdades",
                column: "EdadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asinaciones");

            migrationBuilder.DropTable(
                name: "ClasesEdades");

            migrationBuilder.DropTable(
                name: "Alumnos");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "Edades");

            migrationBuilder.DropTable(
                name: "Encargados");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}
