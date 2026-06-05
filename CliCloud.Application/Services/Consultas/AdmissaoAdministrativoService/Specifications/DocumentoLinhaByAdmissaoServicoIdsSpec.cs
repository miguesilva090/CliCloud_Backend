using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

/// <summary>
/// Linhas que referenciam serviços de admissão.
/// O filtro de documento anulado é aplicado em <see cref="AdmissaoServicoFaturacaoQueryHelper"/>.
/// </summary>
public sealed class DocumentoLinhaByAdmissaoServicoIdsSpec : Specification<DocumentoLinha>
{
  public DocumentoLinhaByAdmissaoServicoIdsSpec(IEnumerable<Guid> admissaoServicoIds)
  {
    Guid[] idArray = admissaoServicoIds.Where(x => x != Guid.Empty).Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query.Where(l =>
      l.AdmissaoServicoId.HasValue && EF.Constant(idArray).Contains(l.AdmissaoServicoId.Value));
  }
}
