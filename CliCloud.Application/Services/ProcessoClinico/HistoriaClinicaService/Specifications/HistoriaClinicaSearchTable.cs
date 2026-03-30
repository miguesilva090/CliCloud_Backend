using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Specifications
{
    public class HistoriaClinicaSearchTable : Specification<HistoriaClinica>
    {
        public HistoriaClinicaSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count != 0)
            {
                foreach (TableFilter filter in filters)
                {
                    switch ((filter.Id ?? string.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        case "utenteid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid utenteId))
                            {
                                _ = Query.Where(x => x.UtenteId == utenteId);
                            }
                            break;
                        case "medicoid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid medicoId))
                            {
                                _ = Query.Where(x => x.MedicoId == medicoId);
                            }
                            break;
                        case "especialidadeid":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && Guid.TryParse(filter.Value, out Guid especialidadeId))
                            {
                                _ = Query.Where(x => x.EspecialidadeId == especialidadeId);
                            }
                            break;
                        case "data":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime data))
                            {
                                _ = Query.Where(x => x.Data.Date == data.Date);
                            }
                            break;
                        case "datainicio":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime dataInicio))
                            {
                                _ = Query.Where(x => x.Data >= dataInicio.Date);
                            }
                            break;
                        case "datafim":
                            if (!string.IsNullOrWhiteSpace(filter.Value) && DateTime.TryParse(filter.Value, out DateTime dataFim))
                            {
                                _ = Query.Where(x => x.Data <= dataFim.Date);
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
