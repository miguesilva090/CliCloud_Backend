using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtenteEmpresaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utente_EmpresaId",
                schema: "Utentes",
                table: "Utente",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_Empresa_EmpresaId",
                schema: "Utentes",
                table: "Utente",
                column: "EmpresaId",
                principalSchema: "Empresas",
                principalTable: "Empresa",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_Empresa_EmpresaId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_EmpresaId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
