using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Email_Templates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAssuntoConsultas",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAssuntoExames",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAssuntoRelatorios",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAssuntoTratamentos",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConteudoConsultas",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConteudoExames",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConteudoRelatorios",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConteudoTratamentos",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAssuntoConsultas",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailAssuntoExames",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailAssuntoRelatorios",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailAssuntoTratamentos",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailConteudoConsultas",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailConteudoExames",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailConteudoRelatorios",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "EmailConteudoTratamentos",
                schema: "Core",
                table: "Clinica");
        }
    }
}
