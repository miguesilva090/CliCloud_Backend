using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.AnalisesService.Specifications
{
    public class AnaliseSearchTable : Specification<Analises>
    {
        public AnaliseSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "nome":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Nome != null && x.Nome.Contains(f.Value));
                            break;
                        case "unidademedida":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.UnidadeMedida != null && x.UnidadeMedida.Contains(f.Value));
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Nome);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
