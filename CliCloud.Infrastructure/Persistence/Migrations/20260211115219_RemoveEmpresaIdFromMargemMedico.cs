using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmpresaIdFromMargemMedico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MargemMedico_Empresa_EmpresaId",
                schema: "Medicos",
                table: "MargemMedico");

            migrationBuilder.DropIndex(
                name: "IX_MargemMedico_EmpresaId_ServicoId_MedicoId",
                schema: "Medicos",
                table: "MargemMedico");

            migrationBuilder.DropIndex(
                name: "IX_MargemMedico_EmpresaId",
                schema: "Medicos",
                table: "MargemMedico");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                schema: "Medicos",
                table: "MargemMedico");

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_ServicoId_MedicoId",
                schema: "Medicos",
                table: "MargemMedico",
                columns: new[] { "ServicoId", "MedicoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MargemMedico_ServicoId_MedicoId",
                schema: "Medicos",
                table: "MargemMedico");

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                schema: "Medicos",
                table: "MargemMedico",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_EmpresaId",
                schema: "Medicos",
                table: "MargemMedico",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_MargemMedico_EmpresaId_ServicoId_MedicoId",
                schema: "Medicos",
                table: "MargemMedico",
                columns: new[] { "EmpresaId", "ServicoId", "MedicoId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MargemMedico_Empresa_EmpresaId",
                schema: "Medicos",
                table: "MargemMedico",
                column: "EmpresaId",
                principalSchema: "Empresas",
                principalTable: "Empresa",
                principalColumn: "Id");
        }
    }
}
