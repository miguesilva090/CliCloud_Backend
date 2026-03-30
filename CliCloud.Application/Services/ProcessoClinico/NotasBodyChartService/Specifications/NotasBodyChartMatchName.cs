using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Specifications
{
    public class NotasBodyChartMatchName : Specification<NotaBodyChart>
    {
        public NotasBodyChartMatchName(string? keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(h => h.Titulo != null && h.Titulo.Contains(keyword));
            }
            _ = Query.OrderBy(h => h.Titulo);
        }
    }
}
