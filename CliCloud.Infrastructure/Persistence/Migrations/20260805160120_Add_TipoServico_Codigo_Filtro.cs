using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TipoServico_Codigo_Filtro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                schema: "Servicos",
                table: "TipoServico",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Filtro",
                schema: "Servicos",
                table: "TipoServico",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoServico_Codigo_Filtro",
                schema: "Servicos",
                table: "TipoServico",
                columns: new[] { "Codigo", "Filtro" });

            // One-shot: popular Codigo/Filtro a partir de LoteDirect já ligado (BD nova).
            migrationBuilder.Sql(
                """
                UPDATE ts
                SET ts.Codigo = src.TipoServico
                FROM Servicos.TipoServico ts
                INNER JOIN (
                    SELECT TipoServicoRegistoId, MIN(TipoServico) AS TipoServico
                    FROM Credenciais.LoteDirect
                    WHERE TipoServicoRegistoId IS NOT NULL
                      AND TipoServico IS NOT NULL
                      AND TipoServico > 0
                    GROUP BY TipoServicoRegistoId
                ) src ON src.TipoServicoRegistoId = ts.Id
                WHERE ts.Codigo IS NULL
                  AND ts.DeletedOn IS NULL;
                """);

            // One-shot opcional: se dbo.TIPO_SRV ainda existir na mesma instância (coexistência),
            // copiar para Servicos.TipoServico — não é runtime da app.
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.TIPO_SRV', N'U') IS NOT NULL
                BEGIN
                    UPDATE ts
                    SET
                        ts.Codigo = leg.c_tipo_srv,
                        ts.Filtro = leg.filtro
                    FROM Servicos.TipoServico ts
                    INNER JOIN dbo.TIPO_SRV leg
                        ON LTRIM(RTRIM(ISNULL(leg.nome, N''))) = LTRIM(RTRIM(ISNULL(ts.Descricao, N'')))
                    WHERE ts.Codigo IS NULL
                      AND ts.DeletedOn IS NULL;

                    INSERT INTO Servicos.TipoServico
                    (
                        Id, Descricao, Codigo, Filtro, TaxaModeradoraSns, PartilhaSemRequisicao,
                        CreatedBy, CreatedOn, LastModifiedBy, LastModifiedOn, DeletedBy, DeletedOn
                    )
                    SELECT
                        NEWID(),
                        LEFT(LTRIM(RTRIM(leg.nome)), 80),
                        leg.c_tipo_srv,
                        leg.filtro,
                        TRY_CONVERT(decimal(18,2), leg.vTaxa),
                        CASE WHEN ISNULL(leg.PartilhaSemRequisicao, 0) <> 0 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END,
                        '00000000-0000-0000-0000-000000000000',
                        SYSUTCDATETIME(),
                        NULL,
                        NULL,
                        NULL,
                        NULL
                    FROM dbo.TIPO_SRV leg
                    WHERE LTRIM(RTRIM(ISNULL(leg.nome, N''))) <> N''
                      AND NOT EXISTS (
                          SELECT 1
                          FROM Servicos.TipoServico ts
                          WHERE ts.DeletedOn IS NULL
                            AND ts.Codigo = leg.c_tipo_srv
                            AND (
                                (ts.Filtro IS NULL AND leg.filtro IS NULL)
                                OR ts.Filtro = leg.filtro
                            )
                      )
                      AND NOT EXISTS (
                          SELECT 1
                          FROM Servicos.TipoServico ts2
                          WHERE ts2.DeletedOn IS NULL
                            AND LTRIM(RTRIM(ISNULL(ts2.Descricao, N''))) = LTRIM(RTRIM(ISNULL(leg.nome, N'')))
                            AND ts2.Codigo IS NOT NULL
                      );
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoServico_Codigo_Filtro",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "Codigo",
                schema: "Servicos",
                table: "TipoServico");

            migrationBuilder.DropColumn(
                name: "Filtro",
                schema: "Servicos",
                table: "TipoServico");
        }
    }
}
