using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Filters
{
    public class FraquezasMuscularesTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public FraquezasMuscularesTableFilter()
        {
            Filters = [];
        }
    }
}
