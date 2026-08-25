using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_Enlace.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCategoriaRecursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Recursos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Profesores",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Canton",
                table: "Encargados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Distrito",
                table: "Encargados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Encargados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Mes = table.Column<int>(type: "int", nullable: true),
                    Anno = table.Column<int>(type: "int", nullable: true),
                    Dia = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contenido = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tamano = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.MaterialId);
                });

            migrationBuilder.CreateTable(
                name: "RolesMes",
                columns: table => new
                {
                    RolMesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EdadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    Anno = table.Column<int>(type: "int", nullable: false),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesMes", x => x.RolMesId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materiales");

            migrationBuilder.DropTable(
                name: "RolesMes");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Profesores");

            migrationBuilder.DropColumn(
                name: "Canton",
                table: "Encargados");

            migrationBuilder.DropColumn(
                name: "Distrito",
                table: "Encargados");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Encargados");
        }
    }
}
