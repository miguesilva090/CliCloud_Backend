using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tecnico_TipoTecnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoTecnico",
                schema: "Tecnicos",
                table: "Tecnico",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Tecnico_TipoTecnico",
                schema: "Tecnicos",
                table: "Tecnico",
                column: "TipoTecnico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tecnico_TipoTecnico",
                schema: "Tecnicos",
                table: "Tecnico");

            migrationBuilder.DropColumn(
                name: "TipoTecnico",
                schema: "Tecnicos",
                table: "Tecnico");
        }
    }
}
