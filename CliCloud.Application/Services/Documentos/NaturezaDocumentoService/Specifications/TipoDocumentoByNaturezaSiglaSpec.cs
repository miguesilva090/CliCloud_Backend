using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications
{
    public class TipoDocumentoByNaturezaSiglaSpec : Specification<TipoDocumento>
    {
        public TipoDocumentoByNaturezaSiglaSpec(string sigla)
        {
            _ = Query.Where(x => x.Natureza == sigla);
        }
    }
}