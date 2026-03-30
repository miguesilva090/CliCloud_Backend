using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Medicos;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.Specifications
{
    public class MargemMedicoSearchTable : Specification<MargemMedico>
    {
        public MargemMedicoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            _ = Query
                .Include(x => x.Servico)
                .Include(x => x.Medico);

            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? "").ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "servico.designacao":
                        case "servicodesignacao":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x => x.Servico != null && x.Servico.Designacao.Contains(filter.Value));
                            }
                            break;
                        case "medico.nome":
                        case "mediconome":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x => x.Medico != null && x.Medico.Nome.Contains(filter.Value));
                            }
                            break;
                        case "medico.numeroContribuinte":
                        case "mediconumerocontribuinte":
                            if (!string.IsNullOrWhiteSpace(filter.Value))
                            {
                                _ = Query.Where(x => x.Medico != null && x.Medico.NumeroContribuinte != null && x.Medico.NumeroContribuinte.Contains(filter.Value));
                            }
                            break;
                        case "servicoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid servicoId))
                            {
                                _ = Query.Where(x => x.ServicoId == servicoId);
                            }
                            break;
                        case "medicoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid medicoId))
                            {
                                _ = Query.Where(x => x.MedicoId == medicoId);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
            {
                _ = Query.OrderByDescending(x => x.CreatedOn);
            }
            else
            {
                _ = Query.OrderBy(dynamicOrder);
            }
        }
    }
}
