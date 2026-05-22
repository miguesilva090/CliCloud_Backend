using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Admissao_RecepcaoFlags_TipoConsulta_CodigoLegado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoLegado",
                schema: "Consultas",
                table: "TiposConsulta",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Sinistrado",
                schema: "Consultas",
                table: "Consulta",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmaConsulta",
                schema: "Consultas",
                table: "Admissao",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmTratamento",
                schema: "Consultas",
                table: "Admissao",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoLegado",
                schema: "Consultas",
                table: "TiposConsulta");

            migrationBuilder.DropColumn(
                name: "ConfirmaConsulta",
                schema: "Consultas",
                table: "Admissao");

            migrationBuilder.DropColumn(
                name: "EmTratamento",
                schema: "Consultas",
                table: "Admissao");

            migrationBuilder.AlterColumn<int>(
                name: "Sinistrado",
                schema: "Consultas",
                table: "Consulta",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
