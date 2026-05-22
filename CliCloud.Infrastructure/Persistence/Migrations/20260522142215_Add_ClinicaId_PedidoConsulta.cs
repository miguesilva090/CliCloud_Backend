using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ClinicaId_PedidoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PedidoConsulta_ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta",
                column: "ClinicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoConsulta_Clinica_ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta",
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
                name: "FK_PedidoConsulta_Clinica_ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta");

            migrationBuilder.DropIndex(
                name: "IX_PedidoConsulta_ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                schema: "Consultas",
                table: "PedidoConsulta");
        }
    }
}
