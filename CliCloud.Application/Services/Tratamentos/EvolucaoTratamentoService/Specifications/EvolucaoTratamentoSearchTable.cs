using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications
{
    public class EvolucaoTratamentoSearchTable : Specification<EvolucaoTratamento>
    {
        public EvolucaoTratamentoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {

            // filters
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid utenteId))
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            break;
                        // Neste contexto, usamos o campo de observação clínica
                        // como "descrição" pesquisável da evolução.
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x =>
                                    x.ObservacaoClinica != null &&
                                    x.ObservacaoClinica.Contains(filter.Value));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }


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
