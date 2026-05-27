using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class DocumentoOrigemClinicaByDocumentoIdSpec : Specification<DocumentoOrigemClinica>
{
    public DocumentoOrigemClinicaByDocumentoIdSpec(Guid documentoId)
    {
        _ = Query.Where(x => x.DocumentoId == documentoId && x.DeletedOn == null);
    }
}