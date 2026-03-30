using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Filters
{
    public class MotivoIsencaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
