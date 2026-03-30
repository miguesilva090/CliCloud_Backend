using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Clinica_Rgpd_Textos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RgpdConsentimento",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RgpdDescritivo",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RgpdMarketing",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RgpdConsentimento",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "RgpdDescritivo",
                schema: "Core",
                table: "Clinica");

            migrationBuilder.DropColumn(
                name: "RgpdMarketing",
                schema: "Core",
                table: "Clinica");
        }
    }
}
