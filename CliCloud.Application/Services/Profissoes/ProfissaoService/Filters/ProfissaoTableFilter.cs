using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.Filters
{
    public class ProfissaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public ProfissaoTableFilter()
        {
            Filters = [];
        }
    }
}
