using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications
{
    public class NaturezaDocumentoSearchList : Specification<NaturezaDocumento>
    {
        public NaturezaDocumentoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => 
                    x.Sigla.Contains(keyword) || 
                    x.Descricao.Contains(keyword));
            }


            _ = Query.OrderBy(x => x.Sigla);
        }
    }
}