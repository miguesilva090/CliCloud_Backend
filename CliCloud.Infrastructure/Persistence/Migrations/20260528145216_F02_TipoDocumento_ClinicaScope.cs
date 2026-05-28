using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F02_TipoDocumento_ClinicaScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE td
                SET td.ClinicaId = c.Id
                FROM Documentos.TipoDocumento td
                CROSS APPLY (
                    SELECT TOP 1 Id
                    FROM Core.Clinica
                    ORDER BY CreatedOn ASC
                ) c
                WHERE td.ClinicaId IS NULL;
            ");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento",
                columns: new[] { "ClinicaId", "Abreviatura" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TipoDocumento_Clinica_ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento",
                column: "ClinicaId",
                principalSchema: "Core",
                principalTable: "Clinica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TipoDocumento_Clinica_ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento",
                column: "Abreviatura",
                unique: true);
        }
    }
}
