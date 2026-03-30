using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_EvolucaoTratamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvolucaoTratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacienteInformadoInicial = table.Column<bool>(type: "bit", nullable: true),
                    PacienteMotivadoInicial = table.Column<bool>(type: "bit", nullable: true),
                    PacienteColaboranteInicial = table.Column<bool>(type: "bit", nullable: true),
                    ObservacoesAvaliacaoInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvaliacaoSubjetivaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoInicioDorInicial = table.Column<int>(type: "int", nullable: true),
                    ValorDorInicial = table.Column<int>(type: "int", nullable: true),
                    TipoDorInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExameFisicoRegiaoInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatologiaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EdemaInicial = table.Column<bool>(type: "bit", nullable: true),
                    TipoEdemaInicial = table.Column<int>(type: "int", nullable: true),
                    RegiaoEdemaInicialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegiaoEdemaInicialDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservacoesEdemaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElasticidadeInicial = table.Column<bool>(type: "bit", nullable: true),
                    ObservacoesElasticidadeInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParestesiasInicial = table.Column<bool>(type: "bit", nullable: true),
                    ZonaParestesiasInicialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaParestesiasInicialDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DorIrradiadaInicial = table.Column<bool>(type: "bit", nullable: true),
                    ZonaDorIrradiadaInicialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaDorIrradiadaInicialDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CicatrizInicial = table.Column<bool>(type: "bit", nullable: true),
                    ZonaCicatrizInicialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaCicatrizInicialDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraquezaMuscularInicial = table.Column<bool>(type: "bit", nullable: true),
                    ZonaFraquezaInicialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaFraquezaInicialDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarchaAutonomaInicial = table.Column<int>(type: "int", nullable: true),
                    ObservacoesMarchaAutonomaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoniometriaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TesteMuscularInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutonomiaInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjetivosEspecificosInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessoesPropostasInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempoNecessarioInicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PacienteInformadoFinal = table.Column<bool>(type: "bit", nullable: true),
                    PacienteMotivadoFinal = table.Column<bool>(type: "bit", nullable: true),
                    PacienteColaboranteFinal = table.Column<bool>(type: "bit", nullable: true),
                    ObservacoesAvaliacaoFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvaliacaoSubjetivaFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoInicioDorFinal = table.Column<int>(type: "int", nullable: true),
                    ValorDorFinal = table.Column<int>(type: "int", nullable: true),
                    TipoDorFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExameFisicoRegiaoFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatologiaFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EdemaFinal = table.Column<bool>(type: "bit", nullable: true),
                    TipoEdemaFinal = table.Column<int>(type: "int", nullable: true),
                    RegiaoEdemaFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegiaoEdemaFinalDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservacoesEdemaFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElasticidadeFinal = table.Column<bool>(type: "bit", nullable: true),
                    ObservacoesElasticidadeFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParestesiasFinal = table.Column<bool>(type: "bit", nullable: true),
                    ZonaParestesiasFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaParestesiasFinalDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DorIrradiadaFinal = table.Column<bool>(type: "bit", nullable: true),
                    ZonaDorIrradiadaFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaDorIrradiadaFinalDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CicatrizFinal = table.Column<bool>(type: "bit", nullable: true),
                    ZonaCicatrizFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaCicatrizFinalDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraquezaMuscularFinal = table.Column<bool>(type: "bit", nullable: true),
                    ZonaFraquezaFinalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZonaFraquezaFinalDescricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarchaAutonomaFinal = table.Column<int>(type: "int", nullable: true),
                    ObservacoesMarchaAutonomaFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoniometriaFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TesteMuscularFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutonomiaFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjetivosAlcancados = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovosObjetivos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessoesPropostasFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempoNecessarioFinal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAlta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoAltaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EscalaDorAlta = table.Column<int>(type: "int", nullable: true),
                    IndicacoesParaUtenteAlta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObservacaoClinica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolucaoTratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolucaoTratamento_Tratamento_TratamentoId",
                        column: x => x.TratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "Tratamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvolucaoTratamento_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvolucaoTratamento_TratamentoId",
                schema: "Tratamentos",
                table: "EvolucaoTratamento",
                column: "TratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolucaoTratamento_UtenteId",
                schema: "Tratamentos",
                table: "EvolucaoTratamento",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvolucaoTratamento",
                schema: "Tratamentos");
        }
    }
}
