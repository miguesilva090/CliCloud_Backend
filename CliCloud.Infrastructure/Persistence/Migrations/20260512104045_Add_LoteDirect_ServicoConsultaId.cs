using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_LoteDirect_ServicoConsultaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "ServicoConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoteDirect_Servico_ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "ServicoConsultaId",
                principalSchema: "Servicos",
                principalTable: "Servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoteDirect_Servico_ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropIndex(
                name: "IX_LoteDirect_ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "ServicoConsultaId",
                schema: "Credenciais",
                table: "LoteDirect");
        }
    }
}
