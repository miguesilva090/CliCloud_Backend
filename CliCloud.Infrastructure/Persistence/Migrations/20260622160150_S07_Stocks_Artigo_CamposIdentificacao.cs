using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S07_Stocks_Artigo_CamposIdentificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmpliacaoGarantia",
                schema: "Stocks",
                table: "Artigo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Capacidade",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                schema: "Stocks",
                table: "Artigo",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MesesGarantia",
                schema: "Stocks",
                table: "Artigo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumSerieUCentral",
                schema: "Stocks",
                table: "Artigo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TemGarantia",
                schema: "Stocks",
                table: "Artigo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TipoMedida",
                schema: "Stocks",
                table: "Artigo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisualizarNaNet",
                schema: "Stocks",
                table: "Artigo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmpliacaoGarantia",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "Capacidade",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "Desconto",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "MesesGarantia",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "NumSerieUCentral",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "TemGarantia",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "TipoMedida",
                schema: "Stocks",
                table: "Artigo");

            migrationBuilder.DropColumn(
                name: "VisualizarNaNet",
                schema: "Stocks",
                table: "Artigo");
        }
    }
}
