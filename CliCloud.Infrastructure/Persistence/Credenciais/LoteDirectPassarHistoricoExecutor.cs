using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectPassarHistoricoExecutor(ApplicationDbContext dbContext)
    : ILoteDirectPassarHistoricoExecutor
{
    public async Task<PassarParaHistoricoResultDTO> ExecutarAsync(
        int codigoOrganismo,
        int mes,
        int ano,
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction tx = await dbContext.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        int actualizados = await dbContext
            .Set<LoteDirect>()
            .Where(x =>
                x.CodigoOrganismo == codigoOrganismo
                && x.Mes == mes
                && x.Ano == ano
                && !x.Historico)
            .ExecuteUpdateAsync(
                s => s.SetProperty(x => x.Historico, true),
                cancellationToken)
            .ConfigureAwait(false);

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);

        return new PassarParaHistoricoResultDTO
        {
            CredenciaisActualizadas = actualizados,
            CodigoOrganismo = codigoOrganismo,
            Mes = mes,
            Ano = ano,
        };
    }
}
