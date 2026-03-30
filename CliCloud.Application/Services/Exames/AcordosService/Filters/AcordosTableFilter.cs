using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Exames.AcordosService.Filters
{
    public class AcordosTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
