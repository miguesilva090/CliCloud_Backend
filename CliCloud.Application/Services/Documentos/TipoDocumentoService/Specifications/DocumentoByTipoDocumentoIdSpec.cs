using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class DocumentoByTipoDocumentoIdSpec : Specification<Documento>
    {
        public DocumentoByTipoDocumentoIdSpec(Guid tipoDocumentoId)
        {
            _ = Query.Where(x => x.TipoDocumentoId == tipoDocumentoId);
        }
    }
}