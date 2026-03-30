using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoMatchAbreviatura : Specification<TipoDocumento>
    {
        public TipoDocumentoMatchAbreviatura(string abreviatura)
        {
            _ = Query.Where(x => x.Abreviatura == abreviatura);
        }
    }
}
