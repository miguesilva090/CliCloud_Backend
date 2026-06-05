using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class AdmissaoServicoByIdsSpec : Specification<AdmissaoServico>
{
  public AdmissaoServicoByIdsSpec(IEnumerable<Guid> ids)
  {
    Guid[] idArray = ids.Where(x => x != Guid.Empty).Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query.Where(x => EF.Constant(idArray).Contains(x.Id));
  }
}
