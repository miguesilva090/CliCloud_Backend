using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Consulta_RecepcaoEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmado",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Efetuado",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Faltou",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmado",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "Efetuado",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "Faltou",
                schema: "Consultas",
                table: "Consulta");
        }
    }
}
