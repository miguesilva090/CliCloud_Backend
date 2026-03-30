using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AnamneseOrtodonticaAnaliseGeral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnamneseOrtodonticaAnaliseGeral",
                schema: "Estomatologia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SimetriaFacial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesenvolvimentoMaxila = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesenvolvimentoMandibula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerfilFacial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlturaFacialInferior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaracteristicasLabios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelacaoLabioDenteSuperior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelacaoLabioDenteInferior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreioLingual = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormaNariz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TecidosMolesIntrabucais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoFacial = table.Column<int>(type: "int", nullable: true),
                    DistanciaIntercomissuralNasal = table.Column<int>(type: "int", nullable: true),
                    DistanciaIntercomissuralPupilar = table.Column<int>(type: "int", nullable: true),
                    Adenoides = table.Column<int>(type: "int", nullable: true),
                    Amigdalas = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnamneseOrtodonticaAnaliseGeral", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnamneseOrtodonticaAnaliseGeral_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnamneseOrtodonticaAnaliseGeral_UtenteId",
                schema: "Estomatologia",
                table: "AnamneseOrtodonticaAnaliseGeral",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnamneseOrtodonticaAnaliseGeral",
                schema: "Estomatologia");
        }
    }
}
