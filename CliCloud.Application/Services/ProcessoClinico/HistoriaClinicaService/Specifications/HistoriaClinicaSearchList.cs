using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;


namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Specifications
{
    public class HistoriaClinicaSearchList : Specification<HistoriaClinica>
    {
        public HistoriaClinicaSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Obs.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
