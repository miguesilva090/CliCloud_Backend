using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_LoteFisioterapia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoteFisioterapia",
                schema: "Credenciais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Indice = table.Column<int>(type: "int", nullable: false),
                    NumeroLote = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    CodigoOrganismo = table.Column<int>(type: "int", nullable: false),
                    TipoLote = table.Column<int>(type: "int", nullable: false),
                    TipoServico = table.Column<int>(type: "int", nullable: false),
                    DataLote = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTaxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Isencao = table.Column<int>(type: "int", nullable: true),
                    NumeroRequisicoes = table.Column<int>(type: "int", nullable: false),
                    TotalK = table.Column<int>(type: "int", nullable: true),
                    TotalC = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoteFisioterapia", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoteFisioterapia_Ano_Mes",
                schema: "Credenciais",
                table: "LoteFisioterapia",
                columns: new[] { "Ano", "Mes" });

            migrationBuilder.CreateIndex(
                name: "IX_LoteFisioterapia_Ano_Mes_CodigoOrganismo_TipoLote_TipoServico_NumeroLote",
                schema: "Credenciais",
                table: "LoteFisioterapia",
                columns: new[] { "Ano", "Mes", "CodigoOrganismo", "TipoLote", "TipoServico", "NumeroLote" });

            migrationBuilder.CreateIndex(
                name: "IX_LoteFisioterapia_Indice",
                schema: "Credenciais",
                table: "LoteFisioterapia",
                column: "Indice",
                unique: true);

            // One-shot: copiar dados históricos se dbo.LOTESPFISIO existir na mesma instância (não é runtime).
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.LOTESPFISIO', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO Credenciais.LoteFisioterapia
                    (
                        Id, Indice, NumeroLote, Ano, Mes, CodigoOrganismo, TipoLote, TipoServico,
                        DataLote, Quantidade, Valor, ValorTaxa, Tipo, Isencao, NumeroRequisicoes,
                        TotalK, TotalC, CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedBy, DeletedOn
                    )
                    SELECT
                        NEWID(),
                        l.indice,
                        ISNULL(l.c_lote, 0),
                        ISNULL(l.ano, 0),
                        ISNULL(l.nummes, 0),
                        ISNULL(l.codinst, 0),
                        ISNULL(l.codtipo, 0),
                        ISNULL(l.tiposerv, 0),
                        ISNULL(l.datalote, SYSUTCDATETIME()),
                        ISNULL(CAST(l.quantidade AS int), 0),
                        ISNULL(CAST(l.valor AS decimal(18,2)), 0),
                        ISNULL(CAST(l.vtaxa AS decimal(18,2)), 0),
                        NULLIF(LTRIM(RTRIM(CAST(l.tipo AS nvarchar(1)))), N''),
                        l.isento,
                        ISNULL(l.numreq, 0),
                        l.totalK,
                        l.totalC,
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL,
                        NULL,
                        NULL
                    FROM dbo.LOTESPFISIO l
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM Credenciais.LoteFisioterapia lf
                        WHERE lf.Indice = l.indice
                    );
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoteFisioterapia",
                schema: "Credenciais");
        }
    }
}
