using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;

public sealed class DocumentoByIdsNaoAnuladosComTipoSpec : Specification<Documento>
{
  public DocumentoByIdsNaoAnuladosComTipoSpec(IEnumerable<Guid> documentoIds)
  {
    Guid[] idArray = documentoIds.Where(x => x != Guid.Empty).Distinct().ToArray();
    if (idArray.Length == 0)
    {
      _ = Query.Where(_ => false);
      return;
    }

    _ = Query
      .Include(d => d.TipoDocumento)
      .Where(d => !d.Anulado && EF.Constant(idArray).Contains(d.Id));
  }
}
