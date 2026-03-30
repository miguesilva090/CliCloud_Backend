using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHabilitacaoIdToEntidadePessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntidadePessoa_HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "HabilitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntidadePessoa_Habilitacao_HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa",
                column: "HabilitacaoId",
                principalSchema: "Utility",
                principalTable: "Habilitacao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntidadePessoa_Habilitacao_HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropIndex(
                name: "IX_EntidadePessoa_HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa");

            migrationBuilder.DropColumn(
                name: "HabilitacaoId",
                schema: "Utility",
                table: "EntidadePessoa");
        }
    }
}
