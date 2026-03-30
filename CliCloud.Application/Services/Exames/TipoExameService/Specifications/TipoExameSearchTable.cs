using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.TipoExameService.Specifications
{
    public class TipoExameSearchTable : Specification<TipoExame>
    {
        public TipoExameSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "designacao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Designacao != null && x.Designacao.Contains(f.Value));
                            break;
                        case "ean":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.EAN != null && x.EAN.Contains(f.Value));
                            break;
                        case "inativo":
                            if (!string.IsNullOrWhiteSpace(f.Value) && bool.TryParse(f.Value, out var inativo))
                                _ = Query.Where(x => x.Inativo == inativo);
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Designacao);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
