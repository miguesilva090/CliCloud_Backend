using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilityFeriadoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.CreateTable(
                name: "Feriado",
                schema: "Utility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feriado", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "UtilizadorId", "NomeSeparador" });

            migrationBuilder.CreateIndex(
                name: "IX_Feriado_ClinicaId_Data",
                schema: "Utility",
                table: "Feriado",
                columns: new[] { "ClinicaId", "Data" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feriado",
                schema: "Utility");

            migrationBuilder.DropIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "UtilizadorId", "NomeSeparador" },
                unique: true);
        }
    }
}
