using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sms_Module_Core : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoSms",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    UsenditArpoone = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Loginapi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Passwordapi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Numapi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remetente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArpooneUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArpooneSender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArpooneApiKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ArpooneOrganizationID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WebhookDeliveredUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebhookNotDeliveredUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebhookPendingUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ControloSmsAutomaticos = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoSms_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoSmsAutomatica",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ativo = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Diasantecedencia = table.Column<int>(type: "int", nullable: false),
                    Textomensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TodosMedicos = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSmsAutomatica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoSmsAutomatica_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoSmsAutomaticaMedico",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoConfiguracao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CodigoMedico = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSmsAutomaticaMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoSmsAutomaticaMedico_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoricoSms",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdMensagem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextoMensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDestinatario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modulo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CodigoUtente = table.Column<int>(type: "int", nullable: true),
                    CodigoMedico = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoFisioterapeuta = table.Column<int>(type: "int", nullable: true),
                    CodigoConsulta = table.Column<int>(type: "int", nullable: true),
                    CodigoTratamento = table.Column<int>(type: "int", nullable: true),
                    CodigoAula = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoSms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoSms_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmsRecebido",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataHoraRecebimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroOrigem = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NumeroDestino = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TextoMensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Encoding = table.Column<int>(type: "int", nullable: true),
                    Mcc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Mnc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TotalSegmentos = table.Column<int>(type: "int", nullable: true),
                    DataHoraProcessamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoUtente = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsRecebido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsRecebido_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSms_ClinicaId",
                schema: "Core",
                table: "ConfiguracaoSms",
                column: "ClinicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSms_ClinicaId_Ativo",
                schema: "Core",
                table: "ConfiguracaoSms",
                columns: new[] { "ClinicaId", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSmsAutomatica_ClinicaId_Ativo",
                schema: "Core",
                table: "ConfiguracaoSmsAutomatica",
                columns: new[] { "ClinicaId", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSmsAutomatica_ClinicaId_Codigo",
                schema: "Core",
                table: "ConfiguracaoSmsAutomatica",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSmsAutomaticaMedico_ClinicaId_CodigoConfiguracao_CodigoMedico",
                schema: "Core",
                table: "ConfiguracaoSmsAutomaticaMedico",
                columns: new[] { "ClinicaId", "CodigoConfiguracao", "CodigoMedico" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSms_ClinicaId_DataHoraCriacao",
                schema: "Core",
                table: "HistoricoSms",
                columns: new[] { "ClinicaId", "DataHoraCriacao" });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSms_ClinicaId_IdMensagem",
                schema: "Core",
                table: "HistoricoSms",
                columns: new[] { "ClinicaId", "IdMensagem" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSms_ClinicaId_Modulo",
                schema: "Core",
                table: "HistoricoSms",
                columns: new[] { "ClinicaId", "Modulo" });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSms_ClinicaId_Status",
                schema: "Core",
                table: "HistoricoSms",
                columns: new[] { "ClinicaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SmsRecebido_ClinicaId_DataHoraRecebimento",
                schema: "Core",
                table: "SmsRecebido",
                columns: new[] { "ClinicaId", "DataHoraRecebimento" });

            migrationBuilder.CreateIndex(
                name: "IX_SmsRecebido_ClinicaId_NumeroOrigem",
                schema: "Core",
                table: "SmsRecebido",
                columns: new[] { "ClinicaId", "NumeroOrigem" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoSms",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ConfiguracaoSmsAutomatica",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ConfiguracaoSmsAutomaticaMedico",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "HistoricoSms",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "SmsRecebido",
                schema: "Core");
        }
    }
}
