using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.HistoriaClinica;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Specifications
{
    public class HistoriaClinicaMatchName : Specification<HistoriaClinica>
    {
        public HistoriaClinicaMatchName(string? keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(h => h.Obs.Contains(keyword));
            }
            _ = Query.OrderBy(h => h.Data);
        }
    }
}
