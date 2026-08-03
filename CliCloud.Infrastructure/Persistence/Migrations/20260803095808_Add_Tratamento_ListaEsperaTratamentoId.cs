using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Tratamento_ListaEsperaTratamentoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "ListaEsperaTratamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_ListaEsperaTratamento_ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "ListaEsperaTratamentoId",
                principalSchema: "Tratamentos",
                principalTable: "ListaEsperaTratamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_ListaEsperaTratamento_ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropColumn(
                name: "ListaEsperaTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento");
        }
    }
}
