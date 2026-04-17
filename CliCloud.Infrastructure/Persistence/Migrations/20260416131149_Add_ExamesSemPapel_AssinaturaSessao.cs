using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ExamesSemPapel_AssinaturaSessao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaPrestacaoAssinarESPDefeito",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExamesSemPapelAssinaturaSessao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtilizadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CMedico = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoCartao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DigestValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SignatureValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Assinatura = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    AssinaturaSubCA = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: false),
                    AtualizadoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamesSemPapelAssinaturaSessao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamesSemPapelAssinaturaSessao_UtilizadorId",
                schema: "Consultas",
                table: "ExamesSemPapelAssinaturaSessao",
                column: "UtilizadorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamesSemPapelAssinaturaSessao",
                schema: "Consultas");

            migrationBuilder.DropColumn(
                name: "AreaPrestacaoAssinarESPDefeito",
                schema: "Core",
                table: "Clinica");
        }
    }
}
