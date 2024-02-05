using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB_Enlace.Migrations
{
    /// <inheritdoc />
    public partial class EliminarCategoriaAgregarNumeroLocker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Recursos");

            migrationBuilder.AddColumn<int>(
                name: "Numero_Locker",
                table: "Recursos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero_Locker",
                table: "Recursos");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Recursos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
