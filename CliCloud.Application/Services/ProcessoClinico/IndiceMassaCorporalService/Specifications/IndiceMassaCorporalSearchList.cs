using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;

namespace CliCloud.Application.Services.IndiceMassaCorporalService.Specifications
{
    public class IndiceMassaCorporalSearchList : Specification<IndiceMassaCorporal>
    {
        public IndiceMassaCorporalSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.UtenteId.ToString().Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
