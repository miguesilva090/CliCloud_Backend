using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Admissao_Pago_Faturado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Faturado",
                schema: "Consultas",
                table: "Admissao",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pago",
                schema: "Consultas",
                table: "Admissao",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faturado",
                schema: "Consultas",
                table: "Admissao");

            migrationBuilder.DropColumn(
                name: "Pago",
                schema: "Consultas",
                table: "Admissao");
        }
    }
}
