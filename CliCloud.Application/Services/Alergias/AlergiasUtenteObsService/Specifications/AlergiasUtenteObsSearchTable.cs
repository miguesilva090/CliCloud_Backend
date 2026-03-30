using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.Alergias;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.Specifications
{
    public class AlergiasUtenteObsSearchTable : Specification<AlergiasUtenteObs>
    {
        public AlergiasUtenteObsSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "observacoes":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.Observacoes != null && x.Observacoes.Contains(filter.Value));
                            break;
                        case "informacaoimportante":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                                _ = Query.Where(x => x.InformacaoImportante != null && x.InformacaoImportante.Contains(filter.Value));
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderByDescending(x => x.CreatedOn);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
