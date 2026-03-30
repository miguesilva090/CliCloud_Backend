using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Clinica_Tratamentos_Config_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Em alguns ambientes estas colunas já podem ter sido removidas manualmente.
            // Para evitar falhas no startup, removemos apenas quando existirem (e removemos default constraints se existirem).
            migrationBuilder.Sql(@"
DECLARE @col sysname;
DECLARE @sql nvarchar(max);

DECLARE @cols TABLE (Name sysname NOT NULL);
INSERT INTO @cols (Name)
VALUES
 (N'AreaPrestacaoDefeitoAreaZ'),
 (N'AvisoInqueritoSessoesDiarias'),
 (N'ControlarAparelhos'),
 (N'CredencialExternaAdse'),
 (N'FaltasConsecutivasMax'),
 (N'FaltasMax'),
 (N'Segundos'),
 (N'Taxamoderadora'),
 (N'TipoPagamento'),
 (N'TipoSrvTratamentos');

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaPrestacaoDefeitoAreaZ",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvisoInqueritoSessoesDiarias",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ControlarAparelhos",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CredencialExternaAdse",
                schema: "Core",
                table: "Clinica",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaltasConsecutivasMax",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaltasMax",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Segundos",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Taxamoderadora",
                schema: "Core",
                table: "Clinica",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoPagamento",
                schema: "Core",
                table: "Clinica",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoSrvTratamentos",
                schema: "Core",
                table: "Clinica",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
