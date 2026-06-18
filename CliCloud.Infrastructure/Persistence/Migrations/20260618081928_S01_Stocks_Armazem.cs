using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S01_Stocks_Armazem : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Stocks");

            migrationBuilder.CreateTable(
                name: "Armazem",
                schema: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Localidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodigoPostalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ArmazemGeral = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armazem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armazem_CodigoPostal_CodigoPostalId",
                        column: x => x.CodigoPostalId,
                        principalSchema: "Utility",
                        principalTable: "CodigoPostal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_ClinicaId",
                schema: "Stocks",
                table: "Armazem",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_ClinicaId_Codigo",
                schema: "Stocks",
                table: "Armazem",
                columns: new[] { "ClinicaId", "Codigo" },
                unique: true,
                filter: "[DeletedOn] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Armazem_CodigoPostalId",
                schema: "Stocks",
                table: "Armazem",
                column: "CodigoPostalId");

            migrationBuilder.Sql($@"
INSERT INTO Stocks.Armazem (Id, ClinicaId, Codigo, Nome, ArmazemGeral, CreatedBy, CreatedOn)
SELECT
    NEWID(),
    cad.ClinicaId,
    1,
    cad.Nome,
    cad.ArmazemGeral,
    '{SeedUserId}',
    GETUTCDATE()
FROM Core.ClinicaArmazemDefault cad
WHERE cad.DeletedOn IS NULL
  AND NOT EXISTS (
      SELECT 1 FROM Stocks.Armazem a
      WHERE a.ClinicaId = cad.ClinicaId AND a.DeletedOn IS NULL
  );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Armazem",
                schema: "Stocks");
        }
    }
}
