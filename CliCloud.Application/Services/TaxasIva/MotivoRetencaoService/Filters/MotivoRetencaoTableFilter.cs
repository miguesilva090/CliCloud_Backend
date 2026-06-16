using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Filters
{
    public class MotivoRetencaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
