using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_LoteDirect_TipoServicoRegisto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoteDirect_TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "TipoServicoRegistoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoteDirect_TipoServico_TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect",
                column: "TipoServicoRegistoId",
                principalSchema: "Servicos",
                principalTable: "TipoServico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoteDirect_TipoServico_TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropIndex(
                name: "IX_LoteDirect_TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect");

            migrationBuilder.DropColumn(
                name: "TipoServicoRegistoId",
                schema: "Credenciais",
                table: "LoteDirect");
        }
    }
}
