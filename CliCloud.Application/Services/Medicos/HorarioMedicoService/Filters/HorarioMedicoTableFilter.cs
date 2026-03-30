using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.Filters
{
    public class HorarioMedicoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public HorarioMedicoTableFilter()
        {
            Filters = [];
        }
    }
}
