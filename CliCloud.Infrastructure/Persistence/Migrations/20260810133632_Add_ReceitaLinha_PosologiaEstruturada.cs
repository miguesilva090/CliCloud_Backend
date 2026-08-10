using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ReceitaLinha_PosologiaEstruturada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosologiaDuracaoUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaDuracaoValor",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaFrequenciaUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaFrequenciaValor",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaInstrucoes",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaQuantidadeUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosologiaQuantidadeValor",
                schema: "Prescricao",
                table: "ReceitaLinha",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosologiaDuracaoUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaDuracaoValor",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaFrequenciaUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaFrequenciaValor",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaInstrucoes",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaQuantidadeUnidade",
                schema: "Prescricao",
                table: "ReceitaLinha");

            migrationBuilder.DropColumn(
                name: "PosologiaQuantidadeValor",
                schema: "Prescricao",
                table: "ReceitaLinha");
        }
    }
}
