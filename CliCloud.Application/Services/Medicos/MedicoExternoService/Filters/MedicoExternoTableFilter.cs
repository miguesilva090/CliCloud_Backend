using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Medicos.MedicoExternoService.Filters
{
    public class MedicoExternoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public MedicoExternoTableFilter()
        {
            Filters = [];
        }
    }
}
