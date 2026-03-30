using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Filters
{
    public class MotivosDesmarcacaoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MotivosDesmarcacaoTableFilter()
        {
            Filters = [];
        }
    }
}
