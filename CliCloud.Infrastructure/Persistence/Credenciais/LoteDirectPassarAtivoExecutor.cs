using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectPassarAtivoExecutor(ApplicationDbContext dbContext)
    : ILoteDirectPassarAtivoExecutor
{
    public async Task<PassarParaAtivoResultDTO> ExecutarAsync(
        int codigoOrganismo,
        int mesOrigem,
        int anoOrigem,
        int mesNovo,
        int anoNovo,
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction tx = await dbContext.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        int actualizados = await dbContext
            .Set<LoteDirect>()
            .Where(x =>
                x.CodigoOrganismo == codigoOrganismo
                && x.Mes == mesOrigem
                && x.Ano == anoOrigem
                && x.Historico)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(x => x.Historico, false)
                    .SetProperty(x => x.Mes, mesNovo)
                    .SetProperty(x => x.Ano, anoNovo),
                cancellationToken)
            .ConfigureAwait(false);

        await tx.CommitAsync(cancellationToken).ConfigureAwait(false);

        return new PassarParaAtivoResultDTO
        {
            CredenciaisActualizadas = actualizados,
            CodigoOrganismo = codigoOrganismo,
            MesOrigem = mesOrigem,
            AnoOrigem = anoOrigem,
            MesNovo = mesNovo,
            AnoNovo = anoNovo,
        };
    }
}
