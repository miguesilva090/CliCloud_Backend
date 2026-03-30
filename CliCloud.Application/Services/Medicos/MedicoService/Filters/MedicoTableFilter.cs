using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Medicos.MedicoService.Filters
{
    public class MedicoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MedicoTableFilter()
        {
            Filters = [];
        }
    }
}
