using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class CredenciaisSnsAgregadoDeleteExecutor(ApplicationDbContext dbContext)
    : ICredenciaisSnsAgregadoDeleteExecutor
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
            LoteDirectAgregado? agregado = await dbContext
                .Set<LoteDirectAgregado>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Indice == indice, cancellationToken)
                .ConfigureAwait(false);

            if (agregado is null)
                throw new InvalidOperationException($"Lote com índice {indice} não encontrado.");

            await dbContext
                .Set<LoteDirectDetalhe>()
                .Where(x => x.LoteDirectAgregadoId == agregado.Id)
                .ExecuteDeleteAsync(cancellationToken)
                .ConfigureAwait(false);

            await dbContext
                .Set<LoteDirect>()
                .Where(x => x.IndiceLote == indice)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(x => x.IndiceLote, (int?)null),
                    cancellationToken)
                .ConfigureAwait(false);

            await dbContext
                .Set<LoteDirectAgregado>()
                .Where(x => x.Id == agregado.Id)
                .ExecuteDeleteAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
    }
}
