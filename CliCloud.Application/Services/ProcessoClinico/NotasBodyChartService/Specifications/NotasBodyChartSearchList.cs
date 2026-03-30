using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;


namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Specifications
{
    public class NotasBodyChartSearchList : Specification<NotaBodyChart>
    {
        public NotasBodyChartSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Titulo != null && x.Titulo.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
