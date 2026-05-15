using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Move_TipoLotes_To_Credenciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF SCHEMA_ID(N'Credenciais') IS NULL
                BEGIN
                    EXEC(N'CREATE SCHEMA [Credenciais]');
                END

                IF OBJECT_ID(N'Credenciais.TipoLotes', N'U') IS NULL
                BEGIN
                    CREATE TABLE [Credenciais].[TipoLotes]
                    (
                        [Id] INT NOT NULL,
                        [Valor] INT NULL,
                        [Designa] NVARCHAR(50) NULL,
                        CONSTRAINT [PK_TipoLotes] PRIMARY KEY ([Id])
                    );
                END

                IF OBJECT_ID(N'dbo.TIPOLOTES', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO [Credenciais].[TipoLotes] ([Id], [Valor], [Designa])
                    SELECT [codigo], [valor], [designa]
                    FROM [dbo].[TIPOLOTES] AS [source]
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM [Credenciais].[TipoLotes] AS [target]
                        WHERE [target].[Id] = [source].[codigo]
                    );

                    DROP TABLE [dbo].[TIPOLOTES];
                END
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'dbo.TIPOLOTES', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[TIPOLOTES]
                    (
                        [codigo] INT NOT NULL,
                        [valor] INT NULL,
                        [designa] NVARCHAR(50) NULL,
                        CONSTRAINT [PK_TIPOLOTES] PRIMARY KEY ([codigo])
                    );
                END

                IF OBJECT_ID(N'Credenciais.TipoLotes', N'U') IS NOT NULL
                BEGIN
                    INSERT INTO [dbo].[TIPOLOTES] ([codigo], [valor], [designa])
                    SELECT [Id], [Valor], [Designa]
                    FROM [Credenciais].[TipoLotes] AS [source]
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM [dbo].[TIPOLOTES] AS [target]
                        WHERE [target].[codigo] = [source].[Id]
                    );

                    DROP TABLE [Credenciais].[TipoLotes];
                END
                """
            );
        }
    }
}
