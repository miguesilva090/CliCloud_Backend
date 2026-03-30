using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Organismos.OrganismoService.Filters
{
    public class OrganismoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public OrganismoTableFilter()
        {
            Filters = [];
        }
    }
}
