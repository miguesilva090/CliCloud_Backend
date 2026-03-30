using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Exames.AnalisesService.Filters
{
    public class AnaliseTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
