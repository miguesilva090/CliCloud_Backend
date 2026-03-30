using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Specifications
{
    public class LocalTratamentoSearchTable : Specification<LocalTratamento>
    {
        public LocalTratamentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "designacao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Designacao.Contains(f.Value));
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderBy(x => x.Designacao);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}
