using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOrtodonticaAnaliseDentaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnamneseOrtodonticaAnaliseDentaria",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoDentadura = table.Column<int>(type: "int", nullable: true),
                    Anomalia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelacaoMolar = table.Column<int>(type: "int", nullable: true),
                    SubDivisaoMolar = table.Column<int>(type: "int", nullable: true),
                    RelacaoCanino = table.Column<int>(type: "int", nullable: true),
                    SubDivisaoCanino = table.Column<int>(type: "int", nullable: true),
                    RelacaoMolar2 = table.Column<int>(type: "int", nullable: true),
                    LinhaMedianaSuperior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinhaMedianaInferior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinhaMedianaDente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MordidaCruzada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MordidaAberta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrespasseVertical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrespasseHorizontal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurvaSpee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaracArcoDentarioMaxila = table.Column<int>(type: "int", nullable: true),
                    CaracArcoDentarioMandibula = table.Column<int>(type: "int", nullable: true),
                    CaracPalato = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOrtodonticaAnaliseDentaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOrtodonticaAnaliseDentaria_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOrtodonticaAnaliseDentaria_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOrtodonticaAnaliseDentaria",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOrtodonticaAnaliseDentaria",
                schema: "Estomatologia");
        }
    }
}
