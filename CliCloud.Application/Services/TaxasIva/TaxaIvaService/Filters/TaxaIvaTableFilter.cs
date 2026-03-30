using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.Filters
{
    public class TaxaIvaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public TaxaIvaTableFilter()
        {
            Filters = [];
        }
    }
}
