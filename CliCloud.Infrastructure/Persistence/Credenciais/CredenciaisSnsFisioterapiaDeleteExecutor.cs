using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class CredenciaisSnsFisioterapiaDeleteExecutor(ApplicationDbContext dbContext)
    : ICredenciaisSnsFisioterapiaDeleteExecutor
{
    public async Task ExecutarAsync(IList<int> indices, CancellationToken cancellationToken = default)
    {
        if (indices is not { Count: > 0 })
            return;

        int[] distintos = indices.Distinct().ToArray();

        await using IDbContextTransaction tx =
            await dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        foreach (int indice in distintos)
        {
            int afetados = await dbContext
                .Set<LoteFisioterapia>()
                .Where(x => x.Indice == indice)
                .ExecuteDeleteAsync(cancellationToken)
                .ConfigureAwait(false);

            if (afetados == 0)
                throw new InvalidOperationException($"Lote com índice {indice} não encontrado.");
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
    }
}
