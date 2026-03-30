using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdEstadoFromEstadosListaEspera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EstadosListaEspera_IdEstado",
                schema: "Tratamentos",
                table: "EstadosListaEspera");

            migrationBuilder.DropColumn(
                name: "IdEstado",
                schema: "Tratamentos",
                table: "EstadosListaEspera");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEstado",
                schema: "Tratamentos",
                table: "EstadosListaEspera",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EstadosListaEspera_IdEstado",
                schema: "Tratamentos",
                table: "EstadosListaEspera",
                column: "IdEstado",
                unique: true);
        }
    }
}
