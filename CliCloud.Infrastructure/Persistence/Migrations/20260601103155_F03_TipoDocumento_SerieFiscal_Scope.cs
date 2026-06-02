using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class F03_TipoDocumento_SerieFiscal_Scope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.AddColumn<int>(
                name: "CodigoTipoDocumentoSaft",
                schema: "Documentos",
                table: "TipoDocumento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoSerie",
                schema: "Documentos",
                table: "Documento",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Documentos.TipoDocumento
                SET NumeroSerie = LTRIM(RTRIM(Abreviatura))
                WHERE NumeroSerie IS NULL OR LTRIM(RTRIM(NumeroSerie)) = '';
            ");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento",
                columns: new[] { "ClinicaId", "Abreviatura" });

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_ClinicaId_NumeroSerie",
                schema: "Documentos",
                table: "TipoDocumento",
                columns: new[] { "ClinicaId", "NumeroSerie" },
                unique: true,
                filter: "[NumeroSerie] IS NOT NULL");

            migrationBuilder.Sql(@"
INSERT INTO Documentos.TipoDocumento (
    Id, ClinicaId, Descricao, Abreviatura, Natureza, TipoMovimento, CodigoTipoDocumentoSaft,
    NumeroSerie, TipoSerie, NumeroDocumento, NumVias, TemCabecalho, Config, AtualizaStock,
    PermiteMovimento, Inactivo, MostraFaturacao, DescarregarTesouraria, Habilitado,
    CodigoATCUD, ATCUDEstado, CreatedBy, CreatedOn
)
SELECT
    NEWID(),
    c.Id,
    v.Descricao,
    v.Abreviatura,
    'D',
    0,
    v.CodigoSaft,
    v.NumeroSerie,
    'N',
    0,
    v.NumVias,
    1,
    0,
    v.AtualizaStock,
    1,
    0,
    v.MostraFaturacao,
    v.DescarregarTesouraria,
    1,
    v.CodigoAtcud,
    'A',
    e.CreatedBy,
    SYSUTCDATETIME()
FROM Core.Clinica c
INNER JOIN Utility.Entidade e ON e.Id = c.Id
CROSS APPLY (VALUES
    (N'Fatura',                    N'FA',  N'FA',  1, 1, 0, 0, 1, N'SEED-FA--'),
    (N'Nota de Débito',            N'ND',  N'ND',  5, 1, 0, 0, 1, N'SEED-ND--'),
    (N'Nota de Crédito',          N'NC',  N'NC',  4, 1, 0, 0, 1, N'SEED-NC--'),
    (N'Guia de Remessa',           N'GR',  N'GR',  1, 0, 0, 0, 0, N'SEED-GR--'),
    (N'Guia de Transporte',        N'GT',  N'GT',  1, 1, 0, 0, 0, N'SEED-GT--'),
    (N'Fatura (Recibo)',           N'FR',  N'FR',  3, 1, 0, 1, 1, N'SEED-FR--'),
    (N'Fatura Pró-Forma',          N'FP',  N'FP',  1, 0, 0, 0, 0, N'SEED-FP--'),
    (N'Recibo',                    N'RC',  N'RC',  3, 1, 0, 1, 1, N'SEED-RC--'),
    (N'V/Fatura',                  N'VF',  N'VF',  3, 1, 0, 1, 1, N'SEED-VF--'),
    (N'Nota Pagamento',            N'NP',  N'NP',  1, 1, 0, 0, 1, N'SEED-NP--'),
    (N'Documento Provisório',      N'DP',  N'DP',  1, 0, 0, 0, 0, N'SEED-DP--'),
    (N'Regularização',             N'RG',  N'RG',  1, 1, 0, 0, 1, N'SEED-RG--'),
    (N'Orçamento',                 N'OR',  N'OR',  1, 0, 0, 0, 0, N'SEED-OR--'),
    (N'Fatura Simplificada Recibo',N'FSR', N'FSR', 2, 1, 0, 1, 1, N'SEED-FSR-'),
    (N'Adiantamento',              N'AD',  N'AD',  1, 1, 0, 0, 1, N'SEED-AD--'),
    (N'Devolução',                 N'DV',  N'DV',  1, 1, 0, 0, 1, N'SEED-DV--'),
    (N'Fatura Simplificada',       N'FS',  N'FS',  2, 1, 0, 0, 1, N'SEED-FS--')
) AS v(Descricao, Abreviatura, NumeroSerie, CodigoSaft, MostraFaturacao, AtualizaStock, DescarregarTesouraria, NumVias, CodigoAtcud)
WHERE e.DeletedOn IS NULL
  AND NOT EXISTS (
      SELECT 1
      FROM Documentos.TipoDocumento td
      WHERE td.ClinicaId = c.Id
        AND td.NumeroSerie = v.NumeroSerie
        AND td.DeletedOn IS NULL
  );
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropIndex(
                name: "IX_TipoDocumento_ClinicaId_NumeroSerie",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "CodigoTipoDocumentoSaft",
                schema: "Documentos",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "TipoSerie",
                schema: "Documentos",
                table: "Documento");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDocumento_ClinicaId_Abreviatura",
                schema: "Documentos",
                table: "TipoDocumento",
                columns: new[] { "ClinicaId", "Abreviatura" },
                unique: true);

            migrationBuilder.Sql(@"
DELETE FROM Documentos.TipoDocumento
WHERE CodigoATCUD LIKE N'SEED-%'
  AND NOT EXISTS (
      SELECT 1 FROM Documentos.Documento d WHERE d.TipoDocumentoId = Documentos.TipoDocumento.Id
  );
");
        }
    }
}
