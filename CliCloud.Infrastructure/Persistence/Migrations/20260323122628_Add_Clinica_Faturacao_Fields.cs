using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Faturacao_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArmazemHabitual",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtPass",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtUser",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cab",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codinstquota",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiretoriaQRC",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailLink",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaturaRecibo",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaturacaoDocumentosImpressao",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ImprimeTicket",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha1",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha2",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha3",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha4",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha5",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linha6",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoIsencaoDefeito",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NVias",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Numdiasquota",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recibo",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Regrafaturacao",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TemSaft",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tiponotificacao",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValorMaxFaturaSimpli",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Valorquota",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmazemHabitual",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "AtPass",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "AtUser",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Cab",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Codinstquota",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "DiretoriaQRC",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailLink",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FaturaRecibo",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "FaturacaoDocumentosImpressao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ImprimeTicket",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha1",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha2",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha3",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha4",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha5",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Linha6",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "MotivoIsencaoDefeito",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "NVias",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Numdiasquota",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Recibo",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Regrafaturacao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "TemSaft",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Tiponotificacao",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ValorMaxFaturaSimpli",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Valorquota",
                schema: "Core",
                table: "Clinica");
        }
    }
}
