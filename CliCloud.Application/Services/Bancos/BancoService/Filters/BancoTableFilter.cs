using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Bancos.BancoService.Filters
{
    public class BancoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public BancoTableFilter()
        {
            Filters = [];
        }
    }
}
