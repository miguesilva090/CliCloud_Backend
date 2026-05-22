using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ListaEsperaConsulta_Sync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListaEsperaConsulta",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganismoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrioridadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoConsultaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: true),
                    HoraFim = table.Column<TimeSpan>(type: "time", nullable: true),
                    Credencial = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ConsultaMarcacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConvertidoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaEsperaConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_ConsultaMarcacao_ConsultaMarcacaoId",
                        column: x => x.ConsultaMarcacaoId,
                        principalSchema: "Consultas",
                        principalTable: "ConsultaMarcacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_Especialidade_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalSchema: "Especialidades",
                        principalTable: "Especialidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_Organismo_OrganismoId",
                        column: x => x.OrganismoId,
                        principalSchema: "Organismos",
                        principalTable: "Organismo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_Prioridades_PrioridadeId",
                        column: x => x.PrioridadeId,
                        principalSchema: "Tratamentos",
                        principalTable: "Prioridades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_TiposConsulta_TipoConsultaId",
                        column: x => x.TipoConsultaId,
                        principalSchema: "Consultas",
                        principalTable: "TiposConsulta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaEsperaConsulta_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "ConsultaMarcacaoId",
                unique: true,
                filter: "[ConsultaMarcacaoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_Data",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_Data_UtenteId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                columns: new[] { "Data", "UtenteId" });

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_EspecialidadeId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_MedicoId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_OrganismoId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "OrganismoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_PrioridadeId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "PrioridadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_TipoConsultaId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "TipoConsultaId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaEsperaConsulta_UtenteId",
                schema: "Consultas",
                table: "ListaEsperaConsulta",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaEsperaConsulta",
                schema: "Consultas");
        }
    }
}
