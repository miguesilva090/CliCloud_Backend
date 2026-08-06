using CliCloud.Application.Common;
using CliCloud.Application.Services.Servicos.TipoServicoService.Specifications;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.TipoServicoService;

public sealed class TipoServicoCodigoLookup(IRepositoryAsync repository) : ITipoServicoCodigoLookup
{
  public async Task<IReadOnlyDictionary<int, string>> ObterNomesPorCodigoAsync(
    IReadOnlyCollection<int> codigos,
    int? filtroClinica = null,
    CancellationToken cancellationToken = default)
  {
    _ = cancellationToken;

    if (codigos is not { Count: > 0 })
      return new Dictionary<int, string>();

    List<TipoServico> linhas = (await repository
      .GetListAsync<TipoServico, Guid>(new TipoServicoByCodigosSpec(codigos))
      .ConfigureAwait(false))
      .ToList();

    Dictionary<int, string> resultado = [];

    foreach (IGrouping<int, TipoServico> grupo in linhas
               .Where(x => x.Codigo.HasValue)
               .GroupBy(x => x.Codigo!.Value))
    {
      // Paridade dbo.TIPO_SRV: preferir linha do filtro da clínica; senão menor filtro.
      TipoServico? escolhido = filtroClinica.HasValue
        ? grupo.FirstOrDefault(x => x.Filtro == filtroClinica.Value)
        : null;

      escolhido ??= grupo.OrderBy(x => x.Filtro ?? int.MaxValue).FirstOrDefault();

      if (escolhido is not null && !string.IsNullOrWhiteSpace(escolhido.Descricao))
        resultado[grupo.Key] = escolhido.Descricao.Trim();
    }

    return resultado;
  }
}
