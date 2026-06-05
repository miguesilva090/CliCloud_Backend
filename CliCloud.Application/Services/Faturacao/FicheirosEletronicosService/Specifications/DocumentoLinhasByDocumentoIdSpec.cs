using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class DocumentoLinhasByDocumentoIdSpec : Specification<DocumentoLinha>
{
    public DocumentoLinhasByDocumentoIdSpec(Guid documentoId)
    {
        _ = Query.Where(x => x.DocumentoId == documentoId && x.DeletedOn == null);
    }
}
