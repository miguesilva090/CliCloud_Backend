using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Dados_Fiscais_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Atividade",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CMoeda",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cae",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Capsocial",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Datafinal",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Datainic",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DespachoUcc",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObsNotaCredito",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Portaria",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Regcom",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZonFisc",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atividade",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "CMoeda",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Cae",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Capsocial",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Datafinal",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Datainic",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "DespachoUcc",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ObsNotaCredito",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Portaria",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Regcom",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "Tipo",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "ZonFisc",
                schema: "Core",
                table: "Clinica");
        }
    }
}
