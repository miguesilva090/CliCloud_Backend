using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F05_Documento_MotivoIsencao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoLinha_MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "MotivoIsencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento",
                column: "MotivoIsencaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_MotivoIsencao_MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento",
                column: "MotivoIsencaoId",
                principalSchema: "Utility",
                principalTable: "MotivoIsencao",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentoLinha_MotivoIsencao_MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha",
                column: "MotivoIsencaoId",
                principalSchema: "Utility",
                principalTable: "MotivoIsencao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_MotivoIsencao_MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentoLinha_MotivoIsencao_MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha");

            migrationBuilder.DropIndex(
                name: "IX_DocumentoLinha_MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha");

            migrationBuilder.DropIndex(
                name: "IX_Documento_MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "MotivoIsencaoId",
                schema: "Documentos",
                table: "DocumentoLinha");

            migrationBuilder.DropColumn(
                name: "MotivoIsencaoId",
                schema: "Documentos",
                table: "Documento");
        }
    }
}
