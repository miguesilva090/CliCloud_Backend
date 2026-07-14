using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectEspHistoricoGateway(ApplicationDbContext dbContext) : ILoteDirectEspHistoricoGateway
{
    public async Task UpdateParaHistoricoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        if (!await TabelaDisponivelAsync(cancellationToken).ConfigureAwait(false))
            return;

        _ = await dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE dbo.RequisicoesEsp
            SET Historico = 1
            WHERE NumeroRequisicao = {0} AND ISNULL(Apagado, 0) = 0
            """,
            [numeroRequisicao.Trim()],
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task UpdateParaAtivoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        if (!await TabelaDisponivelAsync(cancellationToken).ConfigureAwait(false))
            return;

        _ = await dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE dbo.RequisicoesEsp
            SET Historico = 0
            WHERE NumeroRequisicao = {0} AND ISNULL(Apagado, 0) = 0
            """,
            [numeroRequisicao.Trim()],
            cancellationToken
        ).ConfigureAwait(false);
    }

    private async Task<bool> TabelaDisponivelAsync(CancellationToken cancellationToken)
    {
        int? objectId = await dbContext.Database
            .SqlQuery<int?>(
                $"""
                SELECT OBJECT_ID(N'dbo.RequisicoesEsp', N'U') AS Value
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return objectId is > 0;
    }
}
