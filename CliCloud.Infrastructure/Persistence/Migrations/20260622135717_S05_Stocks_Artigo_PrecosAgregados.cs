using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S05_Stocks_Artigo_PrecosAgregados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecoMedioFinal",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoMedioVenda",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UltimoPrecoFinal",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UltimoPrecoVenda",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecoMedioFinal",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "PrecoMedioVenda",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "UltimoPrecoFinal",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "UltimoPrecoVenda",
                schema: "Stocks",
                table: "Artigo");
        }
    }
}
