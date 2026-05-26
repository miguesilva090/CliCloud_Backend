using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications;

public sealed class EvolucoesTratamentoByTratamentoIdSpec : Specification<EvolucaoTratamento>
{
  public EvolucoesTratamentoByTratamentoIdSpec(Guid tratamentoId)
  {
    _ = Query
      .Where(x => x.TratamentoId == tratamentoId && x.DeletedOn == null)
      .OrderByDescending(x => x.CreatedOn);
  }
}
