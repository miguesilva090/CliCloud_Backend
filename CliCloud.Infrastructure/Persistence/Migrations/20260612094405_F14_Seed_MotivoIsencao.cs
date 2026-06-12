using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Paridade com legado Faturacao.MotivoIsencao.LoadDefaultData (CliCloud.Dados.Faturacao).
    /// Insere os 28 motivos SAFT por defeito quando ainda não existem (por Codigo).
    /// </summary>
    public partial class F14_Seed_MotivoIsencao : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DECLARE @CreatedBy uniqueidentifier = '{SeedUserId}';
                DECLARE @Now datetime2 = SYSUTCDATETIME();

                DECLARE @Rows TABLE (
                    Id uniqueidentifier NOT NULL,
                    Codigo nvarchar(20) NOT NULL,
                    CodigoSaft nvarchar(12) NOT NULL,
                    Descricao nvarchar(254) NOT NULL,
                    Norma nvarchar(254) NULL,
                    Mencao nvarchar(254) NULL
                );

                INSERT INTO @Rows (Id, Codigo, CodigoSaft, Descricao, Norma, Mencao) VALUES
                ('E1410000-0000-4000-8000-000000000001', N'1',  N'M01', N'', N'Artigo 16.º n.º 6 alíneaa) a d) do CIVA ', NULL),
                ('E1410000-0000-4000-8000-000000000002', N'11', N'M08', N'Situações em queo destinatário ou adquirente é o devedor do imposto', N'Artigo 2.º n.º 1 alínea j) do CIVA', N'IVA - autoliquidação'),
                ('E1410000-0000-4000-8000-000000000003', N'12', N'M08', N'Situações em que o destinatário ou adquirente é o devedor do imposto', N'Artigo 6.º do CIVA', N'IVA - autoliquidação'),
                ('E1410000-0000-4000-8000-000000000004', N'13', N'M08', N'Situações em que o destinatário ou adquirente é o devedor do imposto', N'Artigo 2.º n.º 1 alínea l) do CIVA', N'IVA - autoliquidação'),
                ('E1410000-0000-4000-8000-000000000005', N'14', N'M08', N'Regime especial aplicável ao ouro para investimento', N'Decreto-Lei n.º 21/2007, de 29 de Janeiro', N'IVA - autoliquiação'),
                ('E1410000-0000-4000-8000-000000000006', N'15', N'M08', N'Regime especial aplicável ao ouro para investimento', N'Decreto-Lei n.º 362/99, de 16 de Setembro', N'IVA - autoliquidação'),
                ('E1410000-0000-4000-8000-000000000007', N'16', N'M09', N'Regime especial dos pequenos retalhistas - Artigo 60.º do CIVA', N'Artigo 60.º do CIVA', N'IVA - não confere direito a dedução'),
                ('E1410000-0000-4000-8000-000000000008', N'17', N'M09', N'Regime de tributação dos combustíveis liquidos aplicável aos revendedores', N'Artigo 72.º n.º 4 do CIVA', N'IVA - não confere direito a dedução'),
                ('E1410000-0000-4000-8000-000000000009', N'18', N'M10', N'Regime especial de isenção - Artigo 53.º do CIVA', N'Artigo 53.º do CIVA', N'IVA - Regime de isenção'),
                ('E1410000-0000-4000-8000-00000000000A', N'19', N'M11', N'', N'Decreto-Lei n.º 346/85, de 23 de agosto', N'Não tributado'),
                ('E1410000-0000-4000-8000-00000000000B', N'2',  N'M02', N'', N'Artigo 6.º do Decreto-Lei n.º 198/90, de 19 de Junho', N'Artigo 6.º do Decreto-Lei n.º 198/90, de 19 de Junho'),
                ('E1410000-0000-4000-8000-00000000000C', N'20', N'M12', N'Regime especial das agências de viagens e circuitos turísticos', N'Decreto-Lei n.º 221/85, de 3 de Julho', N'Regime da margem de lucro - Agências de viagens'),
                ('E1410000-0000-4000-8000-00000000000D', N'21', N'M13', N'Regime especial de tributação dos bens em segunda mão, objetos de arte, de coleção e antiguidades', N'Decreto-Lei n.º 199/96, de 18 de outubro', N'Regime da margem de lucro - Bens em segunda mão'),
                ('E1410000-0000-4000-8000-00000000000E', N'22', N'M13', N'Regime especial de tributação dos bens em segunda mão, objetos de arte, de coleção e antiguidades', N'Decreto-Lei n.º 199/96, de 18 de outubro', N'Regime da margem de lucro - Objetos de arte'),
                ('E1410000-0000-4000-8000-00000000000F', N'23', N'M13', N'Regime especial de tributação dos bens em segunda mão, objetos de arte, de coleção e antiguidades', N'Decreto-Lei n.º 199/96, de 18 de outubro', N'Regime da margem de lucro - Objetos de coleção e antiguidades'),
                ('E1410000-0000-4000-8000-000000000010', N'24', N'M14', N' ', N'Decreto-Lei n.º 199/96, de 18 de outubro', N'Regime da margem de lucro - Objetos de arte'),
                ('E1410000-0000-4000-8000-000000000011', N'25', N'M15', N' ', N'Decreto-Lei n.º 199/96, de 18 de outubro', N'Regime da margem de lucro - Objetos de coleção e antiguidades'),
                ('E1410000-0000-4000-8000-000000000012', N'26', N'M16', N' ', N'Artigo 14.º do RITI', N'Isento Artigo 14.º do RITI (ou similar)'),
                ('E1410000-0000-4000-8000-000000000013', N'27', N'M99', N' ', N'Outras situações de não liquidação do imposto', N'Não sujeito; não tributado (ou similar)'),
                ('E1410000-0000-4000-8000-000000000014', N'3',  N'M03', N'Regime especial de exigibilidade do IVA nas empreitadas e subempreitadas de obras públicas', N'N.º 1 do art.º 7.º do anexo ao Decreto-Lei n.º 204/97, de 9 de Agosto', N'Exigibilidade de caixa'),
                ('E1410000-0000-4000-8000-000000000015', N'4',  N'M03', N'Regime especial de exigibilidade do IVA nas entregas de bens às cooperativas agrícolas', N'N.º 1 do art.º 5.º do anexo ao Decreto-Lei n.º 418/99, de 21 de Outubro', N'Exigibilidade de caixa'),
                ('E1410000-0000-4000-8000-000000000016', N'5',  N'M03', N'Regime especial de exigibilidade do IVA nos serivços de transporte rodoviário nacional de mercadorias', N'Anexo à Lei n.º 15/2009, de 1 de Abril', N'Exigibilidade de caixa'),
                ('E1410000-0000-4000-8000-000000000017', N'6',  N'M04', N'', N'Artigo 13.º do CIVA', N'Isento Artigo 13.º do CIVA'),
                ('E1410000-0000-4000-8000-000000000018', N'7',  N'M05', N'', N'Artigo 14.º do CIVA', N'Isento Artigo 14.º do CIVA'),
                ('E1410000-0000-4000-8000-000000000019', N'8',  N'M06', N'', N'Artigo 15.º do CIVA', N'Isento Artigo 15.º do CIVA'),
                ('E1410000-0000-4000-8000-00000000001A', N'9',  N'M07', N'ISENTO ARTIGO 9.º NR.23/24 DO CIVA', N'ARTIGO 9.º NR.23/24 DO CIVA', N'ISENTO ARTIGO 9.º NR.23/24 DO CIVA'),
                ('E1410000-0000-4000-8000-00000000001B', N'28', N'M07', N'ISENTO ARTIGO 9.º NR.1 DO CIVA', N'ARTIGO 9.º NR.1 DO CIVA', N'ISENTO ARTIGO 9.º NR.1 DO CIVA'),
                ('E1410000-0000-4000-8000-00000000001C', N'29', N'M07', N'ISENTO ARTIGO 9.º NR.2 DO CIVA', N'ARTIGO 9.º NR.2 DO CIVA', N'ISENTO ARTIGO 9.º NR.2 DO CIVA');

                INSERT INTO [Utility].[MotivoIsencao] (
                    [Id], [Codigo], [CodigoSaft], [Descricao], [Norma], [Mencao],
                    [CreatedBy], [CreatedOn]
                )
                SELECT
                    r.Id,
                    r.Codigo,
                    r.CodigoSaft,
                    r.Descricao,
                    NULLIF(LTRIM(RTRIM(r.Norma)), N''),
                    NULLIF(LTRIM(RTRIM(r.Mencao)), N''),
                    @CreatedBy,
                    @Now
                FROM @Rows r
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM [Utility].[MotivoIsencao] m
                    WHERE m.[Codigo] = r.[Codigo]
                      AND m.[DeletedOn] IS NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM [Utility].[MotivoIsencao]
                WHERE [Id] IN (
                    'E1410000-0000-4000-8000-000000000001',
                    'E1410000-0000-4000-8000-000000000002',
                    'E1410000-0000-4000-8000-000000000003',
                    'E1410000-0000-4000-8000-000000000004',
                    'E1410000-0000-4000-8000-000000000005',
                    'E1410000-0000-4000-8000-000000000006',
                    'E1410000-0000-4000-8000-000000000007',
                    'E1410000-0000-4000-8000-000000000008',
                    'E1410000-0000-4000-8000-000000000009',
                    'E1410000-0000-4000-8000-00000000000A',
                    'E1410000-0000-4000-8000-00000000000B',
                    'E1410000-0000-4000-8000-00000000000C',
                    'E1410000-0000-4000-8000-00000000000D',
                    'E1410000-0000-4000-8000-00000000000E',
                    'E1410000-0000-4000-8000-00000000000F',
                    'E1410000-0000-4000-8000-000000000010',
                    'E1410000-0000-4000-8000-000000000011',
                    'E1410000-0000-4000-8000-000000000012',
                    'E1410000-0000-4000-8000-000000000013',
                    'E1410000-0000-4000-8000-000000000014',
                    'E1410000-0000-4000-8000-000000000015',
                    'E1410000-0000-4000-8000-000000000016',
                    'E1410000-0000-4000-8000-000000000017',
                    'E1410000-0000-4000-8000-000000000018',
                    'E1410000-0000-4000-8000-000000000019',
                    'E1410000-0000-4000-8000-00000000001A',
                    'E1410000-0000-4000-8000-00000000001B',
                    'E1410000-0000-4000-8000-00000000001C'
                );
                """);
        }
    }
}
