using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tratamento_UnidadeTempoTecnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnidadeTempoAux",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadeTempoFisio",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnidadeTempoOutro",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnidadeTempoAux",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropColumn(
                name: "UnidadeTempoFisio",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropColumn(
                name: "UnidadeTempoOutro",
                schema: "Tratamentos",
                table: "Tratamento");
        }
    }
}
