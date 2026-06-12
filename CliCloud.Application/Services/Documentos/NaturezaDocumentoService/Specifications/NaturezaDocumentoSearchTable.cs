using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Specifications
{
    public class NaturezaDocumentoSearchTable : Specification<NaturezaDocumento>
    {
        public NaturezaDocumentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (TableFilter f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "sigla":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Sigla.Contains(f.Value));
                            break;
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Descricao.Contains(f.Value));
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Sigla);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
