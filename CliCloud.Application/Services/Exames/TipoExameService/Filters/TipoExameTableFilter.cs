using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Exames.TipoExameService.Filters
{
    public class TipoExameTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
