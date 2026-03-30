using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaTipoConsultaIdAndLocaisTratamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TipoConsultaId",
                schema: "Consultas",
                table: "Consulta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocaisTratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocaisTratamento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tratamento_LocalTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "LocalTratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Consulta_TipoConsultaId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_LocaisTratamento_Codigo",
                schema: "Tratamentos",
                table: "LocaisTratamento",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consulta_TiposConsulta_TipoConsultaId",
                schema: "Consultas",
                table: "Consulta",
                column: "TipoConsultaId",
                principalSchema: "Consultas",
                principalTable: "TiposConsulta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tratamento_LocaisTratamento_LocalTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento",
                column: "LocalTratamentoId",
                principalSchema: "Tratamentos",
                principalTable: "LocaisTratamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulta_TiposConsulta_TipoConsultaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropForeignKey(
                name: "FK_Tratamento_LocaisTratamento_LocalTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropTable(
                name: "LocaisTratamento",
                schema: "Tratamentos");

            migrationBuilder.DropIndex(
                name: "IX_Tratamento_LocalTratamentoId",
                schema: "Tratamentos",
                table: "Tratamento");

            migrationBuilder.DropIndex(
                name: "IX_Consulta_TipoConsultaId",
                schema: "Consultas",
                table: "Consulta");

            migrationBuilder.DropColumn(
                name: "TipoConsultaId",
                schema: "Consultas",
                table: "Consulta");
        }
    }
}
