using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TipoCampo_NumeroLinhas_to_FichaClinicaSecaoCampo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroLinhas",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoCampo",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "TipoCampo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoCampo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroLinhas",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoCampo");

            migrationBuilder.DropColumn(
                name: "TipoCampo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoCampo");
        }
    }
}
