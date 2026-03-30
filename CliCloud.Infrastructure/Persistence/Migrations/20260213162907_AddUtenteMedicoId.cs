using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtenteMedicoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MedicoId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_MedicoId",
                schema: "Utentes",
                table: "Utente",
                column: "MedicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Medico_MedicoId",
                schema: "Utentes",
                table: "Utente",
                column: "MedicoId",
                principalSchema: "Medicos",
                principalTable: "Medico",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Medico_MedicoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_MedicoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
