using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserScopeSeparadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_Ordem",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.AddColumn<Guid>(
                name: "UtilizadorId",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UtilizadorId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            // Backfill para o utilizador atual (ambientes com dados já existentes).
            migrationBuilder.Sql(
                @"
DECLARE @CurrentUserId uniqueidentifier;
SELECT TOP (1) @CurrentUserId = TRY_CONVERT(uniqueidentifier, [Id])
FROM [AspNetUsers]
WHERE TRY_CONVERT(uniqueidentifier, [Id]) IS NOT NULL;

IF @CurrentUserId IS NOT NULL
BEGIN
    UPDATE [ProcessoClinico].[SeparadorPersonalizado]
    SET [UtilizadorId] = @CurrentUserId
    WHERE [UtilizadorId] = '00000000-0000-0000-0000-000000000000';

    UPDATE [ProcessoClinico].[FichaClinicaSecaoTemplate]
    SET [UtilizadorId] = @CurrentUserId
    WHERE [UtilizadorId] = '00000000-0000-0000-0000-000000000000';
END;

;WITH Duplicados AS
(
    SELECT
        [Id],
        [Codigo],
        ROW_NUMBER() OVER (
            PARTITION BY [UtilizadorId], [Codigo]
            ORDER BY [Id]
        ) AS RN
    FROM [ProcessoClinico].[FichaClinicaSecaoTemplate]
)
UPDATE T
SET [Codigo] = CONCAT(
    LEFT(T.[Codigo], 84),
    '_DUP_',
    CAST(D.RN AS varchar(10))
)
FROM [ProcessoClinico].[FichaClinicaSecaoTemplate] T
INNER JOIN Duplicados D ON D.[Id] = T.[Id]
WHERE D.RN > 1;"
            );

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "UtilizadorId", "NomeSeparador" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_Ordem",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "UtilizadorId", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_FichaClinicaSecaoTemplate_UtilizadorId_Codigo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate",
                columns: new[] { "UtilizadorId", "Codigo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_NomeSeparador",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.DropIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_UtilizadorId_Ordem",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.DropIndex(
                name: "IX_FichaClinicaSecaoTemplate_UtilizadorId_Codigo",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                schema: "ProcessoClinico",
                table: "FichaClinicaSecaoTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_SeparadorPersonalizado_ClinicaId_Ordem",
                schema: "ProcessoClinico",
                table: "SeparadorPersonalizado",
                columns: new[] { "ClinicaId", "Ordem" });
        }
    }
}
