using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Alergias.AlergiaService.Filters
{
    public class AlergiaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
