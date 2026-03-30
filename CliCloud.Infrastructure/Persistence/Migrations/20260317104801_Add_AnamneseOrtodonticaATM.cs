using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOrtodonticaATM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnamneseOrtodonticaATM",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Palpacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelacaoCentrica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateralidadeEsquerda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateralidadeDireita = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Protrusao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculosMastigatorios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculosInfra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusculosSupra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApertaOuRangeDentes = table.Column<bool>(type: "bit", nullable: true),
                    MusculosMandibulaDoridosAoAcordar = table.Column<bool>(type: "bit", nullable: true),
                    DorMandibulaOuvido = table.Column<bool>(type: "bit", nullable: true),
                    NaoPodeAbrirFecharBoca = table.Column<bool>(type: "bit", nullable: true),
                    DentesSensiveisDesgastados = table.Column<bool>(type: "bit", nullable: true),
                    SofreuAlgumTraumatismo = table.Column<bool>(type: "bit", nullable: true),
                    SenteBarulhoZumbido = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOrtodonticaATM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOrtodonticaATM_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOrtodonticaATM_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOrtodonticaATM",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOrtodonticaATM",
                schema: "Estomatologia");
        }
    }
}
