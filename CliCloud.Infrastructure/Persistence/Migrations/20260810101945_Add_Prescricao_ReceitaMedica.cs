using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Prescricao_ReceitaMedica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Prescricao");

            migrationBuilder.CreateTable(
                name: "ReceitaMedica",
                schema: "Prescricao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataPrescricao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoReceita = table.Column<int>(type: "int", nullable: false),
                    Desmaterializada = table.Column<int>(type: "int", nullable: false),
                    NumeroReceitaLocal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroReceita = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoAcesso = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Enviada = table.Column<int>(type: "int", nullable: false),
                    Anulada = table.Column<int>(type: "int", nullable: false),
                    DataAnulacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoAnulacaoCodigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MotivoAnulacaoDescricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReceitaRenovavel = table.Column<int>(type: "int", nullable: false),
                    NumeroVias = table.Column<int>(type: "int", nullable: true),
                    PrescricaoPorNome = table.Column<int>(type: "int", nullable: false),
                    MotivoPrescricaoNome = table.Column<int>(type: "int", nullable: true),
                    NumeroBeneficiarioEfr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SiglaEfr = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LocalPrescricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EstadoEnvio = table.Column<int>(type: "int", nullable: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceitaMedica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceitaMedica_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceitaMedica_Medico_MedicoId",
                        column: x => x.MedicoId,
                        principalSchema: "Medicos",
                        principalTable: "Medico",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceitaMedica_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReceitaLinha",
                schema: "Prescricao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceitaMedicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    TipoLinha = table.Column<int>(type: "int", nullable: false),
                    EmbId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cnpem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescricaoEmbalagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Pvp = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Comparticipacao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorUtente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Posologia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CodValidade = table.Column<int>(type: "int", nullable: true),
                    DataValidade = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CodJustificacaoQuantidade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JustificacaoQuantidade = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceitaLinha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceitaLinha_ReceitaMedica_ReceitaMedicaId",
                        column: x => x.ReceitaMedicaId,
                        principalSchema: "Prescricao",
                        principalTable: "ReceitaMedica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaLinha_ReceitaMedicaId",
                schema: "Prescricao",
                table: "ReceitaLinha",
                column: "ReceitaMedicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaMedica_ClinicaId",
                schema: "Prescricao",
                table: "ReceitaMedica",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaMedica_MedicoId",
                schema: "Prescricao",
                table: "ReceitaMedica",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceitaMedica_UtenteId",
                schema: "Prescricao",
                table: "ReceitaMedica",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceitaLinha",
                schema: "Prescricao");

            migrationBuilder.DropTable(
                name: "ReceitaMedica",
                schema: "Prescricao");
        }
    }
}
