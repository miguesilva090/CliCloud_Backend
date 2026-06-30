using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class CredenciaisSnsLegadoLookup(ApplicationDbContext dbContext)
    : ICredenciaisSnsLegadoLookup
{
    private sealed class TipoSrvLegadoRow
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Filtro { get; set; }
    }

    public async Task<IReadOnlyDictionary<int, string>> ObterNomesTipoServicoAsync(
        IReadOnlyCollection<int> codigosTipoServico,
        int? filtroLegado = null,
        CancellationToken cancellationToken = default)
    {
        if (codigosTipoServico is not { Count: > 0 })
            return new Dictionary<int, string>();

        HashSet<int> pedidos = codigosTipoServico.Where(x => x > 0).ToHashSet();
        if (pedidos.Count == 0)
            return new Dictionary<int, string>();

        if (!await TipoSrvLegadoDisponivelAsync(cancellationToken).ConfigureAwait(false))
            return new Dictionary<int, string>();

        List<TipoSrvLegadoRow> linhas = await dbContext.Database
            .SqlQuery<TipoSrvLegadoRow>(
                $"""
                SELECT ts.c_tipo_srv AS Codigo, ts.nome AS Nome, ts.filtro AS Filtro
                FROM dbo.TIPO_SRV ts
                """
            )
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        Dictionary<int, string> resultado = [];

        foreach (IGrouping<int, TipoSrvLegadoRow> grupo in linhas
                     .Where(x => pedidos.Contains(x.Codigo))
                     .GroupBy(x => x.Codigo))
        {
            TipoSrvLegadoRow? escolhido = filtroLegado.HasValue
                ? grupo.FirstOrDefault(x => x.Filtro == filtroLegado.Value)
                : null;

            escolhido ??= grupo.OrderBy(x => x.Filtro).FirstOrDefault();

            if (escolhido is not null && !string.IsNullOrWhiteSpace(escolhido.Nome))
                resultado[grupo.Key] = escolhido.Nome.Trim();
        }

        return resultado;
    }

    private async Task<bool> TipoSrvLegadoDisponivelAsync(CancellationToken cancellationToken)
    {
        int? objectId = await dbContext.Database
            .SqlQuery<int?>(
                $"""
                SELECT OBJECT_ID(N'dbo.TIPO_SRV', N'U') AS Value
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return objectId is > 0;
    }
}
