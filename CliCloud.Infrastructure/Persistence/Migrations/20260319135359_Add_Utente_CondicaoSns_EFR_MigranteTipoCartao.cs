using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Utente_CondicaoSns_EFR_MigranteTipoCartao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CondicaoSns",
                schema: "Utentes",
                table: "Utente",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataValidadeEfr",
                schema: "Utentes",
                table: "Utente",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MigranteTipoCartao",
                schema: "Utentes",
                table: "Utente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroBeneficiarioEfr",
                schema: "Utentes",
                table: "Utente",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CondicaoSns",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "DataValidadeEfr",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "EntidadeFinanceiraResponsavelId",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "MigranteTipoCartao",
                schema: "Utentes",
                table: "Utente");

            migrationBuilder.DropColumn(
                name: "NumeroBeneficiarioEfr",
                schema: "Utentes",
                table: "Utente");
        }
    }
}
