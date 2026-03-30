using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Moedas.MoedaService.Filters
{
    public class MoedaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MoedaTableFilter()
        {
            Filters = [];
        }
    }
}
