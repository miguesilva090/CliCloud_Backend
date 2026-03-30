using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utentes.UtenteService.Filters
{
    public class UtenteTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public UtenteTableFilter()
        {
            Filters = [];
        }
    }
}
