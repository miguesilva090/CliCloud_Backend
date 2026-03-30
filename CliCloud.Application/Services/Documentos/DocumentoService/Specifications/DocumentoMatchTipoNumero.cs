using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoMatchTipoNumero : Specification<Documento>
    {
        public DocumentoMatchTipoNumero(Guid tipoDocumentoId, int numeroDocumento)
        {
            _ = Query.Where(x => x.TipoDocumentoId == tipoDocumentoId && x.NumeroDocumento == numeroDocumento);
        }
    }
}
