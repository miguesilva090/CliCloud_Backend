using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Specifications
{
    public class CategoriaProcedimentoSearchTable : Specification<CategoriaProcedimento>
    {
        public CategoriaProcedimentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Descricao != null && x.Descricao.Contains(f.Value));
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Descricao);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
