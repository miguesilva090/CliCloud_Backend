using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Configuracao_Teleconsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoTeleconsulta",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "jitsi"),
                    BaseMeetingUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, defaultValue: "https://meet.jit.si"),
                    JwtAtivo = table.Column<bool>(type: "bit", nullable: false),
                    JwtAppId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    JwtApiKey = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    JwtKid = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    JwtPrivateKey = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    JanelaEntradaMinutosAntes = table.Column<int>(type: "int", nullable: false, defaultValue: 15),
                    DuracaoPadraoMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    PermitirEntradaAntesDoInicio = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LobbyAtivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoTeleconsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoTeleconsulta_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoTeleconsulta_ClinicaId",
                schema: "Core",
                table: "ConfiguracaoTeleconsulta",
                column: "ClinicaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoTeleconsulta_ClinicaId_Ativo",
                schema: "Core",
                table: "ConfiguracaoTeleconsulta",
                columns: new[] { "ClinicaId", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoTeleconsulta",
                schema: "Core");
        }
    }
}
