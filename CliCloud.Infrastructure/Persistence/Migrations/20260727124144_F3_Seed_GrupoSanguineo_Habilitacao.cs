using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Paridade enum <see cref="CliCloud.Domain.Enums.GrupoSanguineo"/> + habilitações base (combo utente).
    /// </summary>
    public partial class F3_Seed_GrupoSanguineo_Habilitacao : Migration
    {
        private const string SeedUserId = "00000000-0000-0000-0000-000000000001";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DECLARE @CreatedBy uniqueidentifier = '{SeedUserId}';
                DECLARE @Now datetime2 = SYSUTCDATETIME();

                DECLARE @Grupos TABLE (
                    Id uniqueidentifier NOT NULL,
                    Descricao nvarchar(80) NOT NULL
                );

                INSERT INTO @Grupos (Id, Descricao) VALUES
                ('F3A60000-0000-4000-8000-000000000001', N'A+'),
                ('F3A60000-0000-4000-8000-000000000002', N'A-'),
                ('F3A60000-0000-4000-8000-000000000003', N'B+'),
                ('F3A60000-0000-4000-8000-000000000004', N'B-'),
                ('F3A60000-0000-4000-8000-000000000005', N'AB+'),
                ('F3A60000-0000-4000-8000-000000000006', N'AB-'),
                ('F3A60000-0000-4000-8000-000000000007', N'O+'),
                ('F3A60000-0000-4000-8000-000000000008', N'O-'),
                ('F3A60000-0000-4000-8000-000000000009', N'Não Definido');

                INSERT INTO [Utility].[GrupoSanguineo] (
                    [Id], [Descricao], [CreatedBy], [CreatedOn]
                )
                SELECT g.Id, g.Descricao, @CreatedBy, @Now
                FROM @Grupos g
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM [Utility].[GrupoSanguineo] x
                    WHERE LTRIM(RTRIM(x.[Descricao])) = g.[Descricao]
                      AND x.[DeletedOn] IS NULL
                );

                DECLARE @Habilitacoes TABLE (
                    Id uniqueidentifier NOT NULL,
                    Descricao nvarchar(80) NOT NULL
                );

                INSERT INTO @Habilitacoes (Id, Descricao) VALUES
                ('F3B60000-0000-4000-8000-000000000001', N'Sem escolaridade'),
                ('F3B60000-0000-4000-8000-000000000002', N'4.ª classe'),
                ('F3B60000-0000-4000-8000-000000000003', N'6.º ano'),
                ('F3B60000-0000-4000-8000-000000000004', N'9.º ano'),
                ('F3B60000-0000-4000-8000-000000000005', N'12.º ano'),
                ('F3B60000-0000-4000-8000-000000000006', N'Licenciatura'),
                ('F3B60000-0000-4000-8000-000000000007', N'Mestrado'),
                ('F3B60000-0000-4000-8000-000000000008', N'Doutoramento');

                INSERT INTO [Utility].[Habilitacao] (
                    [Id], [Descricao], [CreatedBy], [CreatedOn]
                )
                SELECT h.Id, h.Descricao, @CreatedBy, @Now
                FROM @Habilitacoes h
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM [Utility].[Habilitacao] x
                    WHERE LTRIM(RTRIM(x.[Descricao])) = h.[Descricao]
                      AND x.[DeletedOn] IS NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM [Utility].[GrupoSanguineo]
                WHERE [Id] IN (
                    'F3A60000-0000-4000-8000-000000000001',
                    'F3A60000-0000-4000-8000-000000000002',
                    'F3A60000-0000-4000-8000-000000000003',
                    'F3A60000-0000-4000-8000-000000000004',
                    'F3A60000-0000-4000-8000-000000000005',
                    'F3A60000-0000-4000-8000-000000000006',
                    'F3A60000-0000-4000-8000-000000000007',
                    'F3A60000-0000-4000-8000-000000000008',
                    'F3A60000-0000-4000-8000-000000000009'
                );

                DELETE FROM [Utility].[Habilitacao]
                WHERE [Id] IN (
                    'F3B60000-0000-4000-8000-000000000001',
                    'F3B60000-0000-4000-8000-000000000002',
                    'F3B60000-0000-4000-8000-000000000003',
                    'F3B60000-0000-4000-8000-000000000004',
                    'F3B60000-0000-4000-8000-000000000005',
                    'F3B60000-0000-4000-8000-000000000006',
                    'F3B60000-0000-4000-8000-000000000007',
                    'F3B60000-0000-4000-8000-000000000008'
                );
                """);
        }
    }
}
