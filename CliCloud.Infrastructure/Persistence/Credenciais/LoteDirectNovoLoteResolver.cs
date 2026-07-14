using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectNovoLoteResolver(ApplicationDbContext dbContext) : ILoteDirectNovoLoteResolver
{
    private const int MaxCredenciaisPorLote = 30;
    private const int TipoLoteValorExamesSemPapel = 97;

    public async Task<(int NovoIndice, int NovoLote)> ResolverAsync(
        int codigoOrganismo,
        int tipoLote,
        int tipoServico,
        int mes,
        int ano,
        CancellationToken cancellationToken = default)
    {
        List<LoteDirectAgregado> agregados = await dbContext
            .Set<LoteDirectAgregado>()
            .AsNoTracking()
            .Where(x =>
                x.CodigoOrganismo == codigoOrganismo
                && x.TipoLote == tipoLote
                && x.TipoServico == tipoServico
                && x.Mes == mes
                && x.Ano == ano)
            .OrderBy(x => x.NumeroLote)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (agregados.Count == 0)
        {
            int proximoIndice = await ObterProximoIndiceAsync(cancellationToken).ConfigureAwait(false);
            return (proximoIndice, 1);
        }

        bool isTipo97 = await dbContext
            .Set<TipoLote>()
            .AsNoTracking()
            .Where(x => x.Id == tipoLote)
            .Select(x => x.Valor == TipoLoteValorExamesSemPapel)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (isTipo97)
            return (agregados[0].Indice, agregados[0].NumeroLote);

        LoteDirectAgregado? comEspaco = agregados.FirstOrDefault(x => x.NumeroRequisicoes < MaxCredenciaisPorLote);
        if (comEspaco is not null)
            return (comEspaco.Indice, comEspaco.NumeroLote);

        int novoIndice = await ObterProximoIndiceAsync(cancellationToken).ConfigureAwait(false);
        return (novoIndice, agregados.Count + 1);
    }

    private async Task<int> ObterProximoIndiceAsync(CancellationToken cancellationToken)
    {
        int maxIndice = await dbContext
            .Set<LoteDirectAgregado>()
            .MaxAsync(x => (int?)x.Indice, cancellationToken)
            .ConfigureAwait(false) ?? 0;

        return maxIndice + 1;
    }
}
