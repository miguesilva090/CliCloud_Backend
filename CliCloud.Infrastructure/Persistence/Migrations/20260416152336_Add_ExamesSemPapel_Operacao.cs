using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ExamesSemPapel_Operacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamesSemPapelOperacao",
                schema: "Consultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequisicaoId = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    AreaPrestacao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Assinado = table.Column<bool>(type: "bit", nullable: false),
                    Comunicado = table.Column<bool>(type: "bit", nullable: false),
                    Lotes = table.Column<bool>(type: "bit", nullable: false),
                    IsencaoTaxa = table.Column<bool>(type: "bit", nullable: false),
                    ComTaxa = table.Column<bool>(type: "bit", nullable: false),
                    Pnp = table.Column<bool>(type: "bit", nullable: false),
                    UltimaOperacaoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamesSemPapelOperacao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamesSemPapelOperacao_ClinicaId_RequisicaoId",
                schema: "Consultas",
                table: "ExamesSemPapelOperacao",
                columns: new[] { "ClinicaId", "RequisicaoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamesSemPapelOperacao",
                schema: "Consultas");
        }
    }
}
