using Ardalis.Specification;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Specifications
{
    public class RelatorioAtestadoSearchTable : Specification<RelatorioAtestado>
    {
        public RelatorioAtestadoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    switch (filter.Id.ToLowerInvariant())
                    {
                        case "titulo":
                            _ = Query.Where(x => x.Titulo.Contains(filter.Value));
                            break;
                        case "textohtml":
                            _ = Query.Where(x => x.TextoHtml.Contains(filter.Value));
                            break;
                        default:
                            break;
                    }
                }
            }


            // incluir médico para permitir mapear dados derivados no DTO
            _ = Query.Include(x => x.Medico);

            // sort order
            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder); // dynamic (JQDT) sort order
            }


        }
    }
}
