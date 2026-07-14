using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ListaEsperaTratamento_Sync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListaEsperaTratamento",
                schema: "Tratamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrdemOrigem = table.Column<int>(type: "int", nullable: true),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrioridadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EstadoListaEsperaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocalTratamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PatologiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SinistradoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeguradoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NumSessoes = table.Column<int>(type: "int", nullable: true),
                    HoraDesejada = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NFaltMax = table.Column<int>(type: "int", nullable: true),
                    NFaltComax = table.Column<int>(type: "int", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValidadeCredencial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxaModeradora = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TecObs = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DuracaoTotal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CredencialExterna = table.Column<bool>(type: "bit", nullable: false),
                    Historico = table.Column<bool>(type: "bit", nullable: false),
                    CodigoLegado = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaEsperaTratamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_EstadosListaEspera_EstadoListaEsperaId",
                        column: x => x.EstadoListaEsperaId,
                        principalSchema: "Tratamentos",
                        principalTable: "EstadosListaEspera",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_LocaisTratamento_LocalTratamentoId",
                        column: x => x.LocalTratamentoId,
                        principalSchema: "Tratamentos",
                        principalTable: "LocaisTratamento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Patologias_PatologiaId",
                        column: x => x.PatologiaId,
                        principalSchema: "Tratamentos",
                        principalTable: "Patologias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Prioridades_PrioridadeId",
                        column: x => x.PrioridadeId,
                        principalSchema: "Tratamentos",
                        principalTable: "Prioridades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Seguradora_SeguradoraId",
                        column: x => x.SeguradoraId,
                        principalSchema: "Seguradoras",
                        principalTable: "Seguradora",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Sinistrado_SinistradoId",
                        column: x => x.SinistradoId,
                        principalSchema: "Sinistros",
                        principalTable: "Sinistrado",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaTratamento_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_DataEntrada",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "DataEntrada");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_EstadoListaEsperaId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "EstadoListaEsperaId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_Historico_Ordem",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                columns: new[] { "Historico", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_LocalTratamentoId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "LocalTratamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_MedicoId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_Ordem",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "Ordem");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_OrganismoId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_PatologiaId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "PatologiaId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_PrioridadeId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "PrioridadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_SeguradoraId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "SeguradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_SinistradoId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "SinistradoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaTratamento_UtenteId",
                schema: "Tratamentos",
                table: "ListaEsperaTratamento",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaEsperaTratamento",
                schema: "Tratamentos");
        }
    }
}
