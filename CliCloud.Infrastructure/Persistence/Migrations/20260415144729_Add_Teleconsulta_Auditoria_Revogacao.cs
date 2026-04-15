using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Teleconsulta_Auditoria_Revogacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LinksAtivos",
                schema: "Consultas",
                table: "TeleconsultaSessao",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LinksRevogadosEmUtc",
                schema: "Consultas",
                table: "TeleconsultaSessao",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeleconsultaAcessoLog",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeleconsultaSessaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultaMarcacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Papel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sucesso = table.Column<bool>(type: "bit", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeleconsultaAcessoLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeleconsultaAcessoLog_TeleconsultaSessao_TeleconsultaSessaoId",
                        column: x => x.TeleconsultaSessaoId,
                        principalSchema: "Consultas",
                        principalTable: "TeleconsultaSessao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeleconsultaAcessoLog_ClinicaId_CreatedOn",
                schema: "Consultas",
                table: "TeleconsultaAcessoLog",
                columns: new[] { "ClinicaId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_TeleconsultaAcessoLog_TeleconsultaSessaoId_CreatedOn",
                schema: "Consultas",
                table: "TeleconsultaAcessoLog",
                columns: new[] { "TeleconsultaSessaoId", "CreatedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeleconsultaAcessoLog",
                schema: "Consultas");

            migrationBuilder.DropColumn(
                name: "LinksAtivos",
                schema: "Consultas",
                table: "TeleconsultaSessao");

            migrationBuilder.DropColumn(
                name: "LinksRevogadosEmUtc",
                schema: "Consultas",
                table: "TeleconsultaSessao");
        }
    }
}
