using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.Specifications;

public sealed class TipoServicoByCodigosSpec : Specification<TipoServico>
{
  public TipoServicoByCodigosSpec(IReadOnlyCollection<int> codigos)
  {
    int[] pedidos = codigos.Where(c => c > 0).Distinct().ToArray();
    if (pedidos.Length == 0)
    {
      Query.Where(_ => false);
      return;
    }

    // EF.Constant: evita OPENJSON (falha em SQL Server com compatibilidade baixa).
    Query.Where(x => x.Codigo != null && EF.Constant(pedidos).Contains(x.Codigo.Value));
  }
}
