using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.AlergiaUtenteService.Filters
{
    public class AlergiaUtenteTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
