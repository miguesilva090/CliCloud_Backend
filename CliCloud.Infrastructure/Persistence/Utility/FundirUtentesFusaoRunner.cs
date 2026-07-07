using CliCloud.Application.Services.Utility.FundirUtentesService;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Utility;

public sealed class FundirUtentesFusaoRunner(ApplicationDbContext dbContext) : IFundirUtentesFusaoRunner
{
    private sealed record FusaoUtenteColuna(string TableSchema, string TableName, string ColumnName);

    public async Task ExecutarFusaoReferenciasAsync(
        Guid utenteOrigemId,
        Guid utenteApagarId,
        CancellationToken cancellationToken = default)
    {
        List<FusaoUtenteColuna> colunas = await dbContext.Database
            .SqlQuery<FusaoUtenteColuna>(
                $"""
                SELECT
                    c.TABLE_SCHEMA AS TableSchema,
                    c.TABLE_NAME AS TableName,
                    c.COLUMN_NAME AS ColumnName
                FROM INFORMATION_SCHEMA.COLUMNS c
                INNER JOIN INFORMATION_SCHEMA.TABLES t
                    ON t.TABLE_NAME = c.TABLE_NAME
                   AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
                WHERE t.TABLE_TYPE = 'BASE TABLE'
                  AND c.DATA_TYPE = 'uniqueidentifier'
                  AND c.COLUMN_NAME = 'UtenteId'
                  AND NOT (c.TABLE_SCHEMA = 'Utentes' AND c.TABLE_NAME = 'Utente')
                ORDER BY c.TABLE_SCHEMA, c.TABLE_NAME
                """
            )
            .ToListAsync(cancellationToken);

        foreach (FusaoUtenteColuna col in colunas)
        {
            string fullName = $"[{col.TableSchema}].[{col.TableName}]";
            string colName = $"[{col.ColumnName}]";

            if (col.TableSchema == "Utentes" && col.TableName == "UtenteSubsistemaLinha")
            {
                _ = await dbContext.Database.ExecuteSqlRawAsync(
                    $"DELETE FROM {fullName} WHERE {colName} = {{0}}",
                    [utenteApagarId],
                    cancellationToken);
                continue;
            }

            await RemoverConflitosAntesDeFusaoAsync(
                col.TableSchema,
                col.TableName,
                utenteOrigemId,
                utenteApagarId,
                cancellationToken);

            _ = await dbContext.Database.ExecuteSqlRawAsync(
                $"UPDATE {fullName} SET {colName} = {{0}} WHERE {colName} = {{1}}",
                [utenteOrigemId, utenteApagarId],
                cancellationToken);
        }
    }

    /// <summary>
    /// Tabelas com índice único que inclui UtenteId: apagar linhas do utente a fundir
    /// que colidiriam com registos já existentes no utente de origem.
    /// </summary>
    private async Task RemoverConflitosAntesDeFusaoAsync(
        string tableSchema,
        string tableName,
        Guid utenteOrigemId,
        Guid utenteApagarId,
        CancellationToken cancellationToken)
    {
        if (tableSchema == "ProcessoClinico" && tableName == "RelatorioExames")
        {
            _ = await dbContext.Database.ExecuteSqlRawAsync(
                """
                DELETE a
                FROM [ProcessoClinico].[RelatorioExames] a
                INNER JOIN [ProcessoClinico].[RelatorioExames] o
                    ON o.[UtenteId] = {0} AND a.[MedicoId] = o.[MedicoId]
                WHERE a.[UtenteId] = {1}
                """,
                [utenteOrigemId, utenteApagarId],
                cancellationToken);
        }
    }
}
