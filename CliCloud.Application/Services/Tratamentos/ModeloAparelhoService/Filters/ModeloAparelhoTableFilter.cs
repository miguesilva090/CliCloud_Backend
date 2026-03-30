using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Filters
{
    public class ModeloAparelhoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];

        public ModeloAparelhoTableFilter()
        {
            Filters = [];
        }
    }
}
