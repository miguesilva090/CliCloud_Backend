using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtenteOrganismoSeguradoraCentroSaudeMedicoExterno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CentroSaudeId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicoExternoId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganismoId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SeguradoraId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_CentroSaudeId",
                schema: "Utentes",
                table: "Utente",
                column: "CentroSaudeId");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_MedicoExternoId",
                schema: "Utentes",
                table: "Utente",
                column: "MedicoExternoId");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_OrganismoId",
                schema: "Utentes",
                table: "Utente",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_SeguradoraId",
                schema: "Utentes",
                table: "Utente",
                column: "SeguradoraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_CentroSaude_CentroSaudeId",
                schema: "Utentes",
                table: "Utente",
                column: "CentroSaudeId",
                principalSchema: "CentroSaude",
                principalTable: "CentroSaude",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_MedicoExterno_MedicoExternoId",
                schema: "Utentes",
                table: "Utente",
                column: "MedicoExternoId",
                principalSchema: "Medicos",
                principalTable: "MedicoExterno",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Organismo_OrganismoId",
                schema: "Utentes",
                table: "Utente",
                column: "OrganismoId",
                principalSchema: "Organismos",
                principalTable: "Organismo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Seguradora_SeguradoraId",
                schema: "Utentes",
                table: "Utente",
                column: "SeguradoraId",
                principalSchema: "Seguradoras",
                principalTable: "Seguradora",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_CentroSaude_CentroSaudeId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropForeignKey(
                name: "FK_Utente_MedicoExterno_MedicoExternoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Organismo_OrganismoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Seguradora_SeguradoraId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_CentroSaudeId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_MedicoExternoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_OrganismoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_SeguradoraId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "CentroSaudeId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "MedicoExternoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "OrganismoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "SeguradoraId",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
