using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class DocumentoOrigemClinicaByDocumentoAdmissaoSpec : Specification<DocumentoOrigemClinica>
{
  public DocumentoOrigemClinicaByDocumentoAdmissaoSpec(Guid documentoId, Guid admissaoId)
  {
    _ = Query.Where(x => x.DocumentoId == documentoId && x.AdmissaoId == admissaoId);
  }
}
