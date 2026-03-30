using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.Filters
{
    public class TipoDeDorTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public TipoDeDorTableFilter()
        {
            Filters = [];
        }
    }
}
