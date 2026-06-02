using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

/// <summary>
/// Linhas de documentos não anulados que referenciam serviços de admissão.
/// </summary>
public sealed class DocumentoLinhaByAdmissaoServicoIdsSpec : Specification<DocumentoLinha>
{
  public DocumentoLinhaByAdmissaoServicoIdsSpec(IEnumerable<Guid> admissaoServicoIds)
  {
    List<Guid> ids = admissaoServicoIds.Where(x => x != Guid.Empty).Distinct().ToList();
    if (ids.Count == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query
      .Include(l => l.Documento)
      .Where(l =>
        l.AdmissaoServicoId.HasValue
        && ids.Contains(l.AdmissaoServicoId.Value)
        && l.Documento != null
        && !l.Documento.Anulado
      );
  }
}
