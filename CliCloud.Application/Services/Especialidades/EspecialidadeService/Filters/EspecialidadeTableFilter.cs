using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.Filters
{
    public class EspecialidadeTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public EspecialidadeTableFilter()
        {
            Filters = [];
        }
    }
}
