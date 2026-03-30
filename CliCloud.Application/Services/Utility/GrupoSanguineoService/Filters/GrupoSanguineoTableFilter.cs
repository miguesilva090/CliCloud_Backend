using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.Filters
{
    public class GrupoSanguineoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public GrupoSanguineoTableFilter()
        {
            Filters = [];
        }
    }
}
