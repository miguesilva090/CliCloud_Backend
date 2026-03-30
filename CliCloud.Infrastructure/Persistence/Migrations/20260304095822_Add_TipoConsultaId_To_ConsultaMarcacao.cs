using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TipoConsultaId_To_ConsultaMarcacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaMarcacao_TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "TipoConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultaMarcacao_TiposConsulta_TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao",
                column: "TipoConsultaId",
                principalSchema: "Consultas",
                principalTable: "TiposConsulta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultaMarcacao_TiposConsulta_TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao");

            migrationBuilder.DropIndex(
                name: "IX_ConsultaMarcacao_TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao");

            migrationBuilder.DropColumn(
                name: "TipoConsultaId",
                schema: "Consultas",
                table: "ConsultaMarcacao");
        }
    }
}
