using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;

public sealed class SessoesTratamentoByTratamentoIdSpec : Specification<SessaoTratamento>
{
  public SessoesTratamentoByTratamentoIdSpec(Guid tratamentoId)
  {
    _ = Query
      .Where(x => x.TratamentoId == tratamentoId && x.DeletedOn == null)
      .OrderBy(x => x.NumSessao ?? int.MaxValue)
      .ThenBy(x => x.Data ?? x.CreatedOn);
  }
}
