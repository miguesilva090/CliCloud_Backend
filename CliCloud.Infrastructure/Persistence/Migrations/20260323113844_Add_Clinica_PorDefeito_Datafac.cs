using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_PorDefeito_Datafac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Datafac",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PorDefeito",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Datafac",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "PorDefeito",
                schema: "Core",
                table: "Clinica");
        }
    }
}
