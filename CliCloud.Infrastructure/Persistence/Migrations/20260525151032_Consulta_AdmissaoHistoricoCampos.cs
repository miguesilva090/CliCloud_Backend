using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliCloud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Consulta_AdmissaoHistoricoCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF COL_LENGTH(N'Consultas.Consulta', N'ConfirmaConsulta') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [ConfirmaConsulta] bit NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'DataHoraMarcacao') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [DataHoraMarcacao] datetime2 NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'HoraChegada') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [HoraChegada] time NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'MotivoConsultaId') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [MotivoConsultaId] uniqueidentifier NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'NumDestacavel') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [NumDestacavel] nvarchar(max) NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'Ordem') IS NULL
                    ALTER TABLE [Consultas].[Consulta] ADD [Ordem] int NULL;

                IF COL_LENGTH(N'Consultas.Consulta', N'MotivoConsultaId') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.indexes
                       WHERE name = N'IX_Consulta_MotivoConsultaId'
                         AND object_id = OBJECT_ID(N'Consultas.Consulta')
                   )
                    CREATE INDEX [IX_Consulta_MotivoConsultaId]
                        ON [Consultas].[Consulta] ([MotivoConsultaId]);

                IF COL_LENGTH(N'Consultas.Consulta', N'MotivoConsultaId') IS NOT NULL
                   AND OBJECT_ID(N'Consultas.MotivosConsulta') IS NOT NULL
                   AND NOT EXISTS (
                       SELECT 1
                       FROM sys.foreign_keys
                       WHERE name = N'FK_Consulta_MotivosConsulta_MotivoConsultaId'
                         AND parent_object_id = OBJECT_ID(N'Consultas.Consulta')
                   )
                    ALTER TABLE [Consultas].[Consulta]
                    ADD CONSTRAINT [FK_Consulta_MotivosConsulta_MotivoConsultaId]
                        FOREIGN KEY ([MotivoConsultaId])
                        REFERENCES [Consultas].[MotivosConsulta] ([Id]);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM sys.foreign_keys
                    WHERE name = N'FK_Consulta_MotivosConsulta_MotivoConsultaId'
                      AND parent_object_id = OBJECT_ID(N'Consultas.Consulta')
                )
                    ALTER TABLE [Consultas].[Consulta]
                    DROP CONSTRAINT [FK_Consulta_MotivosConsulta_MotivoConsultaId];

                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_Consulta_MotivoConsultaId'
                      AND object_id = OBJECT_ID(N'Consultas.Consulta')
                )
                    DROP INDEX [IX_Consulta_MotivoConsultaId] ON [Consultas].[Consulta];

                IF COL_LENGTH(N'Consultas.Consulta', N'ConfirmaConsulta') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [ConfirmaConsulta];

                IF COL_LENGTH(N'Consultas.Consulta', N'DataHoraMarcacao') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [DataHoraMarcacao];

                IF COL_LENGTH(N'Consultas.Consulta', N'HoraChegada') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [HoraChegada];

                IF COL_LENGTH(N'Consultas.Consulta', N'MotivoConsultaId') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [MotivoConsultaId];

                IF COL_LENGTH(N'Consultas.Consulta', N'NumDestacavel') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [NumDestacavel];

                IF COL_LENGTH(N'Consultas.Consulta', N'Ordem') IS NOT NULL
                    ALTER TABLE [Consultas].[Consulta] DROP COLUMN [Ordem];
                """);
        }
    }
}
