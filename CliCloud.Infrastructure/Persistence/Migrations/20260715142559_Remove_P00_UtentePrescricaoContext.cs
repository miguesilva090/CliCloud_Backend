using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_P00_UtentePrescricaoContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utente_EntidadeFinanceira_EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropTable(
                name: "UtentePatologiaComparticipacao",
                schema: "Utentes");

            migrationBuilder.DropIndex(
                name: "IX_Utente_CodigoLegado",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_Utente_EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropIndex(
                name: "IX_EntidadeFinanceira_Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira");

            migrationBuilder.DropColumn(
                name: "CodigoLegado",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "OutrosBeneficios",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "Pensionista",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodigoLegado",
                schema: "Utentes",
                table: "Utente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OutrosBeneficios",
                schema: "Utentes",
                table: "Utente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pensionista",
                schema: "Utentes",
                table: "Utente",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UtentePatologiaComparticipacao",
                schema: "Utentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoComparticipacao = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Designacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtentePatologiaComparticipacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtentePatologiaComparticipacao_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalSchema: "Utentes",
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utente_CodigoLegado",
                schema: "Utentes",
                table: "Utente",
                column: "CodigoLegado");

            migrationBuilder.CreateIndex(
                name: "IX_Utente_EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente",
                column: "EntidadeFinanceiraResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeFinanceira_Codigo",
                schema: "EntidadesFinanceiras",
                table: "EntidadeFinanceira",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_UtentePatologiaComparticipacao_UtenteId_CodigoComparticipacao",
                schema: "Utentes",
                table: "UtentePatologiaComparticipacao",
                columns: new[] { "UtenteId", "CodigoComparticipacao" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Utente_EntidadeFinanceira_EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente",
                column: "EntidadeFinanceiraResponsavelId",
                principalSchema: "EntidadesFinanceiras",
                principalTable: "EntidadeFinanceira",
                principalColumn: "Id");
        }
    }
}
