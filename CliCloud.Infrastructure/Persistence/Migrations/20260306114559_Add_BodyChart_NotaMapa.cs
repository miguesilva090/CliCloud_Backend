using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_BodyChart_NotaMapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MapaBodyChart",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaminhoImagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapaBodyChart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarcadorBodyChart",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MapaBodyChartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcadorBodyChart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarcadorBodyChart_MapaBodyChart_MapaBodyChartId",
                        column: x => x.MapaBodyChartId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "MapaBodyChart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotaBodyChart",
                schema: "ProcessoClinico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MapaBodyChartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarcadorBodyChartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaBodyChart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaBodyChart_MapaBodyChart_MapaBodyChartId",
                        column: x => x.MapaBodyChartId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "MapaBodyChart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotaBodyChart_MarcadorBodyChart_MarcadorBodyChartId",
                        column: x => x.MarcadorBodyChartId,
                        principalSchema: "ProcessoClinico",
                        principalTable: "MarcadorBodyChart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotaBodyChart_Tratamento_TratamentoId",
                        column: x => x.TratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "Tratamento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarcadorBodyChart_MapaBodyChartId",
                schema: "ProcessoClinico",
                table: "MarcadorBodyChart",
                column: "MapaBodyChartId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaBodyChart_MapaBodyChartId",
                schema: "ProcessoClinico",
                table: "NotaBodyChart",
                column: "MapaBodyChartId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaBodyChart_MarcadorBodyChartId",
                schema: "ProcessoClinico",
                table: "NotaBodyChart",
                column: "MarcadorBodyChartId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaBodyChart_TratamentoId",
                schema: "ProcessoClinico",
                table: "NotaBodyChart",
                column: "TratamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotaBodyChart",
                schema: "ProcessoClinico");

            migrationBuilder.DropTable(
                name: "MarcadorBodyChart",
                schema: "ProcessoClinico");

            migrationBuilder.DropTable(
                name: "MapaBodyChart",
                schema: "ProcessoClinico");
        }
    }
}
