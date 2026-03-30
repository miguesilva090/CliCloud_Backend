using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.Filters
{
    public class HabilitacaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public HabilitacaoTableFilter()
        {
            Filters = [];
        }
    }
}
