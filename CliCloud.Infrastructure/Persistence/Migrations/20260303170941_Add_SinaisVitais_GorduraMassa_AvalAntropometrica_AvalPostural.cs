using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SinaisVitais_GorduraMassa_AvalAntropometrica_AvalPostural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliacaoAntropometrica",
                schema: "SinaisVitais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    QuadricepEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuadricepDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuadricepDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsquiotibialEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsquiotibialDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsquiotibialDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdutorEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdutorDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdutorDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GluteoEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GluteoDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GluteoDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GemeoEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GemeoDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GemeoDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorOmbroEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorOmbroDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbdutorOmbroDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FlexorOmbroEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FlexorOmbroDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FlexorOmbroDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtensorOmbroEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtensorOmbroDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExtensorOmbroDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorInternoOmbroEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorInternoOmbroDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorInternoOmbroDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorExternoOmbroEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorExternoOmbroDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RotadorExternoOmbroDif = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoAntropometrica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacaoAntropometrica_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AvaliacaoPostural",
                schema: "SinaisVitais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndoforiaPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndoforiaValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExoforiaPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExoforiaValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HiperforiaPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HiperforiaValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesvioPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesvioValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AberturaPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AberturaValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupinadoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupinadoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PronadoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PronadoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeutroPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeutroValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EscoliosePresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EscolioseValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HipercifosePresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HipercifoseValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HiperlordosePresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HiperlordoseValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurtaPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurtaValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValgoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValgoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaroPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaroValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IliacoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IliacoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SacroPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SacroValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubidoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubidoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescidoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescidoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnteriorPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnteriorValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosteriorPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosteriorValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RotacaoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RotacaoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InclinacaoPresente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InclinacaoValores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros1Presente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros1Valores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros2Presente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros2Valores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros3Presente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outros3Valores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoPostural", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacaoPostural_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GorduraMassaMuscular",
                schema: "SinaisVitais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    PercentAguaCorpo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentGordPernaDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentGordPernaEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentGordBracoDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentGordBracoEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentGordTronco = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GorduraVisceral = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MassaMuscPernaDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MassaMuscPernaEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MassaMuscBracoDir = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MassaMuscBracoEsq = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MassaMuscTronco = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConsumoMetabolico = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorduraMassaMuscular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GorduraMassaMuscular_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoAntropometrica_UtenteId",
                schema: "SinaisVitais",
                table: "AvaliacaoAntropometrica",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoPostural_UtenteId",
                schema: "SinaisVitais",
                table: "AvaliacaoPostural",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_GorduraMassaMuscular_UtenteId",
                schema: "SinaisVitais",
                table: "GorduraMassaMuscular",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacaoAntropometrica",
                schema: "SinaisVitais");

            migrationBuilder.DropTable(
                name: "AvaliacaoPostural",
                schema: "SinaisVitais");

            migrationBuilder.DropTable(
                name: "GorduraMassaMuscular",
                schema: "SinaisVitais");
        }
    }
}
