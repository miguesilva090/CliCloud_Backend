using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtenteProvenienciaUtenteId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente",
                column: "ProvenienciaUtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_ProvenienciaUtente_ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente",
                column: "ProvenienciaUtenteId",
                principalSchema: "Utility",
                principalTable: "ProvenienciaUtente",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_ProvenienciaUtente_ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "ProvenienciaUtenteId",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
