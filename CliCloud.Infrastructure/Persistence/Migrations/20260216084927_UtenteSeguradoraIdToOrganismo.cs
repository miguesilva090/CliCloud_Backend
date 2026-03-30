using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UtenteSeguradoraIdToOrganismo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Seguradora_SeguradoraId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Organismo_SeguradoraId",
                schema: "Utentes",
                table: "Utente",
                column: "SeguradoraId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Organismo_SeguradoraId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Seguradora_SeguradoraId",
                schema: "Utentes",
                table: "Utente",
                column: "SeguradoraId",
                principalSchema: "Seguradoras",
                principalTable: "Seguradora",
                principalColumn: "Id");
        }
    }
}
