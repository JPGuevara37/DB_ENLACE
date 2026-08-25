using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_Enlace.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMaterialClase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialClase",
                columns: table => new
                {
                    MaterialClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecursoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Clase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialClase", x => x.MaterialClaseId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialClase");
        }
    }
}
