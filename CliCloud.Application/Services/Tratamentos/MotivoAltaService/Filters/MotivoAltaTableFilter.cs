using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.Filters
{
    public class MotivoAltaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MotivoAltaTableFilter()
        {
            Filters = [];
        }
    }
}
