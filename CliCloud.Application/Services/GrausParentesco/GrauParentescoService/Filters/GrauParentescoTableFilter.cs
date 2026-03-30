using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.Filters
{
    public class GrauParentescoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public GrauParentescoTableFilter()
        {
            Filters = [];
        }
    }
}
