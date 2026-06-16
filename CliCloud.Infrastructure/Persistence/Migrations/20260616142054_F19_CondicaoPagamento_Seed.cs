using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Paridade com legado COND_PAG (CondicaoPagamentoLst).
    /// A F16 criou a tabela mas não fez seed; os modos de pagamento já tinham seed por clínica.
    /// </summary>
    public partial class F19_CondicaoPagamento_Seed : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DECLARE @CreatedBy uniqueidentifier = '{SeedUserId}';
                DECLARE @Now datetime2 = SYSUTCDATETIME();

                INSERT INTO [Pagamentos].[CondicaoPagamento]
                    (Id, ClinicaId, Codigo, Descricao, NDiasPagamento, Desconto, CreatedBy, CreatedOn)
                SELECT NEWID(), c.Id, v.Codigo, v.Descricao, v.NDiasPagamento, v.Desconto, @CreatedBy, @Now
                FROM [Core].[Clinica] c
                CROSS JOIN (VALUES
                    (1, N'1 mês',      30,  CAST(0 AS decimal(18,2))),
                    (2, N'6 meses',    180, CAST(0 AS decimal(18,2))),
                    (3, N'trimestre',  90,  CAST(10 AS decimal(18,2)))
                ) v(Codigo, Descricao, NDiasPagamento, Desconto)
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM [Pagamentos].[CondicaoPagamento] cp
                    WHERE cp.ClinicaId = c.Id
                      AND cp.Codigo = v.Codigo
                      AND cp.DeletedOn IS NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM [Pagamentos].[CondicaoPagamento]
                WHERE [Codigo] IN (1, 2, 3)
                  AND [Descricao] IN (N'1 mês', N'6 meses', N'trimestre')
                  AND [DeletedOn] IS NULL;
                """);
        }
    }
}
