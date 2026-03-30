using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEstadoCivilAndGrupoSanguineoEnumsWithFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrupoSanguineo",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "EstadoCivil",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.AddColumn<Guid>(
                name: "GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente",
                column: "GrupoSanguineoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadePessoa_EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "EstadoCivilId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntidadePessoa_EstadoCivil_EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "EstadoCivilId",
                principalSchema: "Utility",
                principalTable: "EstadoCivil",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_GrupoSanguineo_GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente",
                column: "GrupoSanguineoId",
                principalSchema: "Utility",
                principalTable: "GrupoSanguineo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntidadePessoa_EstadoCivil_EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropForeignKey(
                name: "FK_Utente_GrupoSanguineo_GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_EntidadePessoa_EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropColumn(
                name: "GrupoSanguineoId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "EstadoCivilId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.AddColumn<int>(
                name: "GrupoSanguineo",
                schema: "Utentes",
                table: "Utente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoCivil",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "int",
                nullable: true);
        }
    }
}
