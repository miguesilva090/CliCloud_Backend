using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Identificacao_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCPostal",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Filial",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Idnum",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndicativoTelefone",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidade",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Morada",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIB",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sucursal",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telemovel",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Web",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCPostal",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Fax",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Filial",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Idnum",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "IndicativoTelefone",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Localidade",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Morada",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "NIB",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Sucursal",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Telefone",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Telemovel",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Web",
                schema: "Core",
                table: "Clinica");
        }
    }
}
