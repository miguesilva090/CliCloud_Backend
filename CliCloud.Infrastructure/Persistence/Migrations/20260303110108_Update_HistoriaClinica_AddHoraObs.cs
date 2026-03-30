using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_HistoriaClinica_AddHoraObs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inativo",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas");

            migrationBuilder.RenameColumn(
                name: "ObsHtml",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                newName: "Obs");

            migrationBuilder.AddColumn<string>(
                name: "Hora",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hora",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas");

            migrationBuilder.RenameColumn(
                name: "Obs",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                newName: "ObsHtml");

            migrationBuilder.AddColumn<bool>(
                name: "Inativo",
                schema: "HistoriaClinica",
                table: "HistoriasClinicas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
