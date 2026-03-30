using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoSearchList : Specification<TipoDocumento>
    {
        public TipoDocumentoSearchList(string? keyword = "")
        {
            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword) || x.Abreviatura.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao); // default sort order
        }
    }
}
