using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Servico_MotivoIsencaoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servico_MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico",
                column: "MotivoIsencaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servico_MotivoIsencao_MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico",
                column: "MotivoIsencaoId",
                principalSchema: "Utility",
                principalTable: "MotivoIsencao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servico_MotivoIsencao_MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropIndex(
                name: "IX_Servico_MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "MotivoIsencaoId",
                schema: "Servicos",
                table: "Servico");
        }
    }
}
