using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Backfill_TiposConsulta_CodigoLegado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.TIPOS_CONSULTA', N'U') IS NOT NULL
                BEGIN
                    UPDATE tc
                    SET tc.CodigoLegado = leg.codigo
                    FROM Consultas.TiposConsulta tc
                    INNER JOIN dbo.TIPOS_CONSULTA leg
                        ON LTRIM(RTRIM(ISNULL(leg.designa, N''))) = LTRIM(RTRIM(ISNULL(tc.Designacao, N'')))
                    WHERE tc.CodigoLegado IS NULL;
                END

                UPDATE Consultas.TiposConsulta
                SET CodigoLegado = 1
                WHERE CodigoLegado IS NULL
                  AND (
                    Designacao LIKE N'%1ª%'
                    OR Designacao LIKE N'%1a %'
                    OR Designacao LIKE N'1 %'
                    OR Designacao LIKE N'%Primeir%'
                  );

                UPDATE Consultas.TiposConsulta
                SET CodigoLegado = 2
                WHERE CodigoLegado IS NULL
                  AND (
                    Designacao LIKE N'%Segu%'
                    OR Designacao LIKE N'%2ª%'
                    OR Designacao LIKE N'%2a %'
                  );

                UPDATE Consultas.TiposConsulta
                SET CodigoLegado = 3
                WHERE CodigoLegado IS NULL
                  AND (
                    Designacao LIKE N'%3ª%'
                    OR Designacao LIKE N'%3a %'
                    OR Designacao LIKE N'%AV%'
                  );

                UPDATE Consultas.TiposConsulta
                SET CodigoLegado = 4
                WHERE CodigoLegado IS NULL
                  AND (
                    Designacao LIKE N'%4ª%'
                    OR Designacao LIKE N'%4a %'
                    OR Designacao LIKE N'%Feriado%'
                    OR Designacao LIKE N'%Tele%'
                  );
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Consultas.TiposConsulta SET CodigoLegado = NULL;");
        }
    }
}
