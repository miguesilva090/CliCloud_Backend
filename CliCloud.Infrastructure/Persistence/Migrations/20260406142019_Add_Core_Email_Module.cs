using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Core_Email_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoEmail",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Server = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Porta = table.Column<int>(type: "int", nullable: false),
                    UseSSL = table.Column<bool>(type: "bit", nullable: false),
                    TipoServico = table.Column<int>(type: "int", nullable: false),
                    Inbox = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Outbox = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PermitirEliminarEmail = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoEmail_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoEmailAutomatico",
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
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoEmailAutomatico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoEmailAutomatico_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoricoEmail",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssuntoEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CorpoEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailDestino = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NomeUtente = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DataHoraCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modulo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoEmail_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoEmail_ClinicaId",
                schema: "Core",
                table: "ConfiguracaoEmail",
                column: "ClinicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoEmailAutomatico_ClinicaId_Ativo",
                schema: "Core",
                table: "ConfiguracaoEmailAutomatico",
                columns: new[] { "ClinicaId", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoEmailAutomatico_ClinicaId_Codigo",
                schema: "Core",
                table: "ConfiguracaoEmailAutomatico",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEmail_ClinicaId_DataHoraCriacao",
                schema: "Core",
                table: "HistoricoEmail",
                columns: new[] { "ClinicaId", "DataHoraCriacao" });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEmail_ClinicaId_Status",
                schema: "Core",
                table: "HistoricoEmail",
                columns: new[] { "ClinicaId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoEmail",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ConfiguracaoEmailAutomatico",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "HistoricoEmail",
                schema: "Core");
        }
    }
}
