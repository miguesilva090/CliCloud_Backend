using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class CredenciaisSnsFisioterapiaDeleteExecutor(ApplicationDbContext dbContext) 
    : ICredenciaisSnsFisioterapiaDeleteExecutor
{
    public async Task ExecutarAsync(IList<int> indices, CancellationToken cancellationToken = default)
    {
        if (indices is not { Count: > 0 })
            return;
        
        if (!await TabelaDisponivelAsync(cancellationToken).ConfigureAwait(false))
            throw new InvalidOperationException("Tabela dbo.LOTESPFISIO não disponível.");

        int[] distintos = indices.Distinct().ToArray();

        await using IDbContextTransaction tx =
            await dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        foreach ( int indice in distintos)
        {
            int afetados = await dbContext.Database.ExecuteSqlRawAsync(
                """
                DELETE FROM dbo.LOTESPFISIO
                WHERE indice = {0}
                """,
                [indice],
                cancellationToken
            ).ConfigureAwait(false);

            if (afetados == 0)
                throw new InvalidOperationException($"Lote com índice {indice} não encontrado ");
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<bool> TabelaDisponivelAsync(CancellationToken cancellationToken )
    {
        int? objectid = await dbContext.Database
            .SqlQuery<int?>(
                $"""
                SELECT OBJECT_ID(N'dbo.LOTESPFISIO', N'U') AS Value
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return objectid is > 0;
    }
}