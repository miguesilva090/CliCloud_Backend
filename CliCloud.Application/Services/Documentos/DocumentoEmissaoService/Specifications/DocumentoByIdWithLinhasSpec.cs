using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class DocumentoByIdWithLinhasSpec : Specification<Documento>
{
    public DocumentoByIdWithLinhasSpec(Guid id)
    {
        _ = Query
            .Where(x => x.Id == id && x.DeletedOn == null)
            .Include(x => x.Linhas);
    }
}