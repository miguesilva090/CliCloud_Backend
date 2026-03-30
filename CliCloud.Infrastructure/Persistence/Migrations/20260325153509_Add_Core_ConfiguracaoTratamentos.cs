using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Core_ConfiguracaoTratamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOTA IMPORTANTE:
            // Em alguns ambientes (dev) estas colunas já podem ter sido removidas manualmente,
            // antes do __EFMigrationsHistory existir. Para evitar rebentar no startup,
            // removemos as colunas apenas quando existirem.
            migrationBuilder.Sql(@"
DECLARE @col sysname;
DECLARE @sql nvarchar(max);

DECLARE @cols TABLE (Name sysname NOT NULL);
INSERT INTO @cols (Name)
VALUES
 (N'Ano'),
 (N'Codcentral'),
 (N'Codfact'),
 (N'Codinstquota'),
 (N'Codintern'),
 (N'Credito'),
 (N'Datafac'),
 (N'Datafinal'),
 (N'Datainic'),
 (N'DiretoriaQRC'),
 (N'Filial'),
 (N'Idnum'),
 (N'NVias'),
 (N'Numdiasquota'),
 (N'Recibo'),
 (N'Tcamas'),
 (N'Tiponotificacao'),
 (N'Valorquota');

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
SELECT Name FROM @cols;

OPEN cur;
FETCH NEXT FROM cur INTO @col;
WHILE @@FETCH_STATUS = 0
BEGIN
  IF COL_LENGTH(N'Core.Clinica', @col) IS NOT NULL
  BEGIN
    DECLARE @constraint sysname;
    SELECT @constraint = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c]
      ON [d].[parent_column_id] = [c].[column_id]
     AND [d].[parent_object_id] = [c].[object_id]
    WHERE [d].[parent_object_id] = OBJECT_ID(N'[Core].[Clinica]')
      AND [c].[name] = @col;

    IF @constraint IS NOT NULL
    BEGIN
      SET @sql = N'ALTER TABLE [Core].[Clinica] DROP CONSTRAINT [' + @constraint + N'];';
      EXEC sp_executesql @sql;
    END

    SET @sql = N'ALTER TABLE [Core].[Clinica] DROP COLUMN [' + @col + N'];';
    EXEC sp_executesql @sql;
  END

  FETCH NEXT FROM cur INTO @col;
END

CLOSE cur;
DEALLOCATE cur;
");

            migrationBuilder.CreateTable(
                name: "ConfiguracaoTratamentos",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoSrvTratamentos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaPrestacaoDefeitoAreaZ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControlarAparelhos = table.Column<bool>(type: "bit", nullable: true),
                    Segundos = table.Column<int>(type: "int", nullable: true),
                    FaltasMax = table.Column<int>(type: "int", nullable: true),
                    FaltasConsecutivasMax = table.Column<int>(type: "int", nullable: true),
                    Taxamoderadora = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CredencialExternaAdse = table.Column<bool>(type: "bit", nullable: true),
                    TipoPagamento = table.Column<int>(type: "int", nullable: true),
                    AvisoInqueritoSessoesDiarias = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoTratamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoTratamentos_Clinica_ClinicaId",
                        column: x => x.ClinicaId,
                        principalSchema: "Core",
                        principalTable: "Clinica",
                        principalColumn: "Id");
                });

            migrationBuilder.Sql(@"
-- Migração de dados legacy: em algumas BD as colunas podem já não existir (alterações manuais).
-- Para evitar falhas, lemos cada coluna apenas se existir.
-- Nota: NÃO filtramos por Core.Clinica.DeletedOn porque essa coluna pode não existir em alguns ambientes.
INSERT INTO [Core].[ConfiguracaoTratamentos]
(
  [Id],
  [ClinicaId],
  [TipoSrvTratamentos],
  [AreaPrestacaoDefeitoAreaZ],
  [ControlarAparelhos],
  [Segundos],
  [FaltasMax],
  [FaltasConsecutivasMax],
  [Taxamoderadora],
  [CredencialExternaAdse],
  [TipoPagamento],
  [AvisoInqueritoSessoesDiarias],
  [CreatedBy],
  [CreatedOn],
  [LastModifiedBy],
  [LastModifiedOn],
  [DeletedOn],
  [DeletedBy]
)
SELECT
  NEWID(),
  c.[Id],
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'TipoSrvTratamentos') IS NOT NULL THEN c.[TipoSrvTratamentos] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'AreaPrestacaoDefeitoAreaZ') IS NOT NULL THEN c.[AreaPrestacaoDefeitoAreaZ] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'ControlarAparelhos') IS NOT NULL THEN c.[ControlarAparelhos] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'Segundos') IS NOT NULL THEN c.[Segundos] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'FaltasMax') IS NOT NULL THEN c.[FaltasMax] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'FaltasConsecutivasMax') IS NOT NULL THEN c.[FaltasConsecutivasMax] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'Taxamoderadora') IS NOT NULL THEN c.[Taxamoderadora] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'CredencialExternaAdse') IS NOT NULL THEN c.[CredencialExternaAdse] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'TipoPagamento') IS NOT NULL THEN c.[TipoPagamento] ELSE NULL END,
  CASE WHEN COL_LENGTH(N'Core.Clinica', N'AvisoInqueritoSessoesDiarias') IS NOT NULL THEN c.[AvisoInqueritoSessoesDiarias] ELSE NULL END,
  CAST('00000000-0000-0000-0000-000000000000' AS uniqueidentifier),
  SYSUTCDATETIME(),
  NULL,
  NULL,
  NULL,
  NULL
FROM [Core].[Clinica] c
;
");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoTratamentos_ClinicaId",
                schema: "Core",
                table: "ConfiguracaoTratamentos",
                column: "ClinicaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoTratamentos",
                schema: "Core");

            migrationBuilder.AddColumn<int>(
                name: "Ano",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codcentral",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codfact",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codinstquota",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codintern",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Credito",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Datafac",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Datafinal",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Datainic",
                schema: "Core",
                table: "Clinica",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiretoriaQRC",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Filial",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Idnum",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NVias",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Numdiasquota",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Recibo",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tcamas",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tiponotificacao",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Valorquota",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
