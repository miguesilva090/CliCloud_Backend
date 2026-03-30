using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Filters
{
    public class MarcaAparelhoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];

        public MarcaAparelhoTableFilter()
        {
            Filters = [];
        }
    }
}
