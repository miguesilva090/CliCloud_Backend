using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.Filters
{
    public class MargemMedicoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MargemMedicoTableFilter()
        {
            Filters = [];
        }
    }
}
