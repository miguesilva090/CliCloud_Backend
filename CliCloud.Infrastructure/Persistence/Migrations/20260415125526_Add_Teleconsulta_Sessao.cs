using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Teleconsulta_Sessao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeleconsultaSessao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaMarcacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MeetingUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "jitsi"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Criada"),
                    InicioPrevistoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FimPrevistoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InicioEfetivoUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FimEfetivoUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TokenMedico = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TokenUtente = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeleconsultaSessao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeleconsultaSessao_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeleconsultaSessao_ConsultaMarcacao_ConsultaMarcacaoId",
                        column: x => x.ConsultaMarcacaoId,
                        principalSchema: "Consultas",
                        principalTable: "ConsultaMarcacao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeleconsultaSessao_ClinicaId_ConsultaMarcacaoId_Ativo",
                schema: "Consultas",
                table: "TeleconsultaSessao",
                columns: new[] { "ClinicaId", "ConsultaMarcacaoId", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_TeleconsultaSessao_ConsultaMarcacaoId",
                schema: "Consultas",
                table: "TeleconsultaSessao",
                column: "ConsultaMarcacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TeleconsultaSessao_MeetingId",
                schema: "Consultas",
                table: "TeleconsultaSessao",
                column: "MeetingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeleconsultaSessao",
                schema: "Consultas");
        }
    }
}
