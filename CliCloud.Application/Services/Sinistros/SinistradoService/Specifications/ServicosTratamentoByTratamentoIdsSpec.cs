using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications;

public sealed class ServicosTratamentoByTratamentoIdsSpec : Specification<ServicoTratamento>
{
  public ServicosTratamentoByTratamentoIdsSpec(IEnumerable<Guid> tratamentoIds)
  {
    Guid[] idArray = tratamentoIds.Where(x => x != Guid.Empty).Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query
      .Where(x => EF.Constant(idArray).Contains(x.TratamentoId))
      .Include(x => x.Servico);
  }
}
