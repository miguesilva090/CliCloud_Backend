using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SessaoTratmento_MotivoDesmarcacaoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessaoTratamento_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "MotivosDesmarcacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento",
                column: "MotivosDesmarcacaoId",
                principalSchema: "Tratamentos",
                principalTable: "MotivosDesmarcacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessaoTratamento_MotivosDesmarcacao_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropIndex(
                name: "IX_SessaoTratamento_MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");

            migrationBuilder.DropColumn(
                name: "MotivosDesmarcacaoId",
                schema: "Tratamentos",
                table: "SessaoTratamento");
        }
    }
}
