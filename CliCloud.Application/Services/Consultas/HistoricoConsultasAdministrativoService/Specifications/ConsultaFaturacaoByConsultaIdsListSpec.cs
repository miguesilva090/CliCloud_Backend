using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Specifications;

public sealed class ConsultaFaturacaoByConsultaIdsListSpec : Specification<ConsultaFaturacao>
{
  public ConsultaFaturacaoByConsultaIdsListSpec(IEnumerable<Guid> consultaIds)
  {
    Guid[] idArray = consultaIds.Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query.Where(x =>
      x.ConsultaId != null
      && EF.Constant(idArray).Contains(x.ConsultaId.Value)
      && x.DeletedOn == null);
  }
}
