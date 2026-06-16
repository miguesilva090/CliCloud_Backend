using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Specifications
{
    public class MotivoRetencaoSearchTable : Specification<MotivoRetencao>
    {
        public MotivoRetencaoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "codigo":
                            if (int.TryParse(f.Value, out var codigo))
                                _ = Query.Where(x => x.Codigo == codigo);
                            break;
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Descricao.Contains(f.Value));
                            break;
                        case "tipoimposto":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.TipoImposto == f.Value);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Codigo);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
