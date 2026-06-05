using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications;

public sealed class FaturadasConsultaIdsSpec : Specification<ConsultaFaturacao>
{
  public FaturadasConsultaIdsSpec(IEnumerable<Guid> consultaIds)
  {
    Guid[] idArray = consultaIds.Where(x => x != Guid.Empty).Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query.Where(x =>
      x.ConsultaId.HasValue
      && EF.Constant(idArray).Contains(x.ConsultaId.Value)
      && x.Faturado);
  }
}
