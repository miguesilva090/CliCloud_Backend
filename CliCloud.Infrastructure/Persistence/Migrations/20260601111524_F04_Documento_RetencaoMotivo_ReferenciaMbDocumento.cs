using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F04_Documento_RetencaoMotivo_ReferenciaMbDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentoId",
                schema: "Faturacao",
                table: "ReferenciaMB",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetencaoCodigoMotivo",
                schema: "Documentos",
                table: "Documento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RetencaoMotivo",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentoId",
                schema: "Faturacao",
                table: "ReferenciaMB");

            migrationBuilder.DropColumn(
                name: "RetencaoCodigoMotivo",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "RetencaoMotivo",
                schema: "Documentos",
                table: "Documento");
        }
    }
}
